using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        
		[Required(ErrorMessage = "請輸入Email")]
		[EmailAddress(ErrorMessage = "Email格式錯誤")]
		public string Email { get; set; }

        [Required(ErrorMessage = "請輸入新密碼")]
		[Display(Name = "新密碼")]
		[StringLength(100, ErrorMessage = "密碼長度最少8位", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
		[Display(Name = "確認新密碼")]
		[Compare("Password", ErrorMessage = "確認密碼與新密碼不相符")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
