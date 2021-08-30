using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Convey;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.Docs.Swagger;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.Outbox.Mongo;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using Convey.Persistence.Redis;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Services.Achievement.Application;
using Services.Achievement.Application.Events.External;
using Services.Achievement.Application.Services;
using Services.Achievement.Core.Repositories;
using Services.Achievement.Infrastructure.Contexts;
using Services.Achievement.Infrastructure.Exceptions;
using Services.Achievement.Infrastructure.Logging;
using Services.Achievement.Infrastructure.Mongo.Documents;
using Services.Achievement.Infrastructure.Mongo.Repositories;
using Services.Achievement.Infrastructure.Services;
using IDateTimeProvider = Services.Achievement.Application.Services.IDateTimeProvider;

namespace Services.Achievement.Infrastructure
{
 public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IUserAchievementRepository, UserAchievementMongoRepository>();
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());

            return builder
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddHttpClient()
                .AddConsul()
                .AddFabio()
                .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
                .AddMessageOutbox(o => o.AddMongo())
                .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                .AddMongo()
                .AddRedis()
                .AddJaeger()
                .AddHandlersLogging()
                .AddMongoRepository<UserAchievementDocument, Guid>("userAchievements")
                .AddWebApiSwaggerDocs();
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseErrorHandler()
                .UseSwaggerDocs()
                .UseJaeger()
                .UseConvey()
                .UsePublicContracts<ContractAttribute>()
                .UseRabbitMq()
                .SubscribeEvent<ScoreIncreased>();
            
            return app;         
        }  
        internal static CorrelationContext GetCorrelationContext(this IHttpContextAccessor accessor)
        {
            var headers = accessor.HttpContext?.Request.Headers;
            if (headers != null && headers.TryGetValue("Correlation-Context", out var json))
            {
                return JsonConvert.DeserializeObject<CorrelationContext>(json.FirstOrDefault());
            }

            return null;
        }
        
        internal static IDictionary<string, object> GetHeadersToForward(this IMessageProperties messageProperties)
        {
            const string sagaHeader = "Saga";
            if (messageProperties?.Headers is null || !messageProperties.Headers.TryGetValue(sagaHeader, out var saga))
            {
                return null;
            }

            return saga is null
                ? null
                : new Dictionary<string, object>
                {
                    [sagaHeader] = saga
                };
        }
        
        internal static string GetSpanContext(this IMessageProperties messageProperties, string header)
        {
            if (messageProperties is null)
            {
                return string.Empty;
            }

            if (messageProperties.Headers.TryGetValue(header, out var span) && span is byte[] spanBytes)
            {
                return Encoding.UTF8.GetString(spanBytes);
            }

            return string.Empty;
        }
    }
}