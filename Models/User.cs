namespace AsistenciaBack.Model;

public class User: IdentityUser
{
    public string Rut {get; set;} = string.Empty;
    public virtual ICollection<Curso> Cursos{get;set;}= new HashSet<Curso>();
}