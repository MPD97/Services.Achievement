using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Services.Achievement.Core.Events;
using Services.Achievement.Core.Exceptions;

namespace Services.Achievement.Core.Entities
{
    public class UserAchievement : AggregateRoot
    {
        public const int BronzeScore = 30;
        public const int SilverScore = 100;
        public const int GoldScore = 300;
        public const int MasterScore = 1000;

        private ISet<Achievement> _achievements = new HashSet<Achievement>();

        public IEnumerable<Achievement> Achievements
        {
            get => _achievements;
            private set => _achievements = new HashSet<Achievement>(value);
        }

        public UserAchievement(Guid id, IEnumerable<Achievement> achievements)
        {
            Id = id;
            Achievements = achievements ?? Enumerable.Empty<Achievement>();
        }

        public Achievement CreateAchievement(int score, DateTime createdAt)
        {
            switch (score)
            {
                case > MasterScore:
                    if (!_achievements.Any(a => a.Type == AchievementType.Master))
                        return Achievement.AchievementMaster(Guid.NewGuid(), createdAt);
                    else if (!_achievements.Any(a => a.Type == AchievementType.Gold))
                        return Achievement.AchievementGold(Guid.NewGuid(), createdAt);
                    else if (!_achievements.Any(a => a.Type == AchievementType.Silver))
                        return Achievement.AchievementSilver(Guid.NewGuid(), createdAt);
                    else if (!_achievements.Any(a => a.Type == AchievementType.Bronze))
                        return Achievement.AchievementBronze(Guid.NewGuid(), createdAt);
                    break;

                case > GoldScore:
                    if (!_achievements.Any(a => a.Type == AchievementType.Gold))
                        return Achievement.AchievementGold(Guid.NewGuid(), createdAt);
                    else if (!_achievements.Any(a => a.Type == AchievementType.Silver))
                        return Achievement.AchievementSilver(Guid.NewGuid(), createdAt);
                    else if (!_achievements.Any(a => a.Type == AchievementType.Bronze))
                        return Achievement.AchievementBronze(Guid.NewGuid(), createdAt);
                    break;

                case > SilverScore:
                    if (!_achievements.Any(a => a.Type == AchievementType.Silver))
                        return Achievement.AchievementSilver(Guid.NewGuid(), createdAt);
                    else if (!_achievements.Any(a => a.Type == AchievementType.Bronze))
                        return Achievement.AchievementBronze(Guid.NewGuid(), createdAt);
                    break;

                case > BronzeScore:
                    if (!_achievements.Any(a => a.Type == AchievementType.Bronze))
                        return Achievement.AchievementBronze(Guid.NewGuid(), createdAt);
                    break;
                
                default:
                    throw new NotImplementedException();
            }

            throw new NotImplementedException();
        }

        public bool IsAbleToAddAnyAchievement(int score, int amountAdded)
        {
            var scoreBefore = score - amountAdded;
            switch (scoreBefore)
            {
                case < BronzeScore when score > BronzeScore:
                case < SilverScore when score > SilverScore:
                case < GoldScore when score > GoldScore:
                case < MasterScore when score > MasterScore:
                    return true;
                default:
                    break;
            }

            switch (score)
            {
                case > MasterScore when !_achievements.Any(a => a.Type == AchievementType.Bronze)
                                        || !_achievements.Any(a => a.Type == AchievementType.Silver)
                                        || !_achievements.Any(a => a.Type == AchievementType.Gold)
                                        || !_achievements.Any(a => a.Type == AchievementType.Master):
                case > GoldScore when !_achievements.Any(a => a.Type == AchievementType.Bronze)
                                      || !_achievements.Any(a => a.Type == AchievementType.Silver)
                                      || !_achievements.Any(a => a.Type == AchievementType.Gold):
                case > SilverScore when !_achievements.Any(a => a.Type == AchievementType.Bronze)
                                        || !_achievements.Any(a => a.Type == AchievementType.Silver):
                case > BronzeScore when !_achievements.Any(a => a.Type == AchievementType.Bronze):
                    return true;
                default:
                    return false;
            }
        }

        public bool IsAbleToAddAchievement(Achievement achievement, int newScore)
        {
            switch (achievement.Type)
            {
                case AchievementType.Bronze:
                    if (newScore < BronzeScore)
                        return false;
                    break;
                case AchievementType.Silver:
                    if (newScore < SilverScore)
                        return false;
                    break;
                case AchievementType.Gold:
                    if (newScore < GoldScore)
                        return false;
                    break;
                case AchievementType.Master:
                    if (newScore < MasterScore)
                        return false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(achievement.Type));
            }

            if (_achievements.Any(a => a.Type == achievement.Type))
                return false;
            return true;
        }

        public void AddAchievement(Achievement achievement, int newScore)
        {
            if (!IsAbleToAddAchievement(achievement, newScore))
                throw new CannotAddAchievementException();

            _achievements.Add(achievement);
            AddEvent(new AchievementAdded(this, achievement));
        }
    }
}