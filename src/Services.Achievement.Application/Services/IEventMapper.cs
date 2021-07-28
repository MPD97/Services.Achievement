using System.Collections.Generic;
using Convey.CQRS.Events;
using Services.Achievement.Core;

namespace Services.Achievement.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }
}