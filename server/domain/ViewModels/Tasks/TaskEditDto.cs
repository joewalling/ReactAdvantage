using System;

namespace ReactAdvantage.Domain.ViewModels.Tasks
{
    public class TaskEditDto
    {
        public int? Id { get; set; }

        public int ProjectId { get; set; }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public bool Completed { get; set; }

        public DateTime? CompletionDate { get; set; }
    }
}
