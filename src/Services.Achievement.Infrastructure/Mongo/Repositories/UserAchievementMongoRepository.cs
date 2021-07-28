using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Services.Achievement.Core.Entities;
using Services.Achievement.Core.Repositories;
using Services.Achievement.Infrastructure.Mongo.Documents;

namespace Services.Achievement.Infrastructure.Mongo.Repositories
{
    public class UserAchievementMongoRepository : IUserAchievementRepository
    {
        private readonly IMongoRepository<UserAchievementDocument, Guid> _repository;

        public UserAchievementMongoRepository(IMongoRepository<UserAchievementDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<UserAchievement> GetAsync(Guid userId)
        {
            var userAchievement = await _repository.GetAsync(r => r.Id == userId);

            return userAchievement?.AsEntity();
        }

        public async Task AddAsync(UserAchievement userAchievement)
            => await _repository.AddAsync(userAchievement.AsDocument());

        public async Task UpdateAsync(UserAchievement userAchievement)
            => await _repository.UpdateAsync(userAchievement.AsDocument());
    }
}