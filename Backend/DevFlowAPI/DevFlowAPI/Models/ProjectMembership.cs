using DevFlowAPI.Models.Enums;

namespace DevFlowAPI.Models
{
    public class ProjectMembership
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; } = null!;

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public ProjectRole Role { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime? EndedAt { get; set; }

        

        
    }
}
