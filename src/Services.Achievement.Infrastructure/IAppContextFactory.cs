using Services.Achievement.Application;

namespace Services.Achievement.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}