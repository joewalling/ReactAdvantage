using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ReactAdvantage.Domain.ViewModels.Dto;

namespace ReactAdvantage.Domain.ViewModels.Projects
{
    public class ProjectEditDto : DtoBase, IValidatableObject
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == 4)
                yield return new ValidationResult("4 is not an acceptable answer", new []{"Id"});
        }
    }
}
