using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cuaderno_del_profe.server.Models
{
    public class PeriodoModel
    {
        [Key]
        [Column("idPeriodo")]
        public int IdPeriodo { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Nombre { get; set; }

        [Column("FInicio", TypeName = "datetime")]
        public DateOnly Finicio { get; set; }

        [Column("FFin", TypeName = "datetime")]
        public DateOnly Ffin { get; set; }
    }
}
