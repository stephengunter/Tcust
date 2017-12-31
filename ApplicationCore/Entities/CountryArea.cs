using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
	public class CountryArea:BaseEntity
	{

		public int Parent { get; set; }

		public int PartitionId { get; set; }

		public string Name { get; set; }

		public bool Active { get; set; }

		public bool IsPartition { get; set; }

	}
}
