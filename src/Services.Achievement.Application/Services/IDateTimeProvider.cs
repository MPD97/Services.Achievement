using System;

namespace Services.Achievement.Application.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}