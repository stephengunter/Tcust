using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Views
{
	public class BaseOption
	{
		public BaseOption(string value, string text)
		{
			this.value = value;
			this.text = text;
		}
		public string value { get; set; }
		public string text { get; set; }
	}
}
