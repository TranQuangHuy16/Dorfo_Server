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
        private IMerchantOpeningDayRepository _merchantOpeningDayRepository;
        private IMenuCategoryRepository _menuCategoryRepository;
        private IMenuItemRepository _menuItemRepository;
        private IShipperRepository _shipperRepository;
        private IAddressRepository _addressRepository;
        private IPaymentRepository _paymentRepository;
        private IMerchantCategoryRepository _merchantCategoryRepository;
        private IShopReplyRepository _shopReplyRepository;
        private IReviewRepository _reviewRepository;
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

        public IMerchantOpeningDayRepository MerchantOpeningDayRepository
        {
            get { return _merchantOpeningDayRepository ??= new MerchantOpeningDayRepository(_context); }
        }

        public IMenuCategoryRepository MenuCategoryRepository
        {
            get { return _menuCategoryRepository ??= new MenuCategoryRepository(_context); }
        }

        public IMenuItemRepository MenuItemRepository
        {
            get { return _menuItemRepository ??= new MenuItemRepository(_context); }
        }

        public IMenuItemOptionRepository MenuItemOptionRepository
        {
            get { return _menuItemOptionRepository ??= new MenuItemOptionRepository(_context); }
        }

        public IMenuItemOptionValueRepository MenuItemOptionValueRepository
        {
            get { return _menuItemOptionValueRepository ??= new MenuItemOptionValueRepository(_context); }
        }

        public IShipperRepository ShipperRepository
        {
            get { return _shipperRepository ??= new ShipperRepository(_context); }
        }

        public IOrderRepository OrderRepository
        {
            get { return _orderRepository ??= new OrderRepository(_context); }
        }

        public IAddressRepository AddressRepository
        {
            get { return _addressRepository ??= new AddressRepository(_context); }
        }

        public IPaymentRepository PaymentRepository
        {
            get { return _paymentRepository ??= new PaymentRepository(_context); }
        }

        public IMerchantCategoryRepository MerchantCategory
        {
            get { return _merchantCategoryRepository ??= new MerchantCategoryRepository(_context); }
        }

        public IReviewRepository ReviewRepository
        {
            get { return _reviewRepository ??= new ReviewRepository(_context); }
        }

        public IShopReplyRepository ShopReplyRepository
        {
            get { return _shopReplyRepository ??= new ShopReplyRepository(_context); }
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
