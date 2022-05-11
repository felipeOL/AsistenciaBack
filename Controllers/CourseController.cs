using AutoMapper;
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
	private readonly IMapper _mapper;
	private readonly RoleManager<IdentityRole> _roleManager;
	public CourseController(AppDbContext context, UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
	{
		this._context = context;
		this._userManager = userManager;
		this._mapper = mapper;
		this._roleManager = roleManager;
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator"), HttpPost, Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> CreateCourse([FromBody] CourseRequest courseRequest)
	{
		var user = await this._userManager.FindByIdAsync(courseRequest.ProfessorId);
		if (user is null)
		{
			return this.BadRequest($"(DEV) El usuario con ID {courseRequest.ProfessorId} no existe");
		}
		var roles = await this._userManager.GetRolesAsync(user);
		if (!roles.Contains("Teacher"))
		{
			return this.BadRequest($"(DEV) El usuario {user.UserName} no es profesor");
		}
		var course = this._mapper.Map<Course>(courseRequest);
		if (course is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) Error al mapear el curso");
		}
		course.Users.Add(user);
		user.Courses.Add(course);
		if (this._context.Courses is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El contexto tiene la lista de cursos nula");
		}
		await this._context.Courses.AddAsync(course);
		await this._context.SaveChangesAsync();
		return this.Ok("(DEV) Curso guardado con éxito");
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator"), HttpGet("todos"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<IEnumerable<CourseResponse>>> GetAllCourses()
	{
		if (this._context.Courses is null)
		{
			return this.StatusCode(StatusCodes.Status500InternalServerError, "(DEV) El contexto tiene la lista de cursos nula");
		}
		var courses = this._context.Courses.Include(c => c.Users).ToList();
		var courseResponses = new List<CourseResponse>();
		foreach (var course in courses)
		{
			var courseResponse = this._mapper.Map<CourseResponse>(course);
			foreach (var user in course.Users)
			{
				var roles = await this._userManager.GetRolesAsync(user);
				if (roles.Contains("Teacher"))
				{
					courseResponse.Professor = this._mapper.Map<UserResponse>(user);
					courseResponse.Professor.Role = "Teacher";
					break;
				}
			}
			courseResponses.Add(courseResponse);
		}
		return courseResponses;
	}
}