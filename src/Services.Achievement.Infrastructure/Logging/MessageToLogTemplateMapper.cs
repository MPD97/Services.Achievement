using System;
using System.Collections.Generic;
using Convey.Logging.CQRS;
using Services.Achievement.Application.Events.External;
using Services.Achievement.Application.Exceptions;

namespace Services.Achievement.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(ScoreIncreased),     
                    new HandlerLogTemplate
                    {
                        After = "Checked user: {UserId} ability to achieve achievements.",
                        OnError = new Dictionary<Type, string>
                        {
                            {
                                typeof(AchievementNullException), "Cannot create achievement for this user."
                            }
                        }
                    }
                }
            };
        
        public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var key = message.GetType();
            return MessageTemplates.TryGetValue(key, out var template) ? template : null;
        }
    }
}