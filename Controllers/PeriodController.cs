using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsistenciaBack.Controller;

[ApiController, EnableCors("FrontendCors"), Route("api/periodo")]
public class PeriodController : ControllerBase
{
	private readonly AppDbContext _context;
	private readonly UserManager<User> _userManager;
	public PeriodController(AppDbContext context, UserManager<User> userManager)
	{
		this._context = context;
		this._userManager = userManager;
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator"), HttpPost("crear"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> CreatePeriod([FromBody] PeriodRequest request)
	{
		Period? check = this._context.Periods.Where(p => p.Name == request.Name && p.Year == request.Year).FirstOrDefault();
		if (check is not null)
		{
			return this.BadRequest("Periodo ya existe");
		}
		var period = new Period
		{
			Name = request.Name,
			Year = request.Year,
			Start = request.Start,
			End = request.End
		};
		await this._context.Periods.AddAsync(period);
		await this._context.SaveChangesAsync();
		return this.Ok($"Periodo {request.Name} - {request.Year} creado con éxito");
	}
	[Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator,Teacher,Student"), HttpPost("obtener"), Produces("application/json"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<IEnumerable<Period>>> GetPeriods([FromBody] int request)
	{
		var periods = this._context.Periods.Where(p => p.Year == request).ToList();
		if (!periods.Any())
		{
			return this.NotFound($"No hay periodos creados en el año {request}");
		}
		return periods;
	}
}