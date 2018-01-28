using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Views
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
