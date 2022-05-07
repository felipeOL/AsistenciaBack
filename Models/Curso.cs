namespace AsistenciaBack.Model;

public class Curso
{
    public int Id {get; set;}
    public string Codigo {get; set;} = string.Empty;
    public string Nombre {get; set;} = string.Empty;
    public string Seccion {get; set;} = string.Empty;
    public string Semestre {get; set;} = string.Empty;
    public string Bloque {get; set;} = string.Empty;
}