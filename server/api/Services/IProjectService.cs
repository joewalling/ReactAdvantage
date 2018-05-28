using ReactAdvantage.Domain.ViewModels.Projects;
using Threading = System.Threading.Tasks;

namespace ReactAdvantage.API.Services
{
    public interface IProjectService
    {
        Threading.Task<ProjectListDto> GetAllAsync();
        Threading.Task<ProjectDto> GetByIdAsync(int id);
        Threading.Task<ProjectEditDto> UpdateAsync(ProjectEditDto editDto);
        Threading.Task<ProjectEditDto> MergeAsync(ProjectEditDto editDto);
        Threading.Task<int?> CreateAsync(ProjectEditDto editDto);
        Threading.Task<bool> DeleteAsync(int id);


    }
}
