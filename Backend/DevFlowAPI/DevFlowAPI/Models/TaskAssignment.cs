namespace DevFlowAPI.Models
{
    public class TaskAssignment
    {
        public int Id { get; set; }

        public int ProjectTaskId { get; set; }
        public ProjectTask ProjectTask { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime AssignedAt { get; set; }

        
        
    }
}
