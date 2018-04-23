using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp
{
    public class AppSettings
    {
		public string Title { get; set; }

		public string Email { get; set; }
		public string EmailHost { get; set; }
		public int EmailPort { get; set; }
		public string EmailUserName { get; set; }
		public string EmailPassword { get; set; }

		public string Maintain { get; set; }

		public string EmailApiKey { get; set; }
		
	}
}
