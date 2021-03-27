using System;
using Microsoft.Extensions.Configuration;
using PgDeploy.Config;

namespace PgDeploy
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = LoadConfiguration();
            PostgresConfig pgconf = config.GetSection("PostgreSQL").Get<PostgresConfig>();
            SqlFolders sqlFolders = config.GetSection("SqlFolders").Get<SqlFolders>();
            string pgConnectionString = BuildConnectionString(pgconf);

            Console.WriteLine("Hello World!");
        }

        private static IConfiguration LoadConfiguration()
        {
            var confBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.Development.json", true)
                .AddEnvironmentVariables();

            return confBuilder.Build();
        }

        private static string BuildConnectionString(PostgresConfig pgConfig)
        {
            return $"User ID={pgConfig.User};Password={pgConfig.Password};Host={pgConfig.Hostname};Port={pgConfig.Port};Database={pgConfig.Database};SSL Mode=Require;Trust Server Certificate=true;";
        }
    }
}
