using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReactAdvantage.Domain.Repositories;
using TaskEntity = ReactAdvantage.Domain.Entities.Task;

namespace ReactAdvantage.Data.InMemory.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        public async Task<List<TaskEntity>> GetAllAsync(CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<TaskEntity> GetByIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<List<TaskEntity>> GetByProjectIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<TaskEntity> AddAsync(TaskEntity newTask, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(TaskEntity task, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }


    }
}
