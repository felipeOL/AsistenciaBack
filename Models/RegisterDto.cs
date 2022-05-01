namespace AsistenciaBack.Model;

public class RegisterDto
{
	public enum Type {
		Student,
		Teacher,
		Administrator,
	}
	public string Email { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string Rut { get; set; } = string.Empty;
	public Type Role { get; set; }
}