namespace AsistenciaBack.Model;

public class TokenDto
{
	public IList<string>? Roles { get; set; }
	public string Token { get; set; } = string.Empty;
}