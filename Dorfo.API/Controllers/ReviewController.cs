using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.Exceptions;
using Dorfo.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;

        public ReviewController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        [HttpGet("merchant/{merchantId}")]
        public async Task<IActionResult> GetReviewsByMerchant(Guid merchantId)
        {
            var reviews = await _serviceProviders.ReviewService.GetReviewsByMerchantAsync(merchantId);
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid token");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var review = await _serviceProviders.ReviewService.CreateReviewAsync(userId, request);
            return Ok(new { message = "Tạo review thành công.", data = review });
        }

        [HttpPost("{reviewId}/reply")]
        public async Task<IActionResult> ReplyReview(Guid reviewId, [FromBody] CreateReplyRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reply = await _serviceProviders.ReviewService.AddShopReplyAsync(reviewId, request);

            if (reply == null)
                return BadRequest(new { message = "Review này đã được reply trước đó rồi." });

            return Ok(new { message = "Reply thành công.", data = reply });
        }
    }
}
