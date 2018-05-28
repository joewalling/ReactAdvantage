using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReactAdvantage.Domain.Entities;
using ReactAdvantage.Domain.Repositories;

namespace ReactAdvantage.Data.InMemory.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        public async Task<List<Project>> GetAllAsync(CancellationToken ct = default(CancellationToken))
        {
            var projects = new List<Project>
            {
                new Project{Id = 1, Name = "Project 1"},
                new Project{Id = 2, Name = "Project 2"},
                new Project{Id = 3, Name = "Project 3"}
            };

            return projects;
        }

        public async Task<Project> GetByIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<Project> AddAsync(Project newProject, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Project project, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
