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
            services.AddTransient<CallbackMiddleware>();

            return services;
        }

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

                    await (serviceProvider.GetService<CallbackMiddleware>().InvokeAsync(context, callbackToken));
                });
            });

            return builder;
        }
    }
}
