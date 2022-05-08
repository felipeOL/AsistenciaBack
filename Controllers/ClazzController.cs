using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AsistenciaBack.Controller;

[Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator"), ApiController, EnableCors("FrontendCors"), Route("api/clase")]//esto es para mapear el controlador
public class ClazzController : ControllerBase
{
	private readonly AppDbContext _context;
	private readonly UserManager<User> _userManager;
	private readonly IMapper _mapper;
	public ClazzController(AppDbContext context, UserManager<User> userManager, IMapper mapper)
	{
		this._context = context;
		this._userManager = userManager;
		this._mapper = mapper;
	}
	[HttpPost, Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> CreateClazz([FromBody] ClazzRequest clazzRequest)
	{
		if (this._context.Courses is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El contexto tiene la lista de cursos nula");
		}
		var course = await this._context.Courses.FindAsync(clazzRequest.CourseId);
		if (course is null)
		{
			return this.BadRequest($"(DEV) El curso con ID {clazzRequest.CourseId} no existe");
		}
		var @class = this._mapper.Map<Clazz>(clazzRequest);
		if (@class is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) Error al mapear la clase");
		}
		@class.Course = course;
		course.Clazzs.Add(@class);
		if (this._context.Clazzs is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El contexto tiene la lista de clases nula");
		}
		await this._context.Clazzs.AddAsync(@class);
		await this._context.SaveChangesAsync();
		return this.Ok("(DEV) Clase guardada con éxito");
	}
	[HttpGet("todos"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public ActionResult<IEnumerable<ClazzResponse>> GetAllClazzs()
	{
		if (this._context.Clazzs is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El contexto tiene la lista de clases nula");
		}
		return this._context.Clazzs.Select(c => this._mapper.Map<ClazzResponse>(c)).ToList();
	}
}