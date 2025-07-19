using cuaderno_del_profe.server.Models;
using cuaderno_del_profe.server.Entities;
using Microsoft.EntityFrameworkCore;

namespace cuaderno_del_profe.server.Repositories
{
    public class CalificacionRepo : Repository<Calificacion, CalificacionModel>
    {
        public CalificacionRepo(DbContext dbContext) : base(
            dbContext, 
            new ObjectsMapper<CalificacionModel, Calificacion>(m => new Calificacion() { 
                IdCalificacion = m.IdCalificacion,
                Fevaluacion = m.Fevaluacion,
                Fregistro = m.Fregistro,
                IdEstudiante = m.IdEstudiante,
                IdMateria = m.IdMateria,
                IdPeriodo = m.IdPeriodo,
                Calificacion1 = m.Calificacion1,
            }), 
            (DB, filter) => from m in DB.Set<Calificacion>().Where(filter)
                            select new CalificacionModel()
                            {
                                IdCalificacion = m.IdCalificacion,
                                Fevaluacion = m.Fevaluacion,
                                Fregistro = m.Fregistro,
                                IdEstudiante = m.IdEstudiante,
                                IdMateria = m.IdMateria,
                                IdPeriodo = m.IdPeriodo,
                                Calificacion1 = m.Calificacion1,
                            }
        )
        {

        }
    }
}
