namespace DevFlowAPI.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? Deadline { get; set; }

        public TaskStatus Status { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; } = null!;

        public ICollection<TaskAssignment> Assignments { get; set; } = new List<TaskAssignment>();
        public ICollection<Bug> Bugs { get; set; } = new List<Bug>();
    }
}
