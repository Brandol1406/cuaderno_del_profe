using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace cuaderno_del_profe.server.Models
{
    public class CambiarContrasenaModel
    {
        [Required]
        public string ContrasenaActual { get; set; }
        [Required]
        public string NuevaContrasena { get; set; }
        [Required]
        public string ConfirmNuevaContrasena { get; set; }
    }
}
