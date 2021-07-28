using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Services.Achievement.Application.Events.External
{
    [Message("scores")]
    public class ScoreIncreased : IEvent
    {
        public Guid UserId { get; }
        public int AmountAdded { get; }
        public int TotalScore { get; }
        public string Message { get; }

        public ScoreIncreased(Guid userId, int amountAdded, int totalScore, string message)
        {
            UserId = userId;
            AmountAdded = amountAdded;
            TotalScore = totalScore;
            Message = message;
        }
    }
}