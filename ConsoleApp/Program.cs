using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
		static IConfigurationRoot GetConfiguration()
		{
			

			var builder = new ConfigurationBuilder()
			.SetBasePath(Path.Combine(AppContext.BaseDirectory))
			.AddJsonFile("appsettings.json", optional: true);

			return builder.Build();
		}

		static void Main(string[] args)
        {
			var configuration = GetConfiguration();

			string savePath = configuration["Settings:DbBackupPath"];

			var dbs = new string[] { "IdentityConnection", "TcustConnection", "BlogConnection" };

			foreach (var name in dbs)
			{
				string connectionString = GetConnectionString(configuration, name);
				DatabaseBackup(connectionString, savePath);
			}

		


		}

		

		static string GetConnectionString(IConfigurationRoot configuration, string name)
		{
			return configuration.GetConnectionString(name);
		}

		static void DatabaseBackup(string connectionString, string savePath)
		{
			var conn = new SqlConnection(connectionString);

			string database = conn.Database.ToString();
			string path = savePath;// "E:\\DB_Backups";
			string cmd = "BACKUP DATABASE [" + database + "] TO DISK='" + path + "\\" + database + "-" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + ".bak'";

			using (SqlCommand command = new SqlCommand(cmd, conn))
			{
				if (conn.State != ConnectionState.Open)
				{
					conn.Open();
				}
				command.ExecuteNonQuery();
				conn.Close();
				
			}
		}
	}
}
