using IdentityServer4.Models;

namespace IdentityApp.Areas.Admin.Helper
{
    public static class Extensions
	{
		public static string Hash256(this string input)
		{
			return input.Sha256();
		}
	}
}
