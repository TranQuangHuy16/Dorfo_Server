using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;
using Dorfo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRedisCartService _redis;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IRedisCartService redis, IUnitOfWork unitOfWork)
        {
            _redis = redis;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderResponse?> CreateOrderAsync(CreateOrderRequest request, Guid userId)
        {
            //var cart = await _redis.GetCartByIdAsync(request.CartId);
            var cart = await _redis.GetCartByIdAsync(userId, request.CartId);
            if (cart == null || !cart.Items.Any())
                return null;

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                OrderRef = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{userId.ToString().Substring(0, 6)}",
                UserId = userId,
                MerchantId = cart.Merchant.MerchantId,
                DeliveryAddressId = request.DeliveryAddressId,
                //DeliveryPoint = request.DeliveryPoint,
                PaymentMethodId = 1,

                SubTotal = cart.SubTotal,
                DeliveryFee = cart.DeliveryFee,
                ServiceFee = cart.ServiceFee,
                DiscountAmount = cart.Discount,
                TotalAmount = cart.TotalAmount,

                CreatedAt = DateTime.UtcNow,
                Status = OrderStatusEnum.PENDING,
                Notes = request.Notes
            };

            // map Cart -> OrderItems
            foreach (var ci in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    OrderId = order.OrderId,
                    MenuItemId = ci.MenuItemId,
                    ItemName = ci.MenuItemName,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.PriceAtAdd,
                    SubTotal = (ci.PriceAtAdd + ci.Options.SelectMany(o => o.SelectedValues).Sum(v => v.PriceDelta)) * ci.Quantity
                };

                foreach (var opt in ci.Options)
                {
                    var orderItemOpt = new OrderItemOption
                    {
                        OrderItemOptionId = Guid.NewGuid(),
                        OrderItemId = orderItem.OrderItemId,
                        MenuItemOptionId = opt.OptionId,
                        OptionName = opt.OptionName
                    };

                    foreach (var val in opt.SelectedValues)
                    {
                        orderItemOpt.OrderItemOptionValue.Add(new OrderItemOptionValue
                        {
                            OrderItemOptionValueId = Guid.NewGuid(),
                            OrderItemOptionId = orderItemOpt.OrderItemOptionId,
                            MenuItemOptionValueId = val.OptionValueId,
                            ValueName = val.ValueName ?? string.Empty,
                            PriceDelta = val.PriceDelta
                        });
                    }

                    orderItem.OrderItemOptions.Add(orderItemOpt);
                }

                order.Items.Add(orderItem);
            }

            // save order
            await _unitOfWork.OrderRepository.AddAsync(order);

            // clear cart
            //await _redis.RemoveCartAsync(userId);
            await _redis.RemoveCartAsync(userId, cart.Merchant.MerchantId);

            return MapToResponse(order, cart.Merchant.MerchantName);
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null) return null;
            return MapToResponse(order, order.Merchant?.Name ?? "");
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersByUserAsync(Guid userId)
        {
            var orders = await _unitOfWork.OrderRepository.GetByUserAsync(userId);
            return orders.Select(o => MapToResponse(o, o.Merchant?.Name ?? "")).ToList();
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync()
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync();
            return orders.Select(o => MapToResponse(o, o.Merchant?.Name ?? "")).ToList();
        }


        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatusEnum status)
        {
            return await _unitOfWork.OrderRepository.UpdateStatus(orderId, status);
        }

        private OrderResponse MapToResponse(Order order, string merchantName)
        {
            return new OrderResponse
            {
                OrderId = order.OrderId,
                OrderRef = order.OrderRef,
                MerchantId = order.MerchantId,
                MerchantName = merchantName,
                SubTotal = order.SubTotal,
                DeliveryFee = order.DeliveryFee,
                ServiceFee = order.ServiceFee,
                Discount = order.DiscountAmount,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemResponse
                {
                    OrderItemId = i.OrderItemId,
                    ItemName = i.ItemName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    SubTotal = i.SubTotal,
                    Options = i.OrderItemOptions.Select(o => new OrderItemOptionResponse
                    {
                        OrderItemOptionId = o.OrderItemOptionId,
                        OptionName = o.OptionName,
                        Values = o.OrderItemOptionValue.Select(v => new OrderItemOptionValueResponse
                        {
                            OrderItemOptionValueId = v.OrderItemOptionValueId,
                            ValueName = v.ValueName,
                            PriceDelta = v.PriceDelta
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
        }
    }
}
