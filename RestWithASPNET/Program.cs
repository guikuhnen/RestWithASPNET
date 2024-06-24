using EvolveDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using MySqlConnector;
using RestWithASPNET.Business;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository.Base;
using Serilog;

namespace RestWithASPNET
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();

			var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];
			builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 3, 0))));

			builder.Services.AddMvc(options =>
			{
				options.RespectBrowserAcceptHeader = true;
				options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
				options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
			}).AddXmlSerializerFormatters();

			builder.Services.AddApiVersioning();

			// Serilog
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console()
				.CreateLogger();

			// Dependency Injection
			builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
			builder.Services.AddScoped<IPersonBusiness, PersonBusiness>();
			builder.Services.AddScoped<IBookBusiness, BookBusiness>();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();

				MigrateDatabase(connection);
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}

		private static void MigrateDatabase(string connection)
		{
			try
			{
				var evolveConnection = new MySqlConnection(connection);
				var evolve = new Evolve(evolveConnection, Log.Information)
				{
					Locations = ["db/migrations", "db/dataset"],
					IsEraseDisabled = true
				};
				evolve.Migrate();
			}
			catch (Exception ex)
			{
				Log.Error("Database migration failed.", ex);
				throw;
			}
		}
	}
}
