global using AsistenciaBack.Context;
global using AsistenciaBack.Model;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
var connection = builder.Configuration.GetConnectionString("Dev");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("oauth2", new()
	{
		Description = "Uso: Bearer [TOKEN].",
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
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
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
		IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Token").Value)),
		ValidateIssuer = false,
		ValidateAudience = false
	};
});
// TODO This thing is only local
builder.Services.AddCors(options => options.AddPolicy("FrontendCors", builder => _ = builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("FrontendCors");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
