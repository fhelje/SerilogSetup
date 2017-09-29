using System;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace AcademicWork.Serilog
{
    public static class SerilogConfigurationExtensions
    {
        public static LoggerConfiguration EnrichStandardAcademicWork(this LoggerConfiguration config, IConfigurationRoot configuration)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            return config
                .Enrich.FromLogContext()
                .Enrich.WithProcessName()
                .Enrich.WithProperty("ApplicationVersion", configuration["version"])
                .Enrich.WithProperty("ApiName", configuration["ApiName"]);
                

        }

        public static LoggerConfiguration WriteToStandardAcademicWork(this LoggerConfiguration config, IConfigurationRoot configuration)
        {
            if (configuration["ASPNETCORE_ENVIRONMENT"].ToLower() != "production")
            {
                config.WriteTo.ColoredConsole();
            }

            var elasticUrl = configuration["SerilogElasticsearchUrl"];
            if (string.IsNullOrEmpty(elasticUrl)) return config;

            var user = configuration["SerilogElasticsearchUser"];
            var password = configuration["SerilogElasticsearchPassword"];

            return config
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUrl))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "serilog-{0:yyyy.MM.dd}",
                    ModifyConnectionSettings = c => c.BasicAuthentication(user, password)
                });
        }
    }
}