using Dorfo.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IMerchantRepository MerchantRepository { get; }
        IMerchantOpeningDayRepository MerchantOpeningDayRepository { get; }
        IMenuCategoryRepository MenuCategoryRepository { get; }
        IMenuItemRepository MenuItemRepository { get; }
        int SaveChangesWithTransaction();
        Task<int> SaveChangesWithTransactionAsync();
    }
}
