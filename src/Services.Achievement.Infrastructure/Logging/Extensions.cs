using Convey;
using Convey.Logging.CQRS;
using Microsoft.Extensions.DependencyInjection;
using Services.Achievement.Application.Events.External;

namespace Services.Achievement.Infrastructure.Logging
{
    internal static class Extensions
    {
        public static IConveyBuilder AddHandlersLogging(this IConveyBuilder builder)
        {
            var assembly = typeof(ScoreIncreased).Assembly;
            
            builder.Services.AddSingleton<IMessageToLogTemplateMapper>(new MessageToLogTemplateMapper());
            
            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }
}