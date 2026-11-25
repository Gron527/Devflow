namespace DevFlowAPI.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<Project> Projects { get; set; } = new List<Project>();

        public ICollection<CompanyMembership> Memberships { get; set; } = new List<CompanyMembership>();
    }
}
