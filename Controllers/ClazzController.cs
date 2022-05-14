using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsistenciaBack.Controller;

[ApiController, EnableCors("FrontendCors"), Route("api/clase")]
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
	// TODO El profe debe agregar clases
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator"), HttpPost("crear"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Student"), HttpGet("todosDesdeFecha"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<IEnumerable<ClazzResponse>>> GetAllClazzsFromDate([FromBody] GetStudentClassesFromDate request)
	{
		// Ver si el usuario actual existe
		if (this.HttpContext.User.Identity is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El User.Identity es nulo");
		}
		var currentUser = await this._userManager.FindByIdAsync(this.HttpContext.User.Identity.Name);
		if (currentUser is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) Usuario actual no encontrado");
		}
		// Ver si el usuario actual es un profesor
		var currentRoles = await this._userManager.GetRolesAsync(currentUser);
		if (!currentRoles.Contains("Student"))
		{
			return this.BadRequest($"(DEV) El usuario con ID {currentUser.Id} no es un estudiante");
		}
		if (this._context.Courses is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El contexto tiene la lista de cursos nula");
		}
		var courses = this._context.Courses.Include(c => c.Users).Where(c => c.Users.Contains(currentUser)).ToList();
		var response = new List<ClazzResponse>();
		foreach (var course in courses)
		{
			if (this._context.Clazzs is null)
			{
				return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El contexto tiene la lista de clases nula");
			}
			var classes = this._context.Clazzs
				.Where(c => c.Course != null && c.Course.Id == course.Id && c.Date.Date >= request.Date.Date)
				.Select(c => new ClazzResponse
				{
					Id = c.Id,
					Room = c.Room,
					Mode = c.Mode,
					Block = c.Block,
					Date = c.Date,
					Course = this._mapper.Map<CourseResponse>(course)
				}
				)
				.ToList();
			response.AddRange(classes);
		}
		return response;
	}
}