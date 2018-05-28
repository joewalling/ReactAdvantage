using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReactAdvantage.Data.EntityFramework;
using ReactAdvantage.Domain.Entities;
using ReactAdvantage.Domain.ViewModels.Projects;
using ReactAdvantage.Domain.ViewModels.Tasks;
using AppTask = ReactAdvantage.Domain.Entities.Task;
using Threading = System.Threading.Tasks;

namespace ReactAdvantage.API.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ReactAdvantageContext _dbContext;

        public ProjectService(ReactAdvantageContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Threading.Task<ProjectListDto> GetAllAsync()
        {
            var projectList = new ProjectListDto();

            var projects = await _dbContext.Projects.ToListAsync();
            //    .Select(x => new ProjectDto
            //    {
            //        Id = x.Id,
            //        Name = x.Name
            //    })
            //    .OrderBy(x => x.Name).ToListAsync();

            foreach (Project item in projects)
            {
                ProjectDto newProjectDto = new ProjectDto
                {
                    Id = item.Id,
                    Name = item.Name
                };
                projectList.Projects.Add(newProjectDto);

            }

            projectList.RecordCount = projects.Count;

            return projectList;
        }

        public async Threading.Task<ProjectDto> GetByIdAsync(int id)
        {
            var project = await _dbContext.Projects.FindAsync(id);

            if (project == null)
            {
                return new ProjectDto()
                {
                    NoData = true
                };
            }

            var projectDto = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name
            };

            var tasks = await _dbContext.Tasks.Where(t => t.ProjectId == id).ToListAsync();

            foreach(AppTask item in tasks)
            {
                TaskDto newTaskDto = new TaskDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    ProjectId = item.ProjectId,
                    Description = item.Description,
                    DueDate = item.DueDate,
                    Completed = item.Completed,
                    CompletionDate = item.CompletionDate

                };

                projectDto.TaskList.Add(newTaskDto);
            }

            return projectDto;
        }


        public async Threading.Task<bool> DeleteAsync(int id)
        {
            var project = await _dbContext.Projects.FindAsync(id);
            if (project == null)
            {
                return false;
            }

            _dbContext.Projects.Remove(project);

            await _dbContext.SaveChangesAsync();
           
            return true;
        }

        public async Threading.Task<ProjectEditDto> UpdateAsync(ProjectEditDto editDto)
        {
            if (editDto?.Id == null)
            {
                editDto = new ProjectEditDto {NoData = true};
                return editDto;
            }

            var projectEntity = await GetByIdAsync(editDto.Id.Value);
            projectEntity.Name = editDto.Name;
            await _dbContext.SaveChangesAsync();

            return editDto;
        }

        public async Threading.Task<ProjectEditDto> MergeAsync(ProjectEditDto editDto)
        {
            if (editDto?.Id == null)
            {
                editDto = new ProjectEditDto { NoData = true };
                return editDto;
            }

            var projectEntity = await GetByIdAsync(editDto.Id.Value);
            projectEntity.Name = editDto.Name;
            await _dbContext.SaveChangesAsync();

            return editDto;
        }

        public async Threading.Task<int?> CreateAsync(ProjectEditDto editDto)
        {
            if (editDto.Name != null)
            {
                var project = new Project
                {
                    Name = editDto.Name
                };
                await _dbContext.Projects.AddAsync(project);
                await _dbContext.SaveChangesAsync();

                return project.Id;
            }

            return null;

        }
    }
}
