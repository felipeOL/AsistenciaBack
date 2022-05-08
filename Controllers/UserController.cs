using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AsistenciaBack.Controller;

[ApiController, Route("api/usuario")]
public class UserController : ControllerBase
{
	private readonly IConfiguration _configuration;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly UserManager<User> _userManager;
	private readonly IMapper _mapper;
	public UserController(IConfiguration configuration, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IMapper mapper)
	{
		this._configuration = configuration;
		this._roleManager = roleManager;
		this._userManager = userManager;
		this._mapper = mapper;
	}
	[EnableCors("FrontendCors"), HttpPost("registrar"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> Register([FromBody] RegisterRequest request)
	{
		var check = await this._userManager.FindByIdAsync(request.Email);
		if (check is not null)
		{
			return this.BadRequest("(DEV) Usuario ya existe");
		}
		var user = new User
		{
			Rut = request.Rut,
			Id = request.Email,
			UserName = request.Name,
		};
		var result = await this._userManager.CreateAsync(user, request.Password);
		if (!result.Succeeded)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) Error al agregar el usuario al UserManager");
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
					return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) Error al agregar el usuario al rol de estudiante");
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
					return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) Error al agregar el usuario al rol de profesor");
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
					return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) Error al agregar el usuario al rol de administrador");
				}
				break;
		}
		return this.Ok("(DEV) Usuario creado con éxito");
	}
	[EnableCors("FrontendCors"), HttpPost("login"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
	{
		var user = await this._userManager.FindByIdAsync(request.Email);
		if (user is null)
		{
			return this.BadRequest("(DEV) Usuario no existe");
		}
		var check = await this._userManager.CheckPasswordAsync(user, request.Password);
		if (!check)
		{
			return this.BadRequest("(DEV) Contraseña incorrecta");
		}
		var roles = await this._userManager.GetRolesAsync(user);
		var claims = new List<Claim> {
			new(JwtRegisteredClaimNames.Jti, user.Id),
			new(JwtRegisteredClaimNames.Name, user.UserName)
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
}