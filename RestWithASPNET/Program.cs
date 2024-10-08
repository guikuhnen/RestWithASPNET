using EvolveDb;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using RestWithASPNET.Business;
using RestWithASPNET.Configurations;
using RestWithASPNET.HATEOAS.Hypermedia.Filters;
using RestWithASPNET.HATEOAS.Hypermedia.Enricher;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository;
using RestWithASPNET.Repository.Base;
using RestWithASPNET.Services;
using Serilog;
using System.Text;

namespace RestWithASPNET
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var appName = "Rest with ASP.NET";
			var appVersion = "v1";

			var tokenConfigurations = new TokenConfiguration();
			new ConfigureFromConfigurationOptions<TokenConfiguration>(builder.Configuration.GetSection("TokenConfigurations"))
				.Configure(tokenConfigurations);

			// Add services to the container.
			builder.Services.AddSingleton(tokenConfigurations);

			// Authentication
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new()
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ClockSkew = TimeSpan.Zero,
					ValidIssuer = tokenConfigurations.Issuer,
					ValidAudience = tokenConfigurations.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
				};
			});
			builder.Services.AddAuthorizationBuilder().AddPolicy("Bearer",
				new AuthorizationPolicyBuilder().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
					.RequireAuthenticatedUser()
					.Build()
			);

			builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
			{
				policy.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader();
			}));

			builder.Services.AddRouting(options => options.LowercaseUrls = true);
			builder.Services.AddControllers();

			var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];
			builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 3, 0))));

			builder.Services.AddMvc(options =>
			{
				options.RespectBrowserAcceptHeader = true;
				options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
				options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
			}).AddXmlSerializerFormatters();

			var filterOptions = new HyperMediaFilterOptions();
			filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
			filterOptions.ContentResponseEnricherList.Add(new BookEnricher());

			builder.Services.AddSingleton(filterOptions);

			builder.Services.AddApiVersioning();

			// Serilog
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console()
				.CreateLogger();

			// Dependency Injection
			builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			//// Authentication
			builder.Services.AddTransient<ITokenService, TokenService>();
			builder.Services.AddTransient<ILoginBusiness, LoginBusiness>();
			builder.Services.AddTransient<IUserRepository, UserRepository>();
			//// Business
			builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
			builder.Services.AddTransient<IPersonRepository, PersonRepository>();
			builder.Services.AddScoped<IPersonBusiness, PersonBusiness>();
			builder.Services.AddScoped<IBookBusiness, BookBusiness>();
			builder.Services.AddScoped<IFileBusiness, FileBusiness>();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc(appVersion,
					new OpenApiInfo
					{
						Title = appName,
						Version = appVersion,
						Description = "API RESTFul developed in course 'REST API's RESTFul do 0 � Azure com ASP.NET 8 e 5 e Docker'",
						Contact = new OpenApiContact
						{
							Name = "Guilherme Kuhnen",
							Url = new Uri("https://github.com/guikuhnen")
						}
					});
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme." +
						"\r\n\r\n Enter 'Bearer'[space] and then your token in the text input below." +
						"\r\n\r\nExample: \"Bearer 12345abcdef\"",
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						  new OpenApiSecurityScheme
						  {
							  Reference = new OpenApiReference
							  {
								  Type = ReferenceType.SecurityScheme,
								  Id = "Bearer"
							  }
						  },
						 Array.Empty<string>()
					}
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();

				/*if (!string.IsNullOrWhiteSpace(connection))
					MigrateDatabase(connection);*/
			}

			app.UseHttpsRedirection();

			app.UseCors();

			app.UseAuthorization();

			app.MapControllers();
			app.MapControllerRoute("DefaultApi", "{controller=values}/v{version=apiVersion}/{id?}");

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
