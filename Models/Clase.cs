using System.ComponentModel.DataAnnotations;

namespace AsistenciaBack.Model;

public class Clase
{
    [Key]
    public int Id {get; set;}
    public string Sala {get; set;} = string.Empty;
    public string Modalidad {get; set;} = string.Empty;
    public string Bloque {get; set;} = string.Empty;
    public virtual Curso Curso { get; set; }
}