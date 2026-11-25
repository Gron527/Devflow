namespace DevFlowAPI.Models
{
    public class Technology
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<UserTechnologyExperience> UserTechnologyExperiences { get; set; }  = new List<UserTechnologyExperience>();

        public ICollection<Project> Projects { get; set; }  = new List<Project>();
    }
}
