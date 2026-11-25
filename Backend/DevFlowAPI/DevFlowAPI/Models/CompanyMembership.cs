namespace DevFlowAPI.Models
{
    public class CompanyMembership
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;

        public DateTime StartedAt { get; set; }

        public DateTime? EndedAt { get; set; }

        

        
    }
}
