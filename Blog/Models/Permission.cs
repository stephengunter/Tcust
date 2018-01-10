using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;

namespace Blog.Models
{
    public class Permission : BasePermission
    {

    }

	public class UserPermission : BaseUserPermission
	{
		public int PermissionId { get; set; }
		public Permission Permission { get; set; }
	}
}
