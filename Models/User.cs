namespace AsistenciaBack.Model;

public class User: IdentityUser
{
    public string Rut {get; set;} = string.Empty;
}