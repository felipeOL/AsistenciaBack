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
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,Teacher"), HttpPost("crear"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> CreateClazz([FromBody] ClazzRequest request)
	{
		if (this.HttpContext.User.Identity is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El User.Identity es nulo");
		}
		var currentUser = await this._userManager.FindByIdAsync(this.HttpContext.User.Identity.Name);
		if (currentUser is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) Usuario actual no encontrado");
		}
		var currentRoles = await this._userManager.GetRolesAsync(currentUser);
		if (currentRoles.Contains("Teacher"))
		{
			if (this._context.Courses is null)
			{
				return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El contexto tiene la lista de cursos nula");
			}
			var teacherCourse = this._context.Courses.Include(c => c.Users).Include(c => c.Clazzs).Where(c => c.Users.Contains(currentUser) && c.Clazzs.Any(cz => cz.Id == request.CourseId)).FirstOrDefault();
			if (teacherCourse is null)
			{
				return this.BadRequest($"(DEV) El profesor {currentUser.Id} no se encuentra asociado al curso con ID {request.CourseId}");
			}
		}
		if (this._context.Courses is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El contexto tiene la lista de cursos nula");
		}
		var course = await this._context.Courses.FindAsync(request.CourseId);
		if (course is null)
		{
			return this.BadRequest($"(DEV) El curso con ID {request.CourseId} no existe");
		}
		var @class = this._mapper.Map<Clazz>(request);
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
		return this.Ok($"(DEV) Clase con fecha {request.Date} guardada con éxito en el curso con ID {request.CourseId}");
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Student"), HttpGet("todosDesdeFecha"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<IEnumerable<ClazzResponse>>> GetAllClazzsFromDate([FromBody] GetStudentClassesFromDate request)
	{
		if (this.HttpContext.User.Identity is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El User.Identity es nulo");
		}
		var currentUser = await this._userManager.FindByIdAsync(this.HttpContext.User.Identity.Name);
		if (currentUser is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) Usuario actual no encontrado");
		}
		var currentRoles = await this._userManager.GetRolesAsync(currentUser);
		if (!currentRoles.Contains("Student"))
		{
			return this.BadRequest($"(DEV) El usuario actual con ID {currentUser.Id} no es un estudiante");
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
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Student"), HttpPost("asistir"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> Attend([FromBody] AttendRequest request)
	{
		if (this.HttpContext.User.Identity is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El User.Identity es nulo");
		}
		var currentUser = await this._userManager.FindByIdAsync(this.HttpContext.User.Identity.Name);
		if (currentUser is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) Usuario actual no encontrado");
		}
		var currentRoles = await this._userManager.GetRolesAsync(currentUser);
		if (!currentRoles.Contains("Student"))
		{
			return this.BadRequest($"(DEV) El usuario actual con ID {currentUser.Id} no es un estudiante");
		}
		if (this._context.Courses is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El contexto tiene la lista de cursos nula");
		}
		var course = this._context.Courses.Include(c => c.Users).Include(c => c.Clazzs).Where(c => c.Users.Contains(currentUser) && c.Clazzs.Any(cz => cz.Id == request.ClazzId)).FirstOrDefault();
		if (course is null)
		{
			return this.BadRequest($"(DEV) El estudiante {currentUser.Id} no se encuentra inscrito al curso asociado a la clase con ID {request.ClazzId}");
		}
		if (this._context.Clazzs is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El contexto tiene la lista de clases nula");
		}
		var @class = this._context.Clazzs.Include(c => c.Users).Include(c => c.Course).Where(c => c.Id == request.ClazzId).FirstOrDefault();
		if (@class is null)
		{
			return this.BadRequest($"(DEV) La clase con ID {request.ClazzId} no existe");
		}
		if (@class.Users.Contains(currentUser))
		{
			return this.BadRequest($"(DEV) El estudiante con ID {currentUser.Id} ya asistió a la clase con ID {request.ClazzId}");
		}
		@class.Users.Add(currentUser);
		currentUser.Clazzs.Add(@class);
		await this._context.SaveChangesAsync();
		return this.Ok($"(DEV) Se marcó la asistencia del usuario con ID {currentUser.Id} a la clase con ID {request.ClazzId}");
	}
}