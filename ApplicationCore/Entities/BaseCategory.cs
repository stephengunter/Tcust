

namespace ApplicationCore.Entities
{
	public abstract class BaseCategory: BaseEntity
	{
		public string Code { get; set; }

		public string Name { get; set; }

		public int Order { get; set; }

		public bool Active { get; set; }

		
	}

	
}
