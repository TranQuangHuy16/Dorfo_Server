using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ReviewRequest>> GetReviewsByMerchantAsync(Guid merchantId)
        {
            var reviews = await _unitOfWork.ReviewRepository.GetByMerchantIdAsync(merchantId);

            return reviews.Select(r => new ReviewRequest
            {
                ReviewId = r.ReviewId,
                MerchantId = r.MerchantId,
                MerchantName = r.Merchant?.Name ?? "Unknown Merchant", // ✅ tránh null
                RatingProduct = r.RatingProduct,
                Comment = r.Comment,
                SentAt = r.SentAt,
                CustomerName = r.Customer?.DisplayName ?? "Anonymous", // ✅ tránh null
                ImageUrls = r.Images != null
                    ? r.Images.Select(i => i.ImgUrl).ToList()
                    : new List<string>(), // ✅ tránh null
                Reply = r.ShopReply == null ? null : new ShopReplyRequest
                {
                    ShopReplyId = r.ShopReply.ShopReplyId,
                    MerchantName = r.Merchant.Name ?? "Unknown",
                    Message = r.ShopReply.Message,
                    RepliedAt = r.ShopReply.RepliedAt
                }
            }).ToList();
        }


        public async Task<Review> CreateReviewAsync(Guid customerId, CreateReviewRequest request)
        {
            var review = new Review
            {
                ReviewId = Guid.NewGuid(),
                CustomerId = customerId,
                MerchantId = request.MerchantId,
                RatingProduct = request.RatingProduct,
                Comment = request.Comment,
                SentAt = DateTime.UtcNow,
                Images = new List<ReviewImage>()
            };

            // Nếu có danh sách ảnh thì thêm vào
            if (request.ImageUrls != null && request.ImageUrls.Any())
            {
                foreach (var url in request.ImageUrls)
                {
                    review.Images.Add(new ReviewImage
                    {
                        ReviewImageId = Guid.NewGuid(),
                        ReviewId = review.ReviewId,
                        ImgUrl = url
                    });
                }
            }


            await _unitOfWork.ReviewRepository.AddAsync(review);
            await _unitOfWork.ReviewRepository.SaveChangesAsync();
            return review;
        }

        public async Task<ShopReply?> AddShopReplyAsync(Guid reviewId, CreateReplyRequest request)
        {
            // 1️⃣ Kiểm tra review tồn tại
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
            if (review == null)
                throw new Exception("Review not found.");

            // 2️⃣ Kiểm tra review đã có reply chưa
            var existing = await _unitOfWork.ShopReplyRepository.GetByReviewIdAsync(reviewId);
            if (existing != null)
                return null;

            // 3️⃣ Kiểm tra Merchant tồn tại (đảm bảo không vi phạm FK)
            var merchant = await _unitOfWork.MerchantRepository.GetMerchantByIdAsync(review.MerchantId);
            if (merchant == null)
                throw new Exception("Merchant not found.");

            // 4️⃣ Tạo reply
            var reply = new ShopReply
            {
                ShopReplyId = Guid.NewGuid(),
                ReviewId = reviewId,
                MerchantId = review.MerchantId, // ✅ Lấy từ review, KHÔNG lấy từ request
                Message = request.Message,
                RepliedAt = DateTime.UtcNow
            };

            await _unitOfWork.ShopReplyRepository.AddAsync(reply);
            await _unitOfWork.ShopReplyRepository.SaveChangesAsync();
            return reply;
        }

    }
}
