using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tcust.Models;

namespace Tcust.DAL
{
	public class TcustContextSeed
	{
		public static void SeedData(IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<TcustContext>();
				context.Database.Migrate();


				SeedTermYears(context);
				SeedTerms(context);


				seedDepartments(context);


			}



		}
		
		static void SeedTermYears(TcustContext context)
		{
			var before = new TermYear
			{
				Year = 77,
				Active = false,
				Title = "籌備階段"
			};
			CreateTermYear(before, context);

			for (int year = 78; year <= 106; year++)
			{
				var item = new TermYear
				{
					Year = year,
					Active = false,
					Title = String.Format("{0}學年度",year) 
				};

				CreateTermYear(item, context);
			}
		}
		static void CreateTermYear(TermYear termYear, TcustContext context)
		{
			var exist = context.TermYears.Where(c => c.Year == termYear.Year).FirstOrDefault();
			if (exist == null)
			{
				context.TermYears.Add(termYear);
				context.SaveChanges();
			}
		}


		static void SeedTerms(TcustContext context)
		{
			for (int year = 78; year <= 106; year++)
			{
				var first = new Term
				{
					Number = Convert.ToInt16(String.Format("{0}{1}", year, "1")),
					Active = false,
					Title = "第一學期"
				};
				CreateTerm(first, context);

				var second = new Term
				{
					Number = Convert.ToInt16(String.Format("{0}{1}", year,"2")),
					Active = false,
					Title = "第二學期"
				};

				CreateTerm(second, context);
			}
			
		}
		static void CreateTerm(Term term, TcustContext context)
		{
			var exist = context.Terms.Where(c => c.Number == term.Number).FirstOrDefault();
			if (exist == null)
			{

				term.TermYearId = context.TermYears.Where(c => c.Year == term.Year).FirstOrDefault().Id;
				context.Terms.Add(term);
				context.SaveChanges();
			}
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
			var departments = new List<Department>()
			{
				new Department{  Name="教務處",Code="104000",Active=true, Parent=0,  },
				new Department{  Name="學務處",Code="105000",Active=true, Parent=0,  },
				new Department{  Name="總務處",Code="106000",Active=true, Parent=0,  },
				new Department{  Name="研發處",Code="112000",Active=true, Parent=0,  },
				new Department{  Name="電算中心",Code="118000",Active=true, Parent=0,  },
				new Department{  Name="圖書館",Code="117000",Active=true, Parent=0,  },
				new Department{  Name="進修推廣部",Code="108000",Active=true, Parent=0,  },
				new Department{  Name="人事室",Code="109000",Active=true, Parent=0,  },
				new Department{  Name="國資中心",Code="120000",Active=true, Parent=0,  },
				new Department{  Name="教資中心",Code="119000",Active=true, Parent=0,  },
				new Department{  Name="護理系",Code="206000",Active=true, Parent=0,  },
				new Department{  Name="醫管系",Code="209000",Active=true, Parent=0,  },
				new Department{  Name="行管系",Code="211000",Active=true, Parent=0,  },
				new Department{  Name="科管系",Code="213000",Active=true, Parent=0,  },
				new Department{  Name="醫放系",Code="210000",Active=true, Parent=0,  },
				new Department{  Name="全人教育中心",Code="218000",Active=true, Parent=0,  },
				new Department{  Name="長照所",Code="219000",Active=true, Parent=0,  },

				new Department{  Name="文宣公關組",Code="103010",Active=true, Parent=0,  },
			};
			

			foreach (var department in departments)
			{
				string code = department.Code;
				var exist = context.Departments.Where(d => d.Code == code).FirstOrDefault();
				if (exist == null)
				{
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
