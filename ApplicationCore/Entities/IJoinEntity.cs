using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
	public interface IJoinEntity<TEntity>
	{
		TEntity Navigation { get; set; }
	}
}
