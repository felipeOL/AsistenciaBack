using System.ComponentModel.DataAnnotations;
namespace AsistenciaBack.Model;

public class Curso
{
    [Key]
    public int Id {get; set;}
    public string Codigo {get; set;} = string.Empty;
    public string Nombre {get; set;} = string.Empty;
    public string Seccion {get; set;} = string.Empty;
    public string Semestre {get; set;} = string.Empty;
    public string Bloque {get; set;} = string.Empty;
    public virtual ICollection<User> Users{get; set;}=new HashSet<User>();
	public virtual ICollection<Clase> Clases { get; set; } = new HashSet<Clase>();
}