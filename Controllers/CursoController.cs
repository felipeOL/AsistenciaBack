using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace AsistenciaBack.Controller;
[ApiController,Route("api/cursos"),EnableCors("FrontendCors")]//esto es para mapear el controlador
public class CursoController: ControllerBase
{
    private readonly AppDbContext _context;
    public CursoController(AppDbContext context)
    {
        this._context = context;
    }
    [Authorize(Roles ="Administrator"),HttpPost("Crear")]// en esta parte autorizamos el metodo y el usuario que lo puede usar.
    public async Task<ActionResult>CrearCurso([FromBody] Curso curso)
    {
        this._context.Cursos.Add(curso);// aqui lo agregamos al contexto, falta agregarlo a la DB
        await this._context.SaveChangesAsync();// se aguada ne la DB de manera asincrona 
        return this.Ok("se guardo con exito");// retornamos un mensaje

    }
}