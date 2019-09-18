using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using Permissions.Services;


namespace TcustApp.Authorization
{
	public class HasPermissionHandler : AuthorizationHandler<HasPermissionRequirement>
	{
		IPermissionService permissionService;
		public HasPermissionHandler(IPermissionService permissionService)
		{
			this.permissionService = permissionService;
		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement )
		{
			
			bool isDev = context.CurrentUserIsDev();
			if (isDev)
			{
				context.Succeed(requirement);
				return Task.CompletedTask;
			}

			string userId = context.CurrentUserId();
			string permissionName = requirement.PermissionName;

			bool hasPermission = permissionService.IsUserHasPermission(userId, permissionName);

			if (hasPermission) context.Succeed(requirement);
			else context.Fail();


			return Task.CompletedTask;
		}
	}
}
