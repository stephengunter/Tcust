using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tcust.Models;

namespace Tcust.DAL
{
	public class TcustContextSeed
	{
		public static void Seed(TcustContext context)
		{
			//seedCountries(context);
			//seedPartitions(context);
			//setTaiwanAreas(context);

			//seedPartners(context);

			//seedTypes(context);

			//seedDepartments(context);
		}
		static void seedCountries(TcustContext context)
		{
			var country = new CountryArea()
			{
				Name = "台灣",
				Active = true,
				Parent = 0
			};
			var exist = context.CountryAreas.Where(c => c.Name == country.Name).FirstOrDefault();
			if (exist == null)
			{
				context.CountryAreas.Add(country);
				context.SaveChanges();
			}




		}
		static void seedPartitions(TcustContext context)
		{
			var parent = context.CountryAreas.Where(c => c.Name == "台灣").FirstOrDefault();
			if (parent == null) throw new Exception();

			var areas = new List<string>() { "北區", "中區", "南區", "東區", "離島" };
			foreach (var name in areas)
			{
				var exist = context.CountryAreas.Where(c => c.Name == name).FirstOrDefault();
				if (exist == null)
				{
					var area = new CountryArea()
					{
						Name = name,
						Active = true,
						Parent = parent.Id,
						IsPartition = true
					};
					context.CountryAreas.Add(area);
				}
			}


			context.SaveChanges();
		}
		static void setTaiwanAreas(TcustContext context)
		{
			var tw = context.CountryAreas.Where(c => c.Name == "台灣").FirstOrDefault();
			if (tw == null) throw new Exception();

			string[] northCountries = { "台北市", "新北市", "基隆巿", "桃園縣", "新竹巿", "新竹縣", "苗栗縣" };

			seedPartionAreas(context, tw.Id, "北區", northCountries);

			string[] centerCountries = { "台中巿", "彰化縣", "南投縣", "雲林縣", "嘉義縣" };

			seedPartionAreas(context, tw.Id, "中區", centerCountries);

			string[] southCountries = { "台南巿", "高雄巿", "屏東縣" };

			seedPartionAreas(context, tw.Id, "南區", southCountries);

			string[] eastCountries = { "花蓮縣", "台東縣", "宜蘭縣" };

			seedPartionAreas(context, tw.Id, "東區", southCountries);

			string[] otherCountries = { "金門縣", "連江縣", "澎湖縣" };

			seedPartionAreas(context, tw.Id, "離島", otherCountries);
		}

		static void seedPartionAreas(TcustContext context, int parentId, string partionName, string[] countryNames)
		{
			var partion = context.CountryAreas.Where(c => c.Name == partionName && c.IsPartition).FirstOrDefault();
			if (partion == null) throw new Exception();

			foreach (var name in countryNames)
			{
				var country = context.CountryAreas.Where(c => c.Name == name).FirstOrDefault();
				if (country == null)
				{
					country = new CountryArea()
					{
						Name = name,
						Active = true,
						Parent = parentId
					};
					context.CountryAreas.Add(country);
				}

				country.PartitionId = partion.Id;
			}

			context.SaveChanges();

		}

		static void seedTypes(TcustContext context)
		{
			var types = new Dictionary<string, string>();
			types.Add( "union", "策略聯盟");
			types.Add( "intern", "企業實習");
			types.Add("project","產學合作");
			types.Add("sister", "姊妹校");

			foreach (var item in types)
			{
				string name = item.Key;
				string code = item.Value;
				var exist = context.Types.Where(t => t.Name == name).FirstOrDefault();
				if (exist == null)
				{
					context.Types.Add(new Tcust.Models.Type
					{
						Active = true,
						Code = code,
						Name = name
					});

					context.SaveChanges();
				}
			}


		}

		static void seedDepartments(TcustContext context)
		{
			var names = new Dictionary<string, string>();
			names.Add("護理系", "");
			names.Add("醫務暨健康管理系", "");
			names.Add("醫學影像暨放射科學所", "");
			names.Add("放射醫學科學研究所", "");
			names.Add("資訊科技與管理系", "");
			names.Add("行銷與流通管理系", "");
			names.Add("進修推廣部", "");
			names.Add("資工系", "");

			foreach (var item in names)
			{
				string name = item.Key;
				var exist = context.Types.Where(t => t.Name == name).FirstOrDefault();
				if (exist == null)
				{
					var department=new Department
					{
						Active = true,
						Name = name
					};

					string updatedBy = "";
					department.SetUpdated(updatedBy);

				    context.Departments.Add(department);

					context.SaveChanges();
				}
			}
		}

		static void seedPartners(TcustContext context)
		{
			string updatedBy = "Stephen";
			var partner = new Partner()
			{
				Active = true,
				CountryId = 1,
				AreaId = 7,
				Name = "統一超商",
				Type = PartnerType.Enterprise,

			};
			partner.SetUpdated(updatedBy);

			context.Partners.Add(partner);
			context.SaveChanges();
		}


	}
}
