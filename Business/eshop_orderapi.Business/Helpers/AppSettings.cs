using Microsoft.Extensions.Configuration;
using System;

namespace eshop_orderapi.Business.Helpers
{
    public class AppSettings
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ApiUrl => _configuration.GetSection("Api")["ApiUrl"];
        public bool EnableHangfire => Convert.ToBoolean(_configuration.GetSection("Settings")["EnableHangfire"]);
        public string FrontEndUrl => _configuration.GetSection("Urls")["FrontEnd"];
    }

    public class MailSettings
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string MailFrom => _configuration.GetSection("MailSettings")["From"];
        public string MailHost => _configuration.GetSection("MailSettings")["Host"];
        public string MailPort => _configuration.GetSection("MailSettings")["Port"];
        public string MailPassword => _configuration.GetSection("MailSettings")["Password"];
        public bool EnableMail => Convert.ToBoolean(_configuration.GetSection("MailSettings")["EnableMail"]);
    }

    public class Jwt
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Key => _configuration.GetSection("Jwt")["Key"];
        public string Issuer => _configuration.GetSection("Jwt")["Issuer"];
    }
}