using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;


namespace WebApi.Controllers
{
    public class DBController : BaseApiController
	{
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IOptions<AppSettings> settings;

		
		IConfigurationRoot Configuration { get; set; }
		

		public DBController(IHostingEnvironment environment, IOptions<AppSettings> settings)
		{
			this._hostingEnvironment = environment;
			this.settings = settings;


			var builder = new ConfigurationBuilder()
			   .SetBasePath(_hostingEnvironment.ContentRootPath)
			   .AddJsonFile("appsettings.json", optional: true)
			   .AddEnvironmentVariables();


			Configuration = builder.Build();

		}

		public IActionResult Backup(string key)
        {
			
			if (key != settings.Value.Key)
			{
				return Content("Wrong Key");
			}
			var dbs = new string[] { "IdentityConnection", "TcustConnection", "BlogConnection" };
			foreach (var name in dbs)
			{
				
				DatabaseBackup(Configuration.GetConnectionString(name));
			}

			return Content("DB Backup Has Done");
        }

		


		void DatabaseBackup(string connectionString)
		{
			string savePath = settings.Value.DbRestoreFolder;
			var conn = new SqlConnection(connectionString);

			string database = conn.Database.ToString();
			string path = savePath;  // "E:\\DB_Backups";
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