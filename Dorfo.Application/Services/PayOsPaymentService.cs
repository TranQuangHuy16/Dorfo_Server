using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;
using Dorfo.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using QRCoder;
using System.Drawing;
using Dorfo.Shared.Helpers;
using System.Data.SqlTypes;

namespace Dorfo.Application.Services
{
    public class PayOsPaymentService : IPaymentService
    {
        private readonly PayOS _payOsClient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisCartService _redis;
        private readonly string _returnUrl;
        private readonly string _cancelUrl;
        private readonly IConfiguration _config;

        public PayOsPaymentService(
            IConfiguration config,
            IUnitOfWork unitOfWork,
            IRedisCartService redis,
            IConfiguration configuration,
            PayOS payOS)
        {
            _unitOfWork = unitOfWork;
            _redis = redis;
            _config = configuration;

            //var clientId = config["PayOS:ClientId"] ?? throw new ArgumentNullException("PayOS:ClientId");
            //var apiKey = config["PayOS:ApiKey"] ?? throw new ArgumentNullException("PayOS:ApiKey");
            //var checksumKey = config["PayOS:ChecksumKey"] ?? throw new ArgumentNullException("PayOS:ChecksumKey");

            //_returnUrl = config["PayOS:ReturnUrl"] ?? "";
            //_cancelUrl = _returnUrl + "?status=cancelled";

            //_payOsClient = new PayOS(clientId, apiKey, checksumKey);
            _payOsClient = payOS;
        }

        public async Task<PaymentResponse> CheckoutAsync(Guid userId, Guid merchantId)
        {
            var cart = await _redis.GetCartByMerchantAsync(userId, merchantId);
            if (cart == null) throw new Exception("Cart not found");

            // 1. Tạo Order trong DB
            var orderRef = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}".Substring(0, 24);
            var orderCode = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // long
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                OrderRef = orderRef,
                OrderCode = orderCode,
                UserId = userId,
                MerchantId = merchantId,
                SubTotal = cart.SubTotal,
                DeliveryFee = cart.DeliveryFee,
                ServiceFee = cart.ServiceFee,
                DiscountAmount = cart.Discount,
                TotalAmount = cart.TotalAmount,
                Status = OrderStatusEnum.WAITING_FOR_PAYMENT,
                CreatedAt = DateTime.UtcNow,
                PaymentMethodId = 2, // PayOS
                Items = cart.Items.Select(i => new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    MenuItemId = i.MenuItemId,
                    ItemName = i.MenuItemName,
                    Quantity = i.Quantity,
                    UnitPrice = i.PriceAtAdd,
                    SubTotal = (i.PriceAtAdd + i.Options.SelectMany(o => o.SelectedValues).Sum(v => v.PriceDelta)) * i.Quantity,
                    OrderItemOptions = i.Options.Select(o => new OrderItemOption
                    {
                        OrderItemOptionId = Guid.NewGuid(),
                        MenuItemOptionId = o.OptionId,
                        OptionName = o.OptionName,
                        OrderItemOptionValue = o.SelectedValues.Select(v => new OrderItemOptionValue
                        {
                            OrderItemOptionValueId = Guid.NewGuid(),
                            MenuItemOptionValueId = v.OptionValueId,
                            ValueName = v.ValueName,
                            PriceDelta = v.PriceDelta
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
            await _unitOfWork.OrderRepository.AddAsync(order);

            // 2. Chuẩn bị PaymentData cho PayOS

            var items = order.Items.Select(i => new ItemData(
                name: i.ItemName,
                quantity: i.Quantity,
                price: (int)Math.Round(i.UnitPrice) // ép int
            )).ToList();

            var paymentData = new PaymentData(
                orderCode: orderCode,
                amount: (int)Math.Round(order.TotalAmount),
                description: order.OrderRef,
                items: items,
                cancelUrl: "null",
                returnUrl: "null"
            //cancelUrl: _cancelUrl,
            //returnUrl: _returnUrl
            );

            // 3. Tạo link thanh toán với SDK
            var result = await _payOsClient.createPaymentLink(paymentData);

            // 4. Lưu Payment vào DB
            var payment = new Payment
            {
                PaymentId = Guid.NewGuid(),
                OrderId = order.OrderId,
                PaymentMethodId = 2, // PayOS
                Amount = order.TotalAmount,
                Status = PaymentStatusEnum.PENDING,
                ProviderReference = result.orderCode.ToString(),
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.PaymentRepository.CreateAsync(payment);
            Console.WriteLine(result.qrCode);

            var qrCode = QRCodeHelper.GenerateQRCodeAsBytes(result.qrCode);
            var fileName = $"QRCode_{Guid.NewGuid()}.png";
            var imageUrl = await CloudinaryStorageHelper.UploadImageAsync(qrCode, fileName, _config);

            //var img = await CloudinaryStorageHelper.UploadImageAsync(qrCode, _config);
            // 5. Trả về cho FE
            return new PaymentResponse
            {
                Provider = "PayOS",
                ProviderReference = result.orderCode.ToString(),
                QrImage = imageUrl,
                PaymentUrl = result.checkoutUrl ?? null,
                Amount = order.TotalAmount,
                OrderRef = order.OrderRef
            };



        }

        public async Task<bool> UpdatePaymentStatus(Guid orderId, PaymentStatusEnum status)
        {
            return await _unitOfWork.PaymentRepository.UpdatePaymentStatus(orderId, status);

        }

    }
}

