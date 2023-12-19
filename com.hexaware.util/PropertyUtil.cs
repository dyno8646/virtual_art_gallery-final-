using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Virtual_Art_Gallery.com.hexaware.util
{
    public static class PropertyUtil
    {
        private static IConfiguration _configuration;

        static PropertyUtil()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public static string GetConnectionString()
        {
            try
            {
                return _configuration.GetConnectionString("DefaultConnection");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading database connection string: " + ex.Message);
                return null;
            }
        }
    }
}
