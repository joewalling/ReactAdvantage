using System.Collections.Generic;

namespace ReactAdvantage.Domain.ViewModels.Tasks
{
    public class ExportProjectAsTemplateInput
    {
        public List<TaskEditDto> Items { get; set; }
        public string ProjectName { get; set; }
    }
}
