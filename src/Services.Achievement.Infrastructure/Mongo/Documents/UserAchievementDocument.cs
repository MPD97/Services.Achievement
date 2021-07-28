using System;
using System.Collections.Generic;
using Convey.Types;

namespace Services.Achievement.Infrastructure.Mongo.Documents
{
    public class UserAchievementDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        
        public IEnumerable<AchievementDocument> Achievements { get; set; }
    }
}