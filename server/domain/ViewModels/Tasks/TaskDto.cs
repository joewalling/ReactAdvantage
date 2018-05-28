using System;

namespace ReactAdvantage.Domain.ViewModels.Tasks
{
    public class TaskDto
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public bool Completed { get; set; }

        public DateTime? CompletionDate { get; set; }

    }
}
