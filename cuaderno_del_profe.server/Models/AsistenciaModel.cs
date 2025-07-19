using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cuaderno_del_profe.server.Models
{
    public class AsistenciaModel
    {
        [Key]
        [Column("idAsistencia")]
        public int IdAsistencia { get; set; }

        [Column("idMateria")]
        public int IdMateria { get; set; }
        public string? Materia { get; set; }

        [Column("idPeriodo")]
        public int IdPeriodo { get; set; }
        public string? Periodo { get; set; }

        public DateOnly Fecha { get; set; }
        public List<AsistenciaEstudianteModel> asistenciasEstudiantes { get; set; }
    }
}
