using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsistenciaBack.Controller;

[ApiController, EnableCors("FrontendCors"), Route("api/curso")]
public class CourseController : ControllerBase
{
	private readonly AppDbContext _context;
	private readonly UserManager<User> _userManager;
	public CourseController(AppDbContext context, UserManager<User> userManager)
	{
		this._context = context;
		this._userManager = userManager;
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator"), HttpPost("crear"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> CreateCourse([FromBody] CourseRequest request)
	{
		var user = await this._userManager.FindByIdAsync(request.TeacherId);
		var course = new Course
		{
			Code = request.Code,
			Name = request.Name,
			Section = request.Section,
			Semester = request.Semester,
			Year = request.Year
		};
		foreach (var block in request.BlockRequests)
		{
			course.Blocks.Add(new Block
			{
				Day = block.Day,
				Time = block.Time
			});
		}
		course.Users.Add(user);
		user.Courses.Add(course);
		await this._context.Courses.AddAsync(course);
		await this._context.SaveChangesAsync();
		return this.Ok($"Curso con nombre {request.Name} guardado con éxito");
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,Teacher,Student"), HttpGet("todos"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<IEnumerable<CourseResponse>>> GetAllCourses()
	{
		var currentUser = await this._userManager.FindByIdAsync(this.HttpContext.User.Identity.Name);
		var currentRoles = await this._userManager.GetRolesAsync(currentUser);
		List<Course> courses;
		if (currentRoles.Contains("Administrator"))
		{
			courses = this._context.Courses.Include(c => c.Users).ToList();
		}
		else
		{
			courses = this._context.Courses.Include(c => c.Users).Include(c => c.Blocks).Where(c => c.Users.Contains(currentUser)).ToList();
		}
		var courseResponses = new List<CourseResponse>();
		foreach (var course in courses)
		{
			//var courseResponse = this._mapper.Map<CourseResponse>(course);
			var courseResponse = new CourseResponse
			{
				Id = course.Id,
				Code = course.Code,
				Name = course.Name,
				Section = course.Section,
				Semester = course.Semester,
				Year = course.Year
			};
			foreach (var block in course.Blocks)
			{
				courseResponse.BlockResponses.Add(
					new BlockResponse
					{
						Id = block.Id,
						Day = block.Day,
						Time = block.Time
					}
				);
			}
			foreach (var user in course.Users)
			{
				var roles = await this._userManager.GetRolesAsync(user);
				if (roles.Contains("Teacher"))
				{
					courseResponse.Teacher = new UserResponse
					{
						Email = user.Email,
						Name = user.Name,
						Rut = user.Rut,
						Role = "Teacher"
					};
					break;
				}
			}
			courseResponses.Add(courseResponse);
		}
		if (!courseResponses.Any())
		{
			return this.NotFound("Usuario sin cursos inscritos");
		}
		return courseResponses;
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Teacher"), HttpPost("agregarEstudiante"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> AddStudentToCourse([FromBody] AddStudentToCourse request)
	{
		var currentUser = await this._userManager.FindByIdAsync(this.HttpContext.User.Identity.Name);
		var course =
			this._context.Courses
				.Include(c => c.Users)
				.Where(c => c.Id == request.CourseId && c.Users.Contains(currentUser)).FirstOrDefault();
		if (course is null)
		{
			return this.BadRequest($"El curso {request.CourseId} no le pertenece al profesor {currentUser.Id}");
		}
		var student = await this._userManager.FindByIdAsync(request.StudentId);
		if (student is null)
		{
			return this.BadRequest($"El estudiante {request.StudentId} no existe");
		}
		if (course.Users.Contains(student))
		{
			return this.BadRequest($"El estudiante {request.StudentId} ya se encuentra inscrito al curso {request.CourseId}");
		}
		course.Users.Add(student);
		student.Courses.Add(course);
		await this._context.SaveChangesAsync();
		return this.Ok($"Estudiante con {request.StudentId} ha sido inscrito con éxito al curso {request.CourseId}");
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Student,Teacher"), HttpGet("horarios"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<IEnumerable<BlockScheduleResponse>>> GetSchedule()
	{
		var currentUser = await this._userManager.FindByIdAsync(this.HttpContext.User.Identity.Name);
		var courses = this._context.Courses.Include(c => c.Users).Include(c => c.Blocks).Where(c => c.Users.Contains(currentUser)).ToList();
		var blockScheduleResponses = new List<BlockScheduleResponse>();
		foreach (var course in courses)
		{
			UserResponse teacher = null;
			foreach (var user in course.Users)
			{
				var roles = await this._userManager.GetRolesAsync(user);
				if (roles.Contains("Teacher"))
				{
					teacher = new UserResponse
					{
						Email = user.Email,
						Name = user.Name,
						Rut = user.Rut,
						Role = "Teacher"
					};
					break;
				}
			}
			foreach (var block in course.Blocks)
			{
				var blockScheduleResponse = new BlockScheduleResponse
				{
					CourseName = course.Name,
					CourseSection = course.Section,
					TeacherName = teacher.Name,
					TeacherEmail = teacher.Email,
					Day = block.Day,
					Time = block.Time
				};
				blockScheduleResponses.Add(blockScheduleResponse);
			}
		}
		return blockScheduleResponses;
	}
}