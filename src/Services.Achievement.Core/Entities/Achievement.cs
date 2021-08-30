using System;

namespace Services.Achievement.Core.Entities
{
    public class Achievement
    {
        public AggregateId Id { get; private set; } 
        public AchievementType Type { get; private set; } 
        public string Message { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Achievement(AggregateId id, AchievementType type, string message, DateTime createdAt)
        {
            Id = id;
            Type = type;
            Message = message;
            CreatedAt = createdAt;
        }

        public static Achievement AchievementBronze(Guid id, DateTime createdAt)
            => new (id, AchievementType.Bronze, "Bronze energy medal", createdAt);
        
        public static Achievement AchievementSilver(Guid id, DateTime createdAt)
            => new (id, AchievementType.Silver, "Silver energy medal", createdAt);
        
        public static Achievement AchievementGold(Guid id, DateTime createdAt)
            => new (id, AchievementType.Gold, "Gold energy medal", createdAt);
        
        public static Achievement AchievementMaster(Guid id, DateTime createdAt)
            => new (id, AchievementType.Master, "Master of energy", createdAt);
    }
}