using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Requests
{
    public class ReviewRequest
    {
        public Guid ReviewId { get; set; }
        public int RatingProduct { get; set; }
        public string Comment { get; set; }
        public DateTime SentAt { get; set; }

        // User info
        public string? CustomerName { get; set; }
        public string? CustomerAvatar { get; set; }

        // Merchant info
        public Guid MerchantId { get; set; }
        public string? MerchantName { get; set; }

        public List<string> ImageUrls { get; set; } = new();

        public ShopReplyRequest? Reply { get; set; }
    }

    public class ShopReplyRequest
    {
        public Guid ShopReplyId { get; set; }
        public string Message { get; set; }
        public DateTime RepliedAt { get; set; }

        public string? MerchantName { get; set; }
    }

    public class CreateReviewRequest
    {
        //public Guid CustomerId { get; set; }
        public Guid MerchantId { get; set; }
        public int RatingProduct { get; set; }
        public string Comment { get; set; }
        public List<string>? ImageUrls { get; set; }
    }

    public class CreateReplyRequest
    {
        public Guid MerchantId { get; set; }
        public string Message { get; set; }
    }
}
