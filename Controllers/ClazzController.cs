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
	public ClazzController(AppDbContext context, UserManager<User> userManager)
	{
		this._context = context;
		this._userManager = userManager;
	}
	// TODO El profe debe agregar clases
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,Teacher"), HttpPost("crear"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> CreateClazz([FromBody] ClazzRequest request)
	{
		var currentUser = await this._userManager.FindByIdAsync(this.HttpContext.User.Identity.Name);
		var currentRoles = await this._userManager.GetRolesAsync(currentUser);
		if (currentRoles.Contains("Teacher"))
		{
			var teacher = this._context.Users.Include(u => u.Courses).Where(u => u.Id == currentUser.Id).FirstOrDefault();
			var teacherCourses = teacher.Courses;
			var courseQuery = teacherCourses.Where(pc => pc.Id == request.CourseId).FirstOrDefault();
			if (courseQuery is null)
			{
				return this.BadRequest($"El profesor {currentUser.Id} no se encuentra asociado al curso {request.CourseId}");
			}
		}
		var course = await this._context.Courses.FindAsync(request.CourseId);
		if (course is null)
		{
			return this.BadRequest($"El curso {request.CourseId} no existe");
		}
		var @class = new Clazz
		{
			Room = request.Room,
			Mode = request.Mode,
			Date = request.Date
		};
		var block = await this._context.Blocks.FindAsync(request.BlockRequest.Id);
		@class.Block = block;
		@class.Course = course;
		course.Clazzs.Add(@class);
		await this._context.Clazzs.AddAsync(@class);
		await this._context.SaveChangesAsync();
		return this.Ok($"Clase con fecha {request.Date} guardada con éxito en el curso {request.CourseId}");
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Student,Teacher"), HttpPost("todosDesdeFecha"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<IEnumerable<ClazzResponse>>> GetAllClazzsFromDate([FromQuery] GetStudentClassesFromDate request)
	{
		var currentUser = await this._userManager.FindByIdAsync(this.HttpContext.User.Identity.Name);
		var courses = this._context.Courses.Include(c => c.Users).Where(c => c.Users.Contains(currentUser)).ToList();
		var response = new List<ClazzResponse>();
		var dateToCompare = DateTimeOffset.Parse(request.Date);
		foreach (var course in courses)
		{
			var classes = this._context.Clazzs
				.Where(c => c.Course != null && c.Course.Id == course.Id && DateTime.Compare(dateToCompare.Date, c.Date.Date) >= 0)
				.Select(c => new ClazzResponse
				{
					Id = c.Id,
					Room = c.Room,
					Mode = c.Mode,
					BlockResponse = new BlockResponse
					{
						Id = c.Block.Id,
						Day = c.Block.Day,
						Time = c.Block.Time
					},
					Date = c.Date,
					Course = new CourseResponse
					{
						Id = course.Id,
						Code = course.Code,
						Name = course.Name,
						Section = course.Section,
						Semester = course.Semester
					},
					IsAttended = c.Users.Contains(currentUser),
				}
				)
				.ToList();
			response.AddRange(classes);
		}
		if (!response.Any())
		{
			return this.NotFound("Usuario sin clases futuras");
		}
		return response;
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Student"), HttpPost("asistir"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> Attend([FromBody] AttendRequest request)
	{
		var currentUser = await this._userManager.FindByIdAsync(this.HttpContext.User.Identity.Name);
		var course = this._context.Courses.Include(c => c.Users).Include(c => c.Clazzs).Where(c => c.Users.Contains(currentUser) && c.Clazzs.Any(cz => cz.Id == request.ClazzId)).FirstOrDefault();
		if (course is null)
		{
			return this.BadRequest($"El estudiante {currentUser.Id} no se encuentra inscrito al curso asociado a la clase {request.ClazzId}");
		}
		var @class = this._context.Clazzs.Include(c => c.Users).Include(c => c.Course).Where(c => c.Id == request.ClazzId).FirstOrDefault();
		if (@class is null)
		{
			return this.BadRequest($"La clase {request.ClazzId} no existe");
		}
		if (@class.Users.Contains(currentUser))
		{
			return this.BadRequest($"El estudiante {currentUser.Id} ya asistió a la clase {request.ClazzId}");
		}
		@class.Users.Add(currentUser);
		currentUser.Clazzs.Add(@class);
		await this._context.SaveChangesAsync();
		return this.Ok($"Se marcó la asistencia del usuario {currentUser.Id} a la clase {request.ClazzId}");
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Teacher"), HttpPost("obtenerAsistencias"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<IEnumerable<AttendanceResponse>>> GetAttendances([FromBody] AttendRequest request)
	{
		var @class = this._context.Clazzs.Include(c => c.Users).Include(c => c.Course).ThenInclude(co => co.Users).Where(c => c.Id == request.ClazzId).FirstOrDefault();
		if (@class is null)
		{
			return this.BadRequest($"La clase {request.ClazzId} no existe");
		}
		var result = new List<AttendanceResponse>();
		foreach (var student in @class.Course.Users)
		{
			var roles = await this._userManager.GetRolesAsync(student);
			if (roles.Contains("Student"))
			{
				var attendanceResponse = new AttendanceResponse
				{
					Email = student.Email,
					HasAttend = @class.Users.Contains(student)
				};
				result.Add(attendanceResponse);
			}
		}
		if (!result.Any())
		{
			return this.NotFound("Usuario sin asistencias");
		}
		return result;
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Teacher"), HttpPost("marcarAsistencias"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> MarkAttendances([FromBody] AttendanceRequest request)
	{
		var @class = this._context.Clazzs.Include(c => c.Users).Include(c => c.Course).ThenInclude(co => co.Users).Where(c => c.Id == request.ClazzId).FirstOrDefault();
		if (@class is null)
		{
			return this.BadRequest($"La clase {request.ClazzId} no existe");
		}
		foreach (var attendanceResponse in request.AttendanceResponses)
		{
			var student = await this._context.Users.FindAsync(attendanceResponse.Email);
			var check = @class.Users.Contains(student);
			if (attendanceResponse.HasAttend)
			{
				if (!check)
				{
					@class.Users.Add(student);
					student.Clazzs.Add(@class);
				}
			}
			else
			{
				if (check)
				{
					@class.Users.Remove(student);
					student.Clazzs.Remove(@class);
				}
			}
		}
		await this._context.SaveChangesAsync();
		return this.Ok($"Se actualizó la lista de asistencia de la clase del día {@class.Date} del curso {@class.Course.Name}");
	}
}