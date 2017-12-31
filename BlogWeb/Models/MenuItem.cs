using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWeb.Models
{
	public class MenuItem
	{
		public MenuItem() { }
		public MenuItem(string text, int count)
		{
			this.Text = text;
			this.Count = count;
		}
		public string Text { get; set; }
		public int Count { get; set; }
	}
}
