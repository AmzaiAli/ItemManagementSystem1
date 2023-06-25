using ItemManagementSystem1.Repositories;
using ItemManagementSystem1.Repositories.ItemRepository;
using System;
using System.Threading.Tasks;

namespace ItemManagementSystem1.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IItemRepository ItemRepository { get; }
        Task<int> SaveChangesAsync();

        Task RollbackTransactionAsync();

    }
}
