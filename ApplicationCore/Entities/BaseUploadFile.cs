using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ApplicationCore.Entities
{
	public abstract class BsseUploadFile : BaseRecord
	{

		public string Path { get; set; }

		public string PreviewPath { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public string Type { get; set; }

		public string Name { get; set; }

		public string Title { get; set; }

		public string PS { get; set; }

		
	}
}
