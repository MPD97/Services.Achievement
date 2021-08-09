using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Services.Achievement.Application.Exceptions;
using Services.Achievement.Application.Services;
using Services.Achievement.Core.Entities;
using Services.Achievement.Core.Repositories;

namespace Services.Achievement.Application.Events.External.Handlers
{
    public class ScoreIncreasedHandler: IEventHandler<ScoreIncreased>
    {
        private readonly IUserAchievementRepository _userAchievementRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public ScoreIncreasedHandler(IUserAchievementRepository userAchievementRepository, 
            IDateTimeProvider dateTimeProvider, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _userAchievementRepository = userAchievementRepository;
            _dateTimeProvider = dateTimeProvider;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(ScoreIncreased @event)
        {
            var userAchievement = await _userAchievementRepository.GetAsync(@event.UserId);
            if (userAchievement is null)
            {
                userAchievement = new UserAchievement(@event.UserId, null);
                await _userAchievementRepository.AddAsync(userAchievement);
            }

            if (!userAchievement.IsAbleToAddAnyAchievement(@event.TotalScore, @event.AmountAdded))
                return;

            var achievement = userAchievement.CreateAchievement(@event.TotalScore, _dateTimeProvider.Now);
           
            while (achievement is {})
            {
                if (achievement is null)
                    throw new AchievementNullException();

                userAchievement.AddAchievement(achievement, @event.TotalScore);
                await _userAchievementRepository.UpdateAsync(userAchievement);
            
                if (!userAchievement.IsAbleToAddAnyAchievement(@event.TotalScore, @event.AmountAdded))
                    return;
                
                achievement = userAchievement.CreateAchievement(@event.TotalScore, _dateTimeProvider.Now);
            }
            
            var events = _eventMapper.MapAll(userAchievement.Events);
            await _messageBroker.PublishAsync(events);

        }
    }
}