namespace AsistenciaBack.Model;

public class RegisterDto
{
	public string Email { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string Rut { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}