using ReactAdvantage.Domain.ViewModels.Tasks;
using System.Collections.Generic;
using ReactAdvantage.Domain.ViewModels.Dto;

namespace ReactAdvantage.Domain.ViewModels.Projects
{
    public class ProjectDto : DtoBase
    {
        public ProjectDto()
        {
            TaskList = new List<TaskDto>();
        }
        public int Id { get; set; }
        
        public string Name { get; set; }

        public List<TaskDto> TaskList { get; set; }

    }
}
