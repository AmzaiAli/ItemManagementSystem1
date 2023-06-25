using ItemManagementSystem1.Data;
using ItemManagementSystem1.Models;
using ItemManagementSystem1.Repositories.ItemRepository;
using Microsoft.EntityFrameworkCore.Storage;

namespace ItemManagementSystem1.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IItemRepository _itemRepository;
        private IDbContextTransaction _transaction;


        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _itemRepository = new ItemRepository.ItemRepository(_context);

        }

        public IItemRepository ItemRepository => _itemRepository;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                }
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
