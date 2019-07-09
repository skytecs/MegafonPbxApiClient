using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skytecs.MegafonPbxApiClient
{
    public static class MegafonClientExtensions
    {
        /// <summary>
        /// Добавляет клиент API Облачной АТС Мегафон <see cref="IMegafonApiClient"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddMegafonApi(this IServiceCollection services, Action<MegafonApiOptions> configure)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if(configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var options = new MegafonApiOptions();

            configure(options);

            services.AddSingleton(options);
            services.AddSingleton<IMegafonApiClient, ApiClient>();
           
            return services;
        }

        /// <summary>
        /// Настраивает функции обратного вызова Облачной АТС Мегафон
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddMegafonCallbacks(this IServiceCollection services, Action<MegafonCallbackOptions> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var options = new MegafonCallbackOptions();

            configure(options);
            services.AddSingleton(options);
            services.AddTransient<ICallbackMiddleware, AdHocCallbackMiddleware>();

            return services;
        }

        /// <summary>
        /// Настраивает функции обратного вызова Облачной АТС Мегафон
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddMegafonCallbacks<THandler>(this IServiceCollection services, Action<MegafonCallbackOptions<THandler>> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var options = new MegafonCallbackOptions<THandler>();

            configure(options);
            services.AddSingleton(options);
            services.AddTransient<ICallbackMiddleware, BoundCallbackMiddleware<THandler>>();

            return services;
        }

        /// <summary>
        /// Запускает обработчик обратных вызовов Облачной АТС Мегафон. 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pathMatch">относительный путь к обработчику</param>
        /// <param name="callbackToken">токен из настроек обратных вызовов</param>
        /// <returns></returns>
        public static IApplicationBuilder MapMegafonCallbacks(this IApplicationBuilder builder, PathString pathMatch, string callbackToken)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Map(pathMatch, app =>
            {
                app.Run(async context => {
                    
                    var serviceProvider = context.RequestServices ?? app.ApplicationServices;
                    if (serviceProvider == null)
                    {
                        throw new InvalidOperationException("Service provider is not available.");
                    }

                    await (serviceProvider.GetService<ICallbackMiddleware>().InvokeAsync(context, callbackToken));
                });
            });

            return builder;
        }
    }
}
