using System;
using System.Collections.Generic;
using System.Text;
using Blog.Models;

namespace Blog.Helpers
{
	public static class Extentions
    {
		public static bool IsDiary(this Category category)
		{
			return category.Code == "diary";
		}
	}
}
