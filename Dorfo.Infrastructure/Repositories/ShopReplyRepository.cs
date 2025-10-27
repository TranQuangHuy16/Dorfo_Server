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
    public class ShopReplyRepository : IShopReplyRepository
    {
        private readonly DorfoDbContext _context;
        public ShopReplyRepository(DorfoDbContext context) => _context = context;

        public async Task<ShopReply?> GetByReviewIdAsync(Guid reviewId)
        {
            return await _context.ShopReplies.FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        public async Task AddAsync(ShopReply reply)
        {
            await _context.ShopReplies.AddAsync(reply);
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
