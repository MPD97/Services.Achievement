namespace Services.Achievement.Core.Exceptions
{
    public class CannotAddAchievementException : DomainException
    {
        public override string Code { get; } = "cannot_add_achievement";

        public CannotAddAchievementException() :
            base($"Achievement cannot be added.")
        {
        }
    }
}