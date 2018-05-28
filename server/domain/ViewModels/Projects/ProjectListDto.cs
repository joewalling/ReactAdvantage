using System.Collections.Generic;

namespace ReactAdvantage.Domain.ViewModels.Projects
{
    public class ProjectListDto
    {
        public ProjectListDto()
        {
            Projects = new HashSet<ProjectDto>();
        }

        public virtual ICollection<ProjectDto> Projects { get; set; }

        public int RecordCount { get; set; }

    }
}
