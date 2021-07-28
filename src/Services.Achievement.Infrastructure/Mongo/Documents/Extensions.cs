using System.Linq;
using Services.Achievement.Application.DTO;
using Services.Achievement.Core.Entities;

namespace Services.Achievement.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static UserAchievement AsEntity(this UserAchievementDocument document)
            => new UserAchievement(document.Id, document.Achievements.Select(a 
                =>new Core.Entities.Achievement(a.Id, a.Type, a.Message, a.CreatedAt)));
        

        public static UserAchievementDocument AsDocument(this UserAchievement entity)
            => new UserAchievementDocument
            {
                Id = entity.Id,
                Achievements = entity.Achievements.Select(a 
                    => new AchievementDocument
                    {
                        Id = a.Id,
                        Message = a.Message,
                        Type = a.Type,
                        CreatedAt = a.CreatedAt
                    })
            };

        public static UserAchievementDto AsDto(this UserAchievementDocument document)
            => new UserAchievementDto
            {
                Id = document.Id,
                Achievements = document.Achievements.Select(a 
                    => new AchievementDto()
                    {
                        Id = a.Id,
                        Message = a.Message,
                        Type = a.Type,
                        CreatedAt = a.CreatedAt
                    })
               
            };
    }
}