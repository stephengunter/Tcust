using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ApplicationCore.Helpers
{
	public static class Extensions
	{
		

		public static bool CaseInsensitiveContains(this string text, string value)
		{
			StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase;
			return text.IndexOf(value, stringComparison) >= 0;
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return true;
			}
			/* If this is a list, use the Count property for efficiency. 
             * The Count property is O(1) while IEnumerable.Count() is O(N). */
			var collection = enumerable as ICollection<T>;
			if (collection != null)
			{
				return collection.Count < 1;
			}
			return !enumerable.Any();
		}

		public static IList<string> GetKeywords(this string input)
		{
			if (String.IsNullOrWhiteSpace(input) || String.IsNullOrEmpty(input)) return null;

			return input.Split(new string[] { "-", " " }, StringSplitOptions.RemoveEmptyEntries);

		}

		public static string ReverseString(this string str)
		{
			return String.IsNullOrEmpty(str) ? string.Empty : new string(str.ToCharArray().Reverse().ToArray());
		}
		public static int ToInt(this string str)
		{
			int value = 0;
			if (!int.TryParse(str, out value)) value = 0;

			return value;
		}
		public static bool ToBoolean(this string str)
		{
			if (String.IsNullOrEmpty(str)) return false;

			return (str.ToLower() == "true");
		}

		public static int ToInt(this bool val)
		{
			if (val) return 1;
			return 0;

		}
		

		public static DateTime? ToDatetimeOrNull(this string str)
		{
			DateTime dateValue;
			if (DateTime.TryParse(str, out dateValue)) return dateValue;

			return null;
				
		}

		public static DateTime ToDatetimeOrDefault(this string str, DateTime defaultValue)
		{
			DateTime dateValue;
			if (DateTime.TryParse(str, out dateValue)) return dateValue;

			return defaultValue;

		}

	}
}
