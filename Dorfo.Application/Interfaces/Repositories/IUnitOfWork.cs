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
        IMenuItemOptionRepository MenuItemOptionRepository { get; }
        IMenuItemOptionValueRepository MenuItemOptionValueRepository { get; }
        IOrderRepository OrderRepository { get; }
        //IOrderRepository OrderRepository { get; }
        int SaveChangesWithTransaction();
        Task<int> SaveChangesWithTransactionAsync();
    }
}
