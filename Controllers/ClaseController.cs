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
    public ClaseController(AppDbContext context)
    {
        this._context = context;
    }
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator"), HttpPost, EnableCors("FrontendCors")]// en esta parte autorizamos el metodo y el usuario que lo puede usar.
    public async Task<ActionResult> CrearCurso([FromBody] Clase clase)
    {
        this._context.Clases.Add(clase);// aqui lo agregamos al contexto, falta agregarlo a la DB
        await this._context.SaveChangesAsync();// se aguada ne la DB de manera asincrona
        return this.Ok("se guardo con exito la clase");// retornamos un mensaje
    }
}