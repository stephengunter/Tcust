using System;
using System.Collections.Generic;
using System.Text;
using Tcust.Models;

using System.ComponentModel.DataAnnotations;
using ApplicationCore.Views;

namespace Tcust.Views
{
	public class DepartmentViewModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "請填寫代碼")]
        public string code { get; set; }

        [Required(ErrorMessage = "請填寫名稱")]
        public string name { get; set; }

        public bool active { get; set; }

        public int parent { get; set; }


        public Department MapToEntity(Department entity = null)
		{
			if (entity == null) entity = new Department();

			entity.Code = code;
			entity.Active = active;
			entity.Parent = parent;
			entity.Name = name;

			return entity;
		}
	}


}
