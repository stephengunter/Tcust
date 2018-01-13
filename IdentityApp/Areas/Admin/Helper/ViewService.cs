using IdentityServer4.Models;
using IdentityServer4;
using IdentityServer4.EntityFramework.Mappers;
using IdentityApp.Areas.Admin.Models;

namespace IdentityApp.Areas.Admin.Helper
{
    public class ViewService
    {
		public ClientViewModel MapClientViewModel(IdentityServer4.EntityFramework.Entities.Client client)
		{
			var model = new ClientViewModel();
			model.id = client.Id;
			
			model.title = client.ClientName;
			model.clientId = client.ClientId;
			model.uri = client.ClientUri;
			return model;
		}
	}
}
