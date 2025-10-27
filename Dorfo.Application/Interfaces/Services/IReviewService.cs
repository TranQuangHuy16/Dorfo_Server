using Dorfo.Application.DTOs.Requests;
using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IReviewService
    {
        /// <summary>
        /// Lấy danh sách review theo Merchant (bao gồm ảnh, user name, reply)
        /// </summary>
        Task<List<ReviewRequest>> GetReviewsByMerchantAsync(Guid merchantId);

        /// <summary>
        /// Tạo review mới
        /// </summary>
        Task<Review> CreateReviewAsync(Guid customerId, CreateReviewRequest request);

        /// <summary>
        /// Merchant reply cho review (chỉ reply 1 lần duy nhất)
        /// </summary>
        Task<ShopReply?> AddShopReplyAsync(Guid reviewId, CreateReplyRequest request);
    }
}
