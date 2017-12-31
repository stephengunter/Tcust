using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Paging
{
	public interface IPagingHeader
	{
		 int TotalItems { get; }
		 int PageNumber { get; }
		 int PageSize { get; }
		 int TotalPages { get; }

		 string ToJson();
	}
	public class PagingHeader: IPagingHeader
	{
		public PagingHeader(int totalItems, int pageNumber, int pageSize, int totalPages)
		{
			this.TotalItems = totalItems;
			this.PageNumber = pageNumber;
			this.PageSize = pageSize;
			this.TotalPages = totalPages;
		}

		public int TotalItems { get; }
		public int PageNumber { get; }
		public int PageSize { get; }
		public int TotalPages { get; }

		public string ToJson() => JsonConvert.SerializeObject(this,
									new JsonSerializerSettings
									{
										ContractResolver = new CamelCasePropertyNamesContractResolver()
									});

	}
}
