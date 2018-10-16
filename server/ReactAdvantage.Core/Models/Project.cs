using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReactAdvantage.Domain.MultiTenancy;

namespace ReactAdvantage.Domain.Models
{
    public class Project : IMustHaveTenant
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int TenantId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }

        public virtual Tenant Tenant { get; set; }

        public void UpdateValuesFrom(Project other)
        {
            //only update the editable fields
            //TenantId = other.TenantId;
            Name = other.Name;
        }
    }
}
