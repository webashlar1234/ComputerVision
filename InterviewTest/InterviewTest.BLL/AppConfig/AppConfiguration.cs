using Microsoft.Extensions.Configuration;
using System;

namespace InterviewTest.BLL.AppConfig
{
    public static class AppConfiguration
    {
        private static IConfiguration currentConfig;

        public static void SetConfig(IConfiguration configuration)
        {
            currentConfig = configuration;
        }


        public static string GetConfiguration(string configKey)
        {
            try
            {
                return new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build().GetConnectionString(configKey).ToString();                
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return "";
        }

    }
}
