using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Helpers
{
	public static class JsonHelpers
	{

		public static string ToJsonString(this object model)
		{
			var jsonSettings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
			};
			return JsonConvert.SerializeObject(model, jsonSettings);
		}



	}
}
