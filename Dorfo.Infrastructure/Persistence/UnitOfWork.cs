using Dorfo.Application.Interfaces;
using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DorfoDbContext _context;
        private IUserRepository _userRepository;
        private IMerchantRepository _merchantRepository;
        //private IOrderRepository _orderRepository;
        private IMenuItemOptionRepository _menuItemOptionRepository;
        private IMenuItemOptionValueRepository _menuItemOptionValueRepository;
        private IOrderRepository _orderRepository;
        public UnitOfWork()
        {
        }

        public UnitOfWork(DorfoDbContext context)
        {
            _context = context;
        }
        public IUserRepository UserRepository
        {
            get { return _userRepository ??= new UserRepository(_context); }
        }

        public IMerchantRepository MerchantRepository
        {
            get { return _merchantRepository ??= new MerchantRepository(_context); }
        }

        public IMenuItemOptionRepository MenuItemOptionRepository
        {
            get { return _menuItemOptionRepository ??= new MenuItemOptionRepository(_context); }
        }

        public IMenuItemOptionValueRepository MenuItemOptionValueRepository
        {
            get { return _menuItemOptionValueRepository ??= new MenuItemOptionValueRepository(_context); }
        }

        public IOrderRepository OrderRepository
        {
            get { return _orderRepository ??= new OrderRepository(_context); }
        }
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public int SaveChangesWithTransaction()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }
    }
}
