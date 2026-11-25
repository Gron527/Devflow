using DevFlowAPI.Models.Enums;

namespace DevFlowAPI.Models
{
    public class UserTechnologyExperience
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public int TechnologyId { get; set; }

        public Technology Technology { get; set; } = null!;

        public int ExperienceMonths { get; set; }

        public InterestLevel InterestLevel { get; set; }




    }
}
