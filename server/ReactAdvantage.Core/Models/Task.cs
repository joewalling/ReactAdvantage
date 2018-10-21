using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReactAdvantage.Domain.MultiTenancy;

namespace ReactAdvantage.Domain.Models
{
    public class Task : IMustHaveTenant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public bool Completed { get; set; }

        public DateTime? CompletionDate { get; set; }

        public virtual Tenant Tenant { get; set; }

        public void UpdateValuesFrom(Task other)
        {
            //only update the editable fields
            //TenantId = other.TenantId;
            ProjectId = other.ProjectId;
            Name = other.Name;
            Description = other.Description;
            DueDate = other.DueDate;
            Completed = other.Completed;
            CompletionDate = other.CompletionDate;
        }

    }
}
