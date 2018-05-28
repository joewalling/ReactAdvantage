using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReactAdvantage.Domain.Entities;

namespace ReactAdvantage.Domain.Repositories
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllAsync(CancellationToken ct = default(CancellationToken));
        Task<Project> GetByIdAsync(int id, CancellationToken ct = default(CancellationToken));
        Task<Project> AddAsync(Project newProject, CancellationToken ct = default(CancellationToken));
        Task<bool> UpdateAsync(Project project, CancellationToken ct = default(CancellationToken));
        Task<bool> DeleteAsync(int id, CancellationToken ct = default(CancellationToken));

    }
}
