using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AsistenciaBack.Controller;

[ApiController, EnableCors("FrontendCors"), Route("api/usuario")]
public class UserController : ControllerBase
{
	private readonly IConfiguration _configuration;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly UserManager<User> _userManager;
	public UserController(IConfiguration configuration, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
	{
		this._configuration = configuration;
		this._roleManager = roleManager;
		this._userManager = userManager;
	}
	[HttpPost("registrar"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> Register([FromBody] RegisterRequest request)
	{
		var check = await this._userManager.FindByIdAsync(request.Email);
		if (check is not null)
		{
			return this.BadRequest($"Usuario {request.Email} ya existe");
		}
		var user = new User
		{
			Id = request.Email,
			Rut = request.Rut,
			UserName = request.Email,
			Email = request.Email,
			Name = request.Name
		};
		var result = await this._userManager.CreateAsync(user, request.Password);
		if (!result.Succeeded)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error al agregar el usuario {request.Email} al UserManager");
		}
		switch (request.Role)
		{
			case AccountType.Student:
				if (!await this._roleManager.RoleExistsAsync("Student"))
				{
					_ = await this._roleManager.CreateAsync(new("Student"));
				}
				var studentResult = await this._userManager.AddToRoleAsync(user, "Student");
				if (!studentResult.Succeeded)
				{
					return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error al agregar el estudiante {request.Email}");
				}
				break;
			case AccountType.Teacher:
				if (!await this._roleManager.RoleExistsAsync("Teacher"))
				{
					_ = await this._roleManager.CreateAsync(new("Teacher"));
				}
				var teacherResult = await this._userManager.AddToRoleAsync(user, "Teacher");
				if (!teacherResult.Succeeded)
				{
					return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error al agregar el profesor {request.Email}");
				}
				break;
			case AccountType.Administrator:
				if (!await this._roleManager.RoleExistsAsync("Administrator"))
				{
					_ = await this._roleManager.CreateAsync(new("Administrator"));
				}
				var administratorResult = await this._userManager.AddToRoleAsync(user, "Administrator");
				if (!administratorResult.Succeeded)
				{
					return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error al agregar el administrador{request.Email}");
				}
				break;
		}
		return this.Ok($"Usuario {request.Email} creado con éxito");
	}
	[HttpPost(template: "login"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
	{
		var user = await this._userManager.FindByIdAsync(request.Email);
		if (user is null)
		{
			return this.BadRequest($"combinacion de usuario {request.Email} y contraseña no encontrados");
		}
		var check = await this._userManager.CheckPasswordAsync(user, request.Password);
		if (!check)
		{
			return this.BadRequest("combinacion de usuario y contraseña no encontrados");
		}
		var roles = await this._userManager.GetRolesAsync(user);
		var claims = new List<Claim> {
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Name, user.Id)
		};
		foreach (var role in roles)
		{
			claims.Add(new(ClaimTypes.Role, role));
		}
		var authSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(this._configuration.GetSection("Token").Value));
		var token = new JwtSecurityToken(
			claims: claims,
			expires: DateTime.UtcNow.AddHours(1),
			signingCredentials: new(authSigningKey, SecurityAlgorithms.HmacSha512)
		);
		return this.Ok(new TokenResponse
		{
			Roles = roles,
			Token = new JwtSecurityTokenHandler().WriteToken(token),
		});
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator"), HttpGet("todos"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
	{
		var userResponses = new List<UserResponse>();
		var users = this._userManager.Users.ToList();
		foreach (var user in users)
		{
			var roles = await this._userManager.GetRolesAsync(user);
			var userResponse = new UserResponse
			{
				Email = user.Email,
				Name = user.Name,
				Rut = user.Rut
			};
			userResponse.Role = roles[0];
			userResponses.Add(userResponse);
		}
		if (!users.Any())
		{
			return this.NotFound("No hay alumnos");
		}
		return userResponses;
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Teacher"), HttpPost("crearEstudiantesProfesor"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<IEnumerable<BatchStudentResponse>>> BatchStudentRegister([FromBody] ICollection<string> requests)
	{
		var response = new List<BatchStudentResponse>();
		foreach (var email in requests)
		{
			var check = await this._userManager.FindByIdAsync(email);
			if (check is not null)
			{
				response.Add(new BatchStudentResponse
				{
					Email = email,
					Result = "Error: el usuario ya existe",
					HasCreated = false
				});
				continue;
			}
			var user = new User
			{
				Id = email,
				UserName = email,
				Email = email
			};
			var result = await this._userManager.CreateAsync(user, email);
			response.Add(new BatchStudentResponse
			{
				Email = email,
				Result = "OK",
				HasCreated = true
			});
			if (!await this._roleManager.RoleExistsAsync("Student"))
			{
				_ = await this._roleManager.CreateAsync(new("Student"));
			}
			var studentResult = await this._userManager.AddToRoleAsync(user, "Student");
			if (!studentResult.Succeeded)
			{
				return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error al agregar el estudiante {email}");
			}
		}
		return response;
	}
}