using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Domain.Entities;
using Dorfo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DorfoDbContext _context;
        public ReviewRepository(DorfoDbContext context) => _context = context;

        public async Task<List<Review>> GetByMerchantIdAsync(Guid merchantId)
        {
            return await _context.Reviews
                .Include(r => r.Customer)
                .Include(r => r.Images)
                .Include(r => r.ShopReply)
                .Include(r => r.Merchant)
                .Where(r => r.MerchantId == merchantId)
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(Guid reviewId)
        {
            return await _context.Reviews
                .Include(r => r.ShopReply)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
