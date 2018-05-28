using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReactAdvantage.Domain.Entities;

namespace ReactAdvantage.API.Services
{
    public interface IProjectTemplateTaskRepository : IDisposable
    {
        Task<List<ProjectTemplateTask>> GetAllAsync(CancellationToken ct = default(CancellationToken));
        Task<ProjectTemplateTask> GetByIdAsync(int id, CancellationToken ct = default(CancellationToken));
        Task<List<ProjectTemplateTask>> GetByArtistIdAsync(int id, CancellationToken ct = default(CancellationToken));
        Task<ProjectTemplate> AddAsync(ProjectTemplateTask newProjectTemplateTask, CancellationToken ct = default(CancellationToken));
        Task<bool> UpdateAsync(ProjectTemplateTask projectTemplateTask, CancellationToken ct = default(CancellationToken));
        Task<bool> DeleteAsync(int id, CancellationToken ct = default(CancellationToken));
    }
}
