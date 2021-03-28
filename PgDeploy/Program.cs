using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
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

            List<string> scripts = GetOrderedScriptPaths(sqlFolders);

            foreach (string s in scripts)
            {
                ExecuteSqlScript(s, pgConnectionString);
            }
        }

        private static void ExecuteSqlScript(string scriptPath, string connString)
        {
            string scriptText = File.ReadAllText(scriptPath);
            using (var connection = new NpgsqlConnection(connString))
            {
                Console.WriteLine($"{scriptPath}");
                try
                {
                  connection.Execute(scriptText);
                } catch (Exception x)
                {
                    Console.WriteLine(x.Message);
                    throw;
                }
            }
        }

        private static List<string> GetOrderedScriptPaths(SqlFolders sqlFolders)
        {
            List<string> scripts = new List<string>();
            //goes through folders in the order they are in the app settings
            //and adds filepaths sorted alphabetically per folder.
            foreach(string folder in sqlFolders.SqlScripts)
            {
                scripts.AddRange(Directory.GetFiles(Path.Combine(sqlFolders.SqlRoot, folder), "*.sql").ToList().OrderBy(name => name, StringComparer.InvariantCulture));
            }

            return scripts;
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
