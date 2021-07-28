using System;
using Convey.CQRS.Events;

namespace Services.Achievement.Application.Events
{
    [Contract]
    public class AchievementAdded :IEvent
    {
        public Guid UserId { get; }
        public string Message { get; }
        public string Type { get; }
        public DateTime CreatedAt { get; }

        public AchievementAdded(Guid userId, string message, string type, DateTime createdAt)
        {
            UserId = userId;
            Message = message;
            Type = type;
            CreatedAt = createdAt;
        }
    }
}