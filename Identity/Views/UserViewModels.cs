using ApplicationCore.Views;
using IdentityApp.Helper;
using IdentityApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IdentityApp.Views
{

	public enum RoleType
	{
		Dev,
		Teacher,
		Staff,
		Student
	}

	public class IdentityUserViewModel
	{
		public string id { get; set; }
		public string name { get; set; }
		
		[Required(ErrorMessage ="請填寫Email")]
		[EmailAddress(ErrorMessage = "Email格式錯誤")]
		public string email { get; set; }

		public string phone { get; set; }

		public string roles { get; set; }

		public ProfileViewModel profile { get; set; }


		public ApplicationUser MapToEntity(ApplicationUser entity)
		{
			entity.UserName = email.GetUserNameFromEmail();
			entity.Email = email;
			entity.PhoneNumber = phone;

			return entity;
		}
	}

	public class ProfileViewModel
	{
		[Required(ErrorMessage = "請填寫姓名")]
		public string fullname { get; set; }

		public string sid { get; set; }
		public DateTime dob { get; set; }
		public bool gender { get; set; }

		public Profile MapToEntity(Profile entity)
		{

			entity.Fullname = fullname;
			entity.SID = sid;
			entity.DOB = dob;
			entity.Gender = gender;

			return entity;
		}
	}

	public class IdentityUserEditViewModel
	{
		public List<BaseOption>  roleOptions { get; set; }
		public string[] roles { get; set; }
		public IdentityUserViewModel user { get; set; }
	}
}
