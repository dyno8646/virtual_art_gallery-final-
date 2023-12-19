using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Virtual_Art_Gallery.com.hexaware.util
{
    public static class DBConnection
    {
        private static IConfiguration iconfiguration;

        static DBConnection()
        {
            getAppSettingsFile();
        }

        private static void getAppSettingsFile()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            iconfiguration = builder.Build();
        }

        public static SqlConnection GetConnection()
        {
            string connectionString = getConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public static void CloseConnection(SqlConnection connection)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        private static string getConnectionString()
        {
            return iconfiguration.GetConnectionString("DefaultConnection");
        }
    }
}
