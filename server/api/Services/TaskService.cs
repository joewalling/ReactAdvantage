using System;
using System.Threading.Tasks;
using ReactAdvantage.Domain.ViewModels.Dto;
using ReactAdvantage.Domain.ViewModels.Tasks;

namespace ReactAdvantage.API.Services
{
    public class TaskService : ITaskService
    {
        public async Task<ListResultDto<TaskDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<TaskEditDto> GetById(TaskEditDto editDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(TaskEditDto editDto)
        {
            throw new NotImplementedException();
        }
    }
}
