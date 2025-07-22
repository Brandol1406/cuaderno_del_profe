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
                Fregistro = m.Fregistro ?? DateTime.Now,
                IdEstudiante = m.IdEstudiante,
                IdMateria = m.IdMateria,
                IdPeriodo = m.IdPeriodo,
                Calificacion1 = m.Calificacion1,
            }), 
            (DB, filter) => from m in DB.Set<Calificacion>().Where(filter)
                            join ma in DB.Set<Materia>() on m.IdMateria equals ma.IdMateria
                            join e in DB.Set<Estudiante>() on m.IdEstudiante equals e.IdEstudiante
                            join p in DB.Set<Periodo>() on m.IdPeriodo equals p.IdPeriodo
                            select new CalificacionModel()
                            {
                                IdCalificacion = m.IdCalificacion,
                                Fevaluacion = m.Fevaluacion,
                                Fregistro = m.Fregistro,
                                IdEstudiante = m.IdEstudiante,
                                IdMateria = m.IdMateria,
                                IdPeriodo = m.IdPeriodo,
                                Calificacion1 = m.Calificacion1,
                                Estudiante = e.Nombres + " " + e.Apellidos,
                                Matricula = e.Matricula,
                                Materia = ma.Nombre,
                                Periodo = p.Nombre,
                            }
        )
        {

        }
    }
}
