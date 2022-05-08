using Newtonsoft.Json;

namespace AsistenciaBack.Model.Request;

public class LoginRequest
{
	[JsonProperty("email")]
	public string Email { get; set; } = string.Empty;
	[JsonProperty("contraseña")]
	public string Password { get; set; } = string.Empty;
}