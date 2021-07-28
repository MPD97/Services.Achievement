using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Events;
using Services.Achievement.Application.Services;
using Services.Achievement.Core;
using Services.Achievement.Core.Events;

namespace Services.Achievement.Infrastructure.Services
{
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);

        public IEvent Map(IDomainEvent @event)
        {
            switch (@event)
            {
                case AchievementAdded e: return new Application.Events.AchievementAdded(e.UserAchievement.Id, 
                    e.Achievement.Message, e.Achievement.Type.ToString(), e.Achievement.CreatedAt);
            }

            return null;
        }
    }
}