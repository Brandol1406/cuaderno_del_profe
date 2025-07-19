using cuaderno_del_profe.server.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cuaderno_del_profe.server.Models
{
    public class EstudianteModel
    {
        [Key]
        [Column("idEstudiante")]
        public int IdEstudiante { get; set; }

        [StringLength(20)]
        [Unicode(false)]
        public string? Matricula { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(1)]
        [Unicode(false)]
        public string Sexo { get; set; }

        public DateOnly? FechaNacimiento { get; set; }

        [StringLength(150)]
        [Unicode(false)]
        public string? Direccion { get; set; }

        [StringLength(12)]
        [Unicode(false)]
        public string? Telefono1 { get; set; }

        [StringLength(12)]
        [Unicode(false)]
        public string? Telefono2 { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string? Email { get; set; }
        [Column("FRegistro", TypeName = "datetime")]
        public DateTime? Fregistro { get; set; }
        public List<InscripcionModel>? Inscripciones { get; set; }
    }
}
