namespace Services.Achievement.Application.Exceptions
{
    public class AchievementNullException : AppException
    {
        public override string Code { get; } = "achievement_null";
        public AchievementNullException() 
            : base($"Achievement was null.")
        {
        
        }
    }
}