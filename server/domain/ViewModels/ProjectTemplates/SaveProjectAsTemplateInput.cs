using System.ComponentModel.DataAnnotations;

namespace ReactAdvantage.Domain.ViewModels.ProjectTemplates
{
    public class SaveProjectAsTemplateInput
    {
        public int ProjectId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public ProjectTemplateType Type { get; set; }
    }
}
