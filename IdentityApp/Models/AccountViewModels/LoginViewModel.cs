using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.Models.AccountViewModels
{
    public class LoginViewModel
    {
		[Required(ErrorMessage = "請輸入使用者名稱")]
		public string UserName { get; set; }



		[Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "在這台電腦記住我")]
        public bool RememberMe { get; set; }
    }
}
