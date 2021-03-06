global using AsistenciaBack.Context;
global using AsistenciaBack.Model;
global using AsistenciaBack.Model.Request;
global using AsistenciaBack.Model.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

string? connection;
if (builder.Environment.IsDevelopment())
{
	Console.WriteLine("Builder: I'm in development!");
	connection = Environment.GetEnvironmentVariable("DATABASE_DEV");
}
else if (builder.Environment.IsStaging())
{
	Console.WriteLine("Builder: I'm in staging!");
	connection = Environment.GetEnvironmentVariable("DATABASE_STAGE");
}
else
{
	Console.WriteLine("Builder: I'm in production!");
	connection = Environment.GetEnvironmentVariable("DATABASE_PROD");
}

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("oauth2", new()
	{
		Description = "Uso: Bearer TOKEN",
		In = ParameterLocation.Header,
		Name = "Authorization",
		Scheme = "Bearer",
		Type = SecuritySchemeType.ApiKey
	});
	options.AddSecurityRequirement(new()
		{
			{
				new()
				{
					Reference = new()
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					},
					Scheme = "oauth2",
					Name = "Bearer",
					In = ParameterLocation.Header
				},
				new List<string>()
			}
		}
	);
	options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
	options.SignIn.RequireConfirmedEmail = false;
	options.SignIn.RequireConfirmedPhoneNumber = false;
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.Zero;
	options.Password.RequireDigit = false;
	options.Password.RequiredLength = 0;
	options.Password.RequiredUniqueChars = 0;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new()
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TOKEN") ?? "")),
		ValidateIssuer = false,
		ValidateAudience = false
	};
});

builder.Services.AddCors(options => options.AddPolicy("FrontendCors", builder => _ = builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

app.UseRouting();

app.UseCors("FrontendCors");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(async endpoints =>
{
	endpoints.MapGet("/", async context =>
	{
		context.Response.Redirect("swagger");
	});
	endpoints.MapControllers();
}
);

app.Run();