using cuaderno_del_profe.server.Models;
using cuaderno_del_profe.server.Entities;
using Microsoft.EntityFrameworkCore;

namespace cuaderno_del_profe.server.Repositories
{
    public class AsistenciaRepo : Repository<Asistencia, AsistenciaModel>
    {
        public AsistenciaEstudianteRepo asistenciaEstudianteRepo { get; set; }
        public AsistenciaRepo(DbContext dbContext) : base(
            dbContext, 
            new ObjectsMapper<AsistenciaModel, Asistencia>(m => new Asistencia() { 
                IdAsistencia = m.IdAsistencia,
                Fecha = m.Fecha,
                IdMateria = m.IdMateria,
                IdPeriodo = m.IdPeriodo,
            }), 
            (DB, filter) => from m in DB.Set<Asistencia>().Where(filter)
                            join ma in DB.Set<Materia>() on m.IdMateria equals ma.IdMateria
                            join p in DB.Set<Periodo>() on m.IdPeriodo equals p.IdPeriodo
                            select new AsistenciaModel()
                            {
                                IdAsistencia = m.IdAsistencia,
                                Fecha = m.Fecha,
                                IdMateria = m.IdMateria,
                                IdPeriodo = m.IdPeriodo,
                                Materia = ma.Nombre,
                                Periodo = p.Nombre,
                            }
        )
        {
            asistenciaEstudianteRepo = new AsistenciaEstudianteRepo(dbContext);
        }
        public override AsistenciaModel GetFirst(Func<Asistencia, bool> filter)
        {
            var found = base.GetFirst(filter);

            if (found != null) found.asistenciasEstudiantes = asistenciaEstudianteRepo.Get(x => x.IdAsistencia == found.IdAsistencia).ToList();

            return found;
        }
        public override Asistencia Add(AsistenciaModel model)
        {
            using (var trx = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var created = base.Add(model);
                    SaveChanges();

                    model.IdAsistencia = created.IdAsistencia;
                    asistenciaEstudianteRepo.SaveAsistenciasEstudiantes(model);

                    trx.Commit();
                    return created;
                }
                      catch (Exception ex)
                {
                    trx.Rollback();
                    throw ex;
                }
            }
        }
        public override void Edit(AsistenciaModel model)
        {
            using (var trx = dbContext.Database.BeginTransaction())
            {
                try
                {
                    base.Edit(model);

                    asistenciaEstudianteRepo.SaveAsistenciasEstudiantes(model);

                    trx.Commit();
                }
                catch (Exception ex)
                {
                    trx.Rollback();
                    throw ex;
                }
            }
        }
    }
}
