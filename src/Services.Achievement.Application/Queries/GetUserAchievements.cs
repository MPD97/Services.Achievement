using System;
using Convey.CQRS.Queries;
using Services.Achievement.Application.DTO;

namespace Services.Achievement.Application.Queries
{
    public class GetUserAchievements : IQuery<UserAchievementDto>
    {
        public Guid UserId { get; set; }
    }
}