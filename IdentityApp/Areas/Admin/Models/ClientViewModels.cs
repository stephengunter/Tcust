using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace IdentityApp.Areas.Admin.Models
{
    public class ClientViewModel
    {
		public int id { get; set; }

		public string clientId { get; set; }
		public string title { get; set; }
		public string secret { get; set; }

		
		public string uri { get; set; }

		
		public string adminName { get; set; }


		public Client MapToEntity()
		{
			var client = new Client() { ClientSecrets= new List<Secret>() };

			
			
			client.ClientId = clientId;
			client.ClientName = title;
			client.ClientUri = uri;
			

			return client;
		}
	}

	public class ClientEditForm
	{
		public ClientViewModel client { get; set; }
	}
}
