using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IList<Project> _projects;

        public ProjectService()
        {
            _projects = new List<Project>
            {
                new Project
                {
                    Id = 1,
                    Name = "Project 1 name",
                },
                new Project
                {
                    Id = 2,
                    Name = "Project 2 name",
                },
                new Project
                {
                    Id = 3,
                    Name = "Project 3 name",
                },
                new Project
                {
                    Id = 4,
                    Name = "Project 4 name",
                }
            };
        }


        public Project GetProjectById(int id)
        {
            return GetProjectByIdAsync(id).Result;
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await System.Threading.Tasks.Task.FromResult(_projects.Single(o => Equals(o.Id, id)));
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await System.Threading.Tasks.Task.FromResult(_projects.AsEnumerable());
        }
    }

    public interface IProjectService
    {
        Project GetProjectById(int id);
        Task<Project> GetProjectByIdAsync(int id);
        Task<IEnumerable<Project>> GetProjectsAsync();
    }
}
