using System;
using ReactAdvantage.Domain.Entities.Authorization.Users;

namespace ReactAdvantage.Domain.Entities
{
    public class ProjectTemplateTask 
    {
        public int? TenantId { get; set; }

        public int ProjectTemplateId { get; set; }
        public int Id { get; set; }


        public virtual ProjectTemplate ProjectTemplate { get; set; }

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long? AssignedToUserId { get; set; }

        public DateTime? DueDate { get; set; }

        public bool Completed { get; set; }

        public int Sequence { get; set; }

        public DateTime? CompletionDate { get; set; }

        public bool Heading { get; set; }

        public virtual User AssignedToUser { get; set; }

        public virtual User CreatorUser { get; set; }
    }
}
