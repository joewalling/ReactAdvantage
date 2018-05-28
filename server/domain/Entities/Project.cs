using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactAdvantage.Domain.Entities
{
    public class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
