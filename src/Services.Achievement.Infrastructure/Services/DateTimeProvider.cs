using System;
using Services.Achievement.Application.Services;

namespace Services.Achievement.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }
}