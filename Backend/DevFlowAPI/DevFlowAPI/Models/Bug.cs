using DevFlowAPI.Models.Enums;

namespace DevFlowAPI.Models
{
    public class Bug
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ExpectedBehavior { get; set; } = string.Empty;

        public BugPriority Priority { get; set; }

        public BugSeverity Severity { get; set; }

        public DateTime ReportedAt { get; set; }

        public DateTime? SolvedAt { get; set; }

        public int ProjectTaskId { get; set; }

        public ProjectTask ProjectTask { get; set; } = null!;

        public int ReporterUserId { get; set; }

        public User Reporter { get; set; } = null!;

        public int? AssignedUserId { get; set; }

        public User? AssignedUser { get; set; }

        public BugStatus Status { get; set; }
        

        
    }
}
