using ReactAdvantage.Domain.ViewModels.Dto;
using ReactAdvantage.Domain.ViewModels.Tasks;
using Threading = System.Threading.Tasks;

namespace ReactAdvantage.API.Services
{
    public interface ITaskService
    {
        Threading.Task<ListResultDto<TaskDto>> GetAll();
        Threading.Task<TaskEditDto> GetById(TaskEditDto editDto);
        Threading.Task<bool> Delete(TaskEditDto editDto);
    }
}
