using System;
using Services.Achievement.Core.Entities;

namespace Services.Achievement.Infrastructure.Mongo.Documents
{
    public class AchievementDocument
    {
        public Guid Id { get; set; } 
        public AchievementType Type { get; set; } 
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}