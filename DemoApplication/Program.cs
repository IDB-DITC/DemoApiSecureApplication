
using DemoApplication.Api.Services;
using DemoApplication.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
namespace DemoApplication.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddDbContext<SalesDb>(opt =>
			{
				opt.UseSqlServer("server = (localdb)\\MSSqlLocaldb; Database= demoAppDatabase;  trusted_connection =true;");//AttachDbFileName = Database\\demoAppDatabase.mdf;
			});



			builder.Services.AddIdentity<IdentityUser, IdentityRole>(
				//options => options.SignIn.RequireConfirmedAccount = true
				)
				.AddEntityFrameworkStores<SalesDb>();

			//builder.Services.TryAddScoped<SignInManager<IdentityUser>>();



			builder.Services.AddControllers()

			//	.AddJsonOptions(options =>
			//{

			//	options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
			//	options.JsonSerializerOptions.PropertyNamingPolicy = null;
			//})
				
				;


			builder.Services.Configure<JsonOptions>(opt =>
			{
				opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			});

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Scheme = "bearer",
					Description = "Please insert JWT token into field"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
							new string[] { }
						}
					});
			});

			builder.Services.AddTokenService();


			builder.Services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				//opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
.AddJwtBearer(opt =>
{


	var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SignKey"]);
	//opt.SaveToken = true;
	opt.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = false,
		ValidateAudience = false,
		RequireExpirationTime = true,
		ValidateLifetime = true,
	};
	opt.UseSecurityTokenValidators = true;
});






			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseStaticFiles();

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
