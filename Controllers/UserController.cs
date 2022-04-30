using System.Security.Claims;
using System.Security.Cryptography;
using AsistenciaBack.Context;
using AsistenciaBack.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AsistenciaBack.Controller;

[ApiController, Route("api/usuario")]
public class UserController : ControllerBase
{
	private readonly AppDbContext _context;
	public UserController(AppDbContext context) => this._context = context;
	public ActionResult Login([FromBody] LoginDto request)
	{
		var check =
			from u in this._context.Users
			where u.Email == request.Email
			select u;
		if (!check.Any()) {
			return this.BadRequest("Usuario no existe. (TODO MODIFICAR ESTE MENSAJE EN PRODUCCIÓN).");
		}
		var user = check.First();
		if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt)) {
			return this.BadRequest("Contraseña incorrecta. (TODO MODIFICAR ESTE MENSAJE EN PRODUCCIÓN).");
		}
		return this.Ok("TODO El tokens jaja asies");
	}
	[HttpPost("registrar")]
	public ActionResult Register([FromBody] RegisterDto request)
	{
		var check =
			from u in this._context.Users
			where u.Email == request.Email || u.Rut == request.Rut
			select u;
		if (check.Any())
		{
			return this.BadRequest("Usuario ya existe. (TODO MODIFICAR ESTE MENSAJE EN PRODUCCIÓN).");
		}
		CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
		var newUser = new User
		{
			Email = request.Email,
			FullName = request.FullName,
			PasswordHash = passwordHash,
			PasswordSalt = passwordSalt,
			Rut = request.Rut,
		};
		return this.Ok("Usuario registrado con éxito.");
	}
	private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
	{
		using var hmac = new HMACSHA512();
		passwordSalt = hmac.Key;
		passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

	}
	private static string CreateToken(User user)
	{
		var claims = new List<Claim>
		{
			new(ClaimTypes.Name, user.Email)
		};
		var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes()); // TODO token shit
	}
	private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
	{
		using var hmac = new HMACSHA512(passwordSalt);
		var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		return computedHash.SequenceEqual(passwordHash);
	}
}