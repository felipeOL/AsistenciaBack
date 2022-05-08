using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AsistenciaBack.Controller;
[ApiController, Route("api/curso")]//esto es para mapear el controlador
public class CursoController: ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    public CursoController(AppDbContext context, UserManager<User> userManager)
    {
        this._context = context;
        this._userManager = userManager;
    }
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator"), HttpPost("CrearCurso/{id}"), EnableCors("FrontendCors")]// en esta parte autorizamos el metodo y el usuario que lo puede usar.
    public async Task<ActionResult> CrearCurso([FromBody] CursoDto cursoDto,string id)
    {
        var result = await this._userManager.FindByIdAsync(id);
        if(result is null)
        {
            return this.BadRequest("error usuario no encontrado");
        }
        var roles = await this._userManager.GetRolesAsync(result);
        var check = roles.Contains("Teacher");
        if(!check)
        {
            return this.BadRequest("error el usuario no es profesor");
        }
        var cursoNuevo = new Curso{
            Codigo = cursoDto.Codigo,
            Nombre = cursoDto.Nombre,
            Seccion = cursoDto.Seccion,
            Semestre = cursoDto.Semestre,
            Bloque = cursoDto.Bloque

        };
        cursoNuevo.Users.Add(result);
        result.Cursos.Add(cursoNuevo);
        this._context.Cursos.Add(cursoNuevo);// aqui lo agregamos al contexto, falta agregarlo a la DB
        await this._context.SaveChangesAsync();// se aguada ne la DB de manera asincrona
        return this.Ok("se guardo con exito");// retornamos un mensaje
    }
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator"), HttpGet("GetAllCursos"), EnableCors("FrontendCors")]
    public async Task<ActionResult<IEnumerable<Curso>>> GetAllCursos()
    {
        //ojo con las relaciones circulares
        return this._context.Cursos.Include(c => c.Users).ToList();
    }
}