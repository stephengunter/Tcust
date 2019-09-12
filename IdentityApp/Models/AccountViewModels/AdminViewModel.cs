using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.Models.AccountViewModels
{
    public class AdminViewModel
    {
        [Required(ErrorMessage ="請輸入UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "請輸入Key")]
        public string Key { get; set; }

        [Required(ErrorMessage = "請輸入Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
