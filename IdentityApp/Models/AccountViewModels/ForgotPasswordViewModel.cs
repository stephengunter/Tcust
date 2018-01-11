using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage ="請輸入Email")]
        [EmailAddress(ErrorMessage = "Email格式錯誤")]
        public string Email { get; set; }
    }
}
