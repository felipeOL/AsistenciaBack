using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AsistenciaBack.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AsistenciaBack.Controller;

[ApiController, Route("api/usuario")]
public class UserController : ControllerBase
{
	private readonly AppDbContext _context;
	private readonly IConfiguration _configuration;
	public UserController(AppDbContext context, IConfiguration configuration)
	{
		this._context = context;
		this._configuration = configuration;
	}
	private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
	{
		using var hmac = new HMACSHA512();
		passwordSalt = hmac.Key;
		passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

	}
	private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
	{
		using var hmac = new HMACSHA512(passwordSalt);
		var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		return computedHash.SequenceEqual(passwordHash);
	}
	[HttpPost("login"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public ActionResult Login([FromBody] LoginDto request)
	{
		var check =
			from u in this._context.Users
			where u.Email == request.Email
			select u;
		if (!check.Any())
		{
			return this.BadRequest("(DEV) Usuario no existe.");
		}
		var user = check.First();
		if (user.PasswordHash is null || user.PasswordSalt is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) PasswordHash o PasswordSalt nulos.");
		}
		if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
		{
			return this.BadRequest("(DEV) Contraseña incorrecta.");
		}
		return this.Ok(this.CreateToken(user));
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
	private string CreateToken(User user)
	{
		var claims = new List<Claim>
			{
				new(ClaimTypes.Name, user.Email)
			};
		var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(this._configuration.GetSection("Token").Value));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
		var token = new JwtSecurityToken(
			claims: claims,
			expires: DateTime.Now.AddHours(1),
			signingCredentials: creds
		);
		var jwt = new JwtSecurityTokenHandler().WriteToken(token);
		return jwt;
	}
}