using System.Collections.Generic;

namespace Services.Achievement.Core.Entities
{
    public interface IAggregateRoot
    {
        IEnumerable<IDomainEvent> Events { get; }
        AggregateId Id { get;  }
    }
}