using System;
using System.Collections.Generic;

namespace Services.Achievement.Application.DTO
{
    public class UserAchievementDto
    {
        public Guid Id { get; set; }
        public IEnumerable<AchievementDto> Achievements { get; set; }
    }
}