using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AsistenciaBack.Controller;
[ApiController, Route("api/clase")]//esto es para mapear el controlador
public class ClaseController: ControllerBase
{
    private readonly AppDbContext _context;
	private readonly UserManager<User> _userManager;
	public ClaseController(AppDbContext context, UserManager<User> userManager)
    {
        this._context = context;
		this._userManager = userManager;
    }
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator"), HttpPost("CrearClase"), EnableCors("FrontendCors")]// en esta parte autorizamos el metodo y el usuario que lo puede usar.
	public async Task<ActionResult> CrearClase([FromBody] ClaseDto claseDto)
	{
		var result = await this._context.Cursos.FindAsync(claseDto.CursoId);
		if (result is null)
		{
			return this.BadRequest("error curso no encontrado");
		}
		var claseNueva = new Clase
		{
            Sala = claseDto.Sala,
            Modalidad = claseDto.Modalidad,
            Bloque = claseDto.Bloque,
            Curso = result
		};
		result.Clases.Add(claseNueva);
		this._context.Clases.Add(claseNueva);
		await this._context.SaveChangesAsync();// se aguada ne la DB de manera asincrona
		return this.Ok("se guardo con exito");// retornamos un mensaje
	}
}