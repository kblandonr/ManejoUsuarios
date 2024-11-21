using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ManejoUsuarios.Models;

[Table("Usuarios")]
public partial class Usuario
{
    [JsonProperty(propertyName: "id")]
    public string id { get; set; }
    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public int? Edad { get; set; }
    
    
}
