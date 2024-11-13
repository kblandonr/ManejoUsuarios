using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManejoUsuarios.Models;

[Table("Usuarios")]
public partial class Usuario
{
    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public int? Edad { get; set; }
    [Key]
    public int Id { get; set; }
}
