
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Blog.DAL;
using Blog.Services;
using Tcust.DAL;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

		#region   ConfigureServices


		public void ConfigureDevelopmentServices(IServiceCollection services)
		{
			// use in-memory database
			//ConfigureTestingServices(services);

			// use real database
			ConfigureProductionServices(services);

		}
		public void ConfigureTestingServices(IServiceCollection services)
		{
			// use in-memory database
			//services.AddDbContext<TcustContext>(c =>
			//	c.UseInMemoryDatabase("Catalog"));

			ConfigureServices(services);
		}

		public void ConfigureProductionServices(IServiceCollection services)
		{
			// use real database
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

			services.AddCors();



			ConfigureServices(services);
		}

		#endregion

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
        {
			services.AddScoped(typeof(IBlogRepository<>), typeof(BlogRepository<>));
			services.AddScoped(typeof(ITcustRepository<>), typeof(TcustRepository<>));

			services.AddScoped<IPostService, PostService>();
			services.AddScoped<ITopPostService, TopPostService>();

			//services.AddScoped<ICatalogService, CachedCatalogService>();
			//services.AddScoped<IBasketService, BasketService>();
			//services.AddScoped<IBasketViewModelService, BasketViewModelService>();
			//services.AddScoped<IOrderService, OrderService>();
			//services.AddScoped<IOrderRepository, OrderRepository>();
			//services.AddScoped<CatalogService>();
			//services.Configure<CatalogSettings>(Configuration);
			//services.AddSingleton<IUriComposer>(new UriComposer(Configuration.Get<CatalogSettings>()));

			//services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
			//services.AddTransient<IEmailSender, EmailSender>();


			services.AddMvc().AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
			

			
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

			app.UseCors(builder =>
				builder.WithOrigins("http://localhost:6330"));

			app.UseMvc();
        }
    }
}
