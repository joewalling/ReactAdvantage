using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReactAdvantage.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<List<Entities.Task>> GetAllAsync(CancellationToken ct = default(CancellationToken));
        Task<Entities.Task> GetByIdAsync(int id, CancellationToken ct = default(CancellationToken));
        Task<List<Entities.Task>> GetByProjectIdAsync(int id, CancellationToken ct = default(CancellationToken));
        Task<Entities.Task> AddAsync(Entities.Task newTask, CancellationToken ct = default(CancellationToken));
        Task<bool> UpdateAsync(Entities.Task task, CancellationToken ct = default(CancellationToken));
        Task<bool> DeleteAsync(int id, CancellationToken ct = default(CancellationToken));

    }
}
