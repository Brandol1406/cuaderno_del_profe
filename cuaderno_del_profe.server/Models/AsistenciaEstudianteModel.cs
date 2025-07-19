using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cuaderno_del_profe.server.Models
{
    public class AsistenciaEstudianteModel
    {
        [Key]
        [Column("idAsistenciaEstudiante")]
        public int IdAsistenciaEstudiante { get; set; }

        [Column("idAsistencia")]
        public int IdAsistencia { get; set; }

        [Column("idEstudiante")]
        public int IdEstudiante { get; set; }
        public string? Estudiante { get; set; }
        public string? Matricula { get; set; }

        public bool Presente { get; set; }
    }
}
