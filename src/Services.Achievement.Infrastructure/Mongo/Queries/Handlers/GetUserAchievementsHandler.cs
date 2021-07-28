using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Services.Achievement.Application.DTO;
using Services.Achievement.Application.Queries;
using Services.Achievement.Core.Entities;
using Services.Achievement.Infrastructure.Mongo.Documents;

namespace Services.Achievement.Infrastructure.Mongo.Queries.Handlers
{
    public class GetUserAchievementsHandler: IQueryHandler<GetUserAchievements, UserAchievementDto>
    {
        private readonly IMongoRepository<UserAchievementDocument, Guid> _repository;

        public GetUserAchievementsHandler(IMongoRepository<UserAchievementDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<UserAchievementDto> HandleAsync(GetUserAchievements query)
        {
            var document = await _repository.GetAsync(p => p.Id == query.UserId);

            return document?.AsDto();        
        }
    }
}