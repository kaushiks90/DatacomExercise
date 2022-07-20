using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;


namespace DatacomConsole.Utils
{
    public static class ConfigExtension
    {
        public static void AddConfiguration<T>(this IServiceCollection services, IConfiguration configuration, string configurationNode) where T : class
        {
            if (string.IsNullOrEmpty(configurationNode))
            {
                configurationNode = typeof(T).Name;
            }
            var instance = Activator.CreateInstance<T>();
            new ConfigureFromConfigurationOptions<T>(configuration.GetSection(configurationNode)).Configure(instance);
            services.AddSingleton(instance);
        }
    }
}
