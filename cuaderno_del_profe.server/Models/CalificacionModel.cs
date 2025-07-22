using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cuaderno_del_profe.server.Models
{
    public class CalificacionModel
    {
        [Key]
        [Column("idCalificacion")]
        public int IdCalificacion { get; set; }

        [Column("idEstudiante")]
        public int IdEstudiante { get; set; }
        public string? Estudiante { get; set; }
        public string? Matricula { get; set; }

        [Column("idMateria")]
        public int IdMateria { get; set; }
        public string? Materia { get; set; }

        [Column("idPeriodo")]
        public int IdPeriodo { get; set; }
        public string? Periodo { get; set; }
        [Column("Calificacion")]
        public int Calificacion1 { get; set; }

        [Column("FRegistro", TypeName = "datetime")]
        public DateTime? Fregistro { get; set; }

        [Column("FEvaluacion", TypeName = "datetime")]
        public DateTime Fevaluacion { get; set; }
    }
}
