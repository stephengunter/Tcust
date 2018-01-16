using IdentityApp.Data;
using IdentityApp.Models;
using IdentityApp.Specifications;
using System.Threading.Tasks;

namespace IdentityApp.Services
{
	public interface IUserService
	{
		ApplicationUser GetUserByEmail(string email);

	}


	public class UserService: IUserService
	{
		private readonly IIdentityRepository<ApplicationUser> userRepository;

		public UserService(IIdentityRepository<ApplicationUser> userRepository)
		{
			this.userRepository = userRepository;
		}

		public ApplicationUser GetUserByEmail(string email)
		{
			
			var spec = new UserFilterSpecifications(email);
			var user = userRepository.GetSingleBySpec(spec);

			
			return user;
			
		}
	}
}
