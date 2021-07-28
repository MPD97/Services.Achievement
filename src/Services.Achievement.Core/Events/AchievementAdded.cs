using Services.Achievement.Core.Entities;

namespace Services.Achievement.Core.Events
{
    public class AchievementAdded :IDomainEvent
    {
        public UserAchievement UserAchievement { get; }
        public Entities.Achievement Achievement { get; }
        public AchievementAdded(UserAchievement userAchievement, Entities.Achievement achievement)
        {
            UserAchievement = userAchievement;
            Achievement = achievement;
        }
    }
}