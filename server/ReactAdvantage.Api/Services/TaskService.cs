using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactAdvantage.Api.Services
{
    public class TaskService :ITaskService
    {
        private IList<Domain.Models.Task> _tasks;

        public TaskService()
        {
            _tasks = new List<Domain.Models.Task>
            {
                new Domain.Models.Task
                {
                    Id = 1,
                    Name = "Task 1 name",
                    Description = "Description of Task 1",
                    DueDate = DateTime.Parse("2018-01-01"),
                    ProjectId = 1
                },
                new Domain.Models.Task
                {
                    Id = 2,
                    Name = "Task 2 name",
                    Description = "Description of Task 2",
                    DueDate = DateTime.Parse("2018-01-02"),
                    ProjectId = 1
                },
                new Domain.Models.Task
                {
                    Id = 3,
                    Name = "Task 3 name",
                    Description = "Description of Task 3",
                    DueDate = DateTime.Parse("2018-01-03"),
                    ProjectId = 1
                },
                new Domain.Models.Task
                {
                    Id = 4,
                    Name = "Task 4 name",
                    Description = "Description of Task 4",
                    DueDate = DateTime.Parse("2018-01-04"),
                    ProjectId = 2
                }
            };
        }


        public Domain.Models.Task GetTaskById(int id)
        {
            return GetTaskByIdAsync(id).Result;
        }

        public async Task<Domain.Models.Task> GetTaskByIdAsync(int id)
        {
            return await Task.FromResult(_tasks.Single(o => Equals(o.Id, id)));
        }

        public async Task<IEnumerable<Domain.Models.Task>> GetTasksAsync()
        {
            return await Task.FromResult(_tasks.AsEnumerable());
        }
    }

    public interface ITaskService
    {
        Domain.Models.Task GetTaskById(int id);
        Task<Domain.Models.Task> GetTaskByIdAsync(int id);
        Task<IEnumerable<Domain.Models.Task>> GetTasksAsync();
    }
}
