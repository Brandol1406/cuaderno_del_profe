using cuaderno_del_profe.server.Models;
using cuaderno_del_profe.server.Entities;
using Microsoft.EntityFrameworkCore;

namespace cuaderno_del_profe.server.Repositories
{
    public class InscripcionRepo : Repository<Inscripcion, InscripcionModel>
    {
        public InscripcionRepo(DbContext dbContext) : base(
            dbContext, 
            new ObjectsMapper<InscripcionModel, Inscripcion>(m => new Inscripcion() { 
                IdMateria = m.IdMateria,
                IdPeriodo = m.IdPeriodo,
                IdEstudiante = m.IdEstudiante,
                IdInscripcion = m.IdInscripcion,
            }), 
            (DB, filter) => from m in DB.Set<Inscripcion>().Where(filter)
                            join p in DB.Set<Periodo>() on m.IdPeriodo equals p.IdPeriodo
                            join ma in DB.Set<Materia>() on m.IdMateria equals ma.IdMateria
                            join e in DB.Set<Estudiante>() on m.IdEstudiante equals e.IdEstudiante
                            select new InscripcionModel()
                            {
                                IdMateria = m.IdMateria,
                                IdPeriodo = m.IdPeriodo,
                                IdEstudiante = m.IdEstudiante,
                                IdInscripcion = m.IdInscripcion,
                                Periodo = p.Nombre,
                                Materia = ma.Nombre,
                                Estudiante = $"{e.Nombres} {e.Apellidos}",
                                Matricula = e.Matricula,
                                FInicioPeriodo = p.Finicio,
                                FFinPeriodo = p.Ffin,
                            }
        )
        {

        }
        public void SaveInscripciones(EstudianteModel parent)
        {
            //Se remueven las viejas
            var set = dbContext.Set<Inscripcion>();
            var toRemove = set.Where(x => x.IdEstudiante == parent.IdEstudiante);
            set.RemoveRange(toRemove);
            SaveChanges();

            if (parent.Inscripciones == null) return;

            //Se añaden las nuevas
            var news = parent.Inscripciones.Select(x => new Inscripcion()
            {
                IdEstudiante = parent.IdEstudiante,
                IdMateria = x.IdMateria,
                IdPeriodo = x.IdPeriodo,
            });
            set.AddRange(news);
            SaveChanges();
        }
    }
}
