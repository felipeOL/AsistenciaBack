namespace AsistenciaBack.Model;

public class User
{
	public int Id { get; set; }
	public byte[]? PasswordHash { get; set; }
	public byte[]? PasswordSalt { get; set; }
	public string Email { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string Rut { get; set; } = string.Empty;
}