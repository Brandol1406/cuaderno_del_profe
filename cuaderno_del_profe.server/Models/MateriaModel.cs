using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cuaderno_del_profe.server.Models
{
    public class MateriaModel
    {
        [Key]
        [Column("idMateria")]
        public int IdMateria { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Nombre { get; set; }

        [StringLength(250)]
        [Unicode(false)]
        public string Descripcion { get; set; }
    }
}
