
using IdentityApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using IdentityApp.Services;
using Tcust.DAL;
using Tcust.Services;
using Cat = Blog;
using Blog.Services;
using Blog.DAL;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;

namespace WebApi
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{

			services.AddMvcCore().AddAuthorization().AddApiExplorer()
						.AddJsonFormatters()
						.AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); 
            
				

			services.AddAuthentication("Bearer")
			.AddIdentityServerAuthentication(options =>
			{
				options.Authority = "http://localhost:50000";// Configuration["Settings:AuthUrl"]; //"http://localhost:50000";
				options.RequireHttpsMetadata = false;

				options.ApiName = "apiApp";


			});

			

			services.AddCors(options =>
			{
				// this defines a CORS policy called "default"
				options.AddPolicy("default", policy =>
				{
					//policy.WithOrigins("http://localhost:50003", "http://localhost:50002", "http://localhost:2397")
						policy.AllowAnyOrigin().AllowAnyHeader()
						.AllowAnyMethod();
				});
			});

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "WebApiStarter", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            });

            services.Configure<Blog.Settings>(Configuration.GetSection("BlogSettings"));

			services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

			services.AddDbContext<ApplicationDbContext>(c =>
			{
				try
				{
					c.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
				}
				catch (System.Exception ex)
				{
					var message = ex.Message;
				}
			});

			services.AddDbContext<TcustContext>(c =>
			{
				try
				{
					c.UseSqlServer(Configuration.GetConnectionString("TcustConnection"));
				}
				catch (System.Exception ex)
				{
					var message = ex.Message;
				}
			});

			services.AddDbContext<BlogContext>(c =>
			{
				try
				{
					c.UseSqlServer(Configuration.GetConnectionString("BlogConnection"));
				}
				catch (System.Exception ex)
				{
					var message = ex.Message;
				}
			});


			//Identity
			services.AddScoped(typeof(IIdentityRepository<>), typeof(IdentityRepository<>));

			services.AddScoped<IUserService, UserService>();


			//Tcust
			services.AddScoped(typeof(ITcustRepository<>), typeof(TcustRepository<>));

			services.AddScoped<ITermService, TermService>();
			services.AddScoped<IDepartmentService, DepartmentService>();

			//Blog
			services.AddScoped(typeof(IBlogRepository<>), typeof(BlogRepository<>));

			services.AddScoped<IPostService, PostService>();
			services.AddScoped<IAttachmentService, AttachmentService>();

		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseDeveloperExceptionPage();
			app.UseCors("default");

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WepApiStarter");
            });
            app.UseSwagger();

            app.UseAuthentication();

			app.UseMvc(routes =>
			{
				

				routes.MapRoute(
					name: "default",
					template: "{controller=Posts}/{action=Index}/{id?}");
				});
		}
	}

	
}
