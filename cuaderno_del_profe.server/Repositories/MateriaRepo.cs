using cuaderno_del_profe.server.Models;
using cuaderno_del_profe.server.Entities;
using Microsoft.EntityFrameworkCore;

namespace cuaderno_del_profe.server.Repositories
{
    public class MateriaRepo : Repository<Materia, MateriaModel>
    {
        public MateriaRepo(DbContext dbContext) : base(
            dbContext, 
            new ObjectsMapper<MateriaModel, Materia>(m => new Materia() { 
                Descripcion = m.Descripcion,
                IdMateria = m.IdMateria,
                Nombre = m.Nombre,
            }), 
            (DB, filter) => from m in DB.Set<Materia>().Where(filter)
                            select new MateriaModel()
                            {
                                Descripcion = m.Descripcion,
                                IdMateria = m.IdMateria,
                                Nombre = m.Nombre,
                            }
        )
        {

        }
    }
}
