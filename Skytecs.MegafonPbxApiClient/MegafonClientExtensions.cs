using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skytecs.MegafonPbxApiClient
{
    public static class MegafonClientExtensions
    {
        public static IServiceCollection AddMegafon(this IServiceCollection services, Action<MegafonApiOptions> configure)
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

        public static IApplicationBuilder UseMegafon(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.UseMiddleware<CallbackMiddleware>();

            return builder;
        }
    }
}
