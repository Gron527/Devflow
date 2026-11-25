namespace DevFlowAPI.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime Deadline { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;

        public ICollection<ProjectMembership> Memberships { get; set; }
            = new List<ProjectMembership>();

        public ICollection<Technology> Technologies { get; set; }
            = new List<Technology>();

        public ICollection<ProjectTask> Tasks { get; set; }  = new List<ProjectTask>();
    }
}
