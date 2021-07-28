using System;
using System.Threading.Tasks;
using Services.Achievement.Core.Entities;

namespace Services.Achievement.Core.Repositories
{
    public interface IUserAchievementRepository
    {
        Task<UserAchievement> GetAsync(Guid userId);
        Task AddAsync(UserAchievement userAchievement);
        Task UpdateAsync(UserAchievement userAchievement);
    }
}