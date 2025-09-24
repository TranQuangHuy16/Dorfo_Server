using Dorfo.Domain.Entities;
using Dorfo.Domain.Enums;

public class Order
{
    public Guid OrderId { get; set; }
    public string OrderRef { get; set; } = null!;
    public long OrderCode { get; set; }
    public Guid UserId { get; set; }
    public Guid MerchantId { get; set; }
    public Guid? DeliveryAddressId { get; set; }
    public string? DeliveryPoint { get; set; }

    public int PaymentMethodId { get; set; }
    public DateTime? ScheduledFor { get; set; }
    public bool IsScheduled { get; set; } // computed column

    public decimal SubTotal { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal ServiceFee { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? Notes { get; set; }

    // ✅ Enum trạng thái đơn hàng
    public OrderStatusEnum Status { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Merchant Merchant { get; set; } = null!;
    public Address? DeliveryAddress { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = null!;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();
}
