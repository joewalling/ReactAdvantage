using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReactAdvantage.Domain.Entities;

namespace ReactAdvantage.API.Services
{
    public interface IProjectTemplateRepository : IDisposable
    {
        Task<List<ProjectTemplate>> GetAllAsync(CancellationToken ct = default(CancellationToken));
        Task<ProjectTemplate> GetByIdAsync(int id, CancellationToken ct = default(CancellationToken));
        Task<List<ProjectTemplate>> GetByArtistIdAsync(int id, CancellationToken ct = default(CancellationToken));
        Task<ProjectTemplate> AddAsync(ProjectTemplate newProjectTemplate, CancellationToken ct = default(CancellationToken));
        Task<bool> UpdateAsync(ProjectTemplate projectTemplate, CancellationToken ct = default(CancellationToken));
        Task<bool> DeleteAsync(int id, CancellationToken ct = default(CancellationToken));
    }
}
