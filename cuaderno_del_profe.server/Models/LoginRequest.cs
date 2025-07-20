using System.ComponentModel.DataAnnotations;

namespace cuaderno_del_profe.server.Models
{
    public class LoginRequest
    {
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Contrasena { get; set; }
    }
}
