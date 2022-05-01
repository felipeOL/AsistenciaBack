using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace AsistenciaBack.Controller;

[ApiController, Route("api/usuario")]
public class UserController : ControllerBase
{
	private readonly IConfiguration _configuration;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly UserManager<IdentityUser> _userManager;
	public UserController(IConfiguration configuration, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
	{
		this._configuration = configuration;
		this._roleManager = roleManager;
		this._userManager = userManager;
	}
	[HttpPost("login"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto request)
	{
		var user = await this._userManager.FindByIdAsync(request.Run);
		if (user is null)
		{
			return this.BadRequest("(DEV) Usuario no existe.");
		}
		var check = await this._userManager.CheckPasswordAsync(user, request.Password);
		if (!check)
		{
			return this.BadRequest("(DEV) Contraseña incorrecta");
		}
		var roles = await this._userManager.GetRolesAsync(user);
		var claims = new List<Claim> {
			new(JwtRegisteredClaimNames.Jti, user.Id),
			new(JwtRegisteredClaimNames.Email, user.Email),
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
		return this.Ok(new TokenDto {
			Roles = roles,
			Token = new JwtSecurityTokenHandler().WriteToken(token),
		});
	}
	[HttpPost("registrar"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> Register([FromBody] RegisterDto request)
	{
		var check =
			from u in this._context.Users
			where u.Email == request.Email || u.Rut == request.Rut
			select u;
		if (check.Any())
		{
			return this.BadRequest("(DEV) Usuario ya existe.");
		}
		CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
		var user = new User
		{
			Email = request.Email,
			FullName = request.FullName,
			PasswordHash = passwordHash,
			PasswordSalt = passwordSalt,
			Rut = request.Rut,
		};
		if (this._context.Users is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) La lista de usuarios dentro del contexto es nula.");
		}
		var add = await this._context.Users.AddAsync(user);
		if (add.State != EntityState.Added)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) Error al agregar al nuevo usuario dentro del contexto.");
		}
		var save = await this._context.SaveChangesAsync();
		return save != 0 ? this.Ok("Usuario registrado con éxito.") : this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) Error al guardar el usuario dentro de la base de datos.");
	}
}