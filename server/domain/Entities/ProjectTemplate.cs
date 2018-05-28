using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ReactAdvantage.Domain.Entities.Authorization.Users;

namespace ReactAdvantage.Domain.Entities
{
    public class ProjectTemplate 
    {
        public ProjectTemplate()
        {
            ProjectTemplateTasks = new HashSet<ProjectTemplateTask>();
        }

        public virtual int? TenantId { get; set; }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ProjectTemplateType Type { get; set; }

        public string Description { get; set; }

        public virtual User CreatorUser { get; set; }

        public int CreatorUserId { get; set; }

        public virtual ICollection<ProjectTemplateTask> ProjectTemplateTasks { get; set; }

    }

    public enum ProjectTemplateType
    {
        Basic = 1,
        Premium = 2,
        Personal = 3,
        Shared = 4
    }
}
