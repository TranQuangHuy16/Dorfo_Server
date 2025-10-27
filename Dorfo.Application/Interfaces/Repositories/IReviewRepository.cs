using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetByMerchantIdAsync(Guid merchantId);
        Task<Review?> GetByIdAsync(Guid reviewId);
        Task AddAsync(Review review);
        Task SaveChangesAsync();
    }
}
