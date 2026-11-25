namespace DevFlowAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateOnly BirthDate { get; set; }

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<CompanyMembership> CompanyMemberships { get; set; } = new List<CompanyMembership>();

        public ICollection<ProjectMembership> ProjectMemberShips { get; set; } = new List<ProjectMembership>();

        public ICollection<UserTechnologyExperience> TechnologyExperiences { get; set; }  = new List<UserTechnologyExperience>();

        public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();

        public ICollection<Bug> ReportedBugs { get; set; } = new List<Bug>();

        public ICollection<Bug> AssignedBugs { get; set; } = new List<Bug>();
    }
}
