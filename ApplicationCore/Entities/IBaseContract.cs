using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public interface IBaseContract
    {
		 DateTime? StartDate { get; set; }

		 DateTime? EndDate { get; set; }
	}
}
