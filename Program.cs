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
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

string? connection;
if (builder.Environment.IsDevelopment())
{
	connection = Environment.GetEnvironmentVariable("DATABASE_DEV");
}
else if (builder.Environment.IsStaging())
{
	connection = Environment.GetEnvironmentVariable("DATABASE_STAGE");
}
else
{
	connection = Environment.GetEnvironmentVariable("DATABASE_PROD");
	builder.WebHost.UseKestrel(optinos =>
	{
		optinos.Listen(System.Net.IPAddress.Any, 7000);
	}
	);
}

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
builder.Services.AddHttpsRedirection(options =>
{
	options.HttpsPort = 7000;
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHsts();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("FrontendCors");

app.UseEndpoints(endpoints =>
{
	endpoints.MapGet("/", async context =>
	{
		context.Response.Redirect("swagger");
	});
	endpoints.MapControllers();
}
);

app.Run();
