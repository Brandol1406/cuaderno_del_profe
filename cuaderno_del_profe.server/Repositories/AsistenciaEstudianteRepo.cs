using cuaderno_del_profe.server.Models;
using cuaderno_del_profe.server.Entities;
using Microsoft.EntityFrameworkCore;

namespace cuaderno_del_profe.server.Repositories
{
    public class AsistenciaEstudianteRepo : Repository<AsistenciaEstudiante, AsistenciaEstudianteModel>
    {
        public AsistenciaEstudianteRepo(DbContext dbContext) : base(
            dbContext, 
            new ObjectsMapper<AsistenciaEstudianteModel, AsistenciaEstudiante>(m => new AsistenciaEstudiante() { 
                IdEstudiante = m.IdEstudiante,
                IdAsistenciaEstudiante = m.IdAsistenciaEstudiante,
                IdAsistencia = m.IdAsistencia,
                Presente = m.Presente,
            }), 
            (DB, filter) => from m in DB.Set<AsistenciaEstudiante>().Where(filter)
                            join e in DB.Set<Estudiante>() on m.IdEstudiante equals e.IdEstudiante
                            select new AsistenciaEstudianteModel()
                            {
                                IdAsistenciaEstudiante = m.IdAsistenciaEstudiante,
                                IdAsistencia = m.IdAsistencia,
                                Presente = m.Presente,
                                IdEstudiante = m.IdEstudiante,
                                Estudiante = $"{e.Nombres} {e.Apellidos}",
                                Matricula = e.Matricula,
                            }
        )
        {

        }
        public void SaveAsistenciasEstudiantes(AsistenciaModel parent)
        {
            //Se remueven las viejas
            var set = dbContext.Set<AsistenciaEstudiante>();
            var toRemove = set.Where(x => x.IdAsistencia == parent.IdAsistencia);
            set.RemoveRange(toRemove);
            SaveChanges();

            if (parent.asistenciasEstudiantes == null) return;

            //Se añaden las nuevas
            var news = parent.asistenciasEstudiantes.Select(x => new AsistenciaEstudiante()
            {
                IdAsistencia = parent.IdAsistencia,
                IdEstudiante = x.IdEstudiante,
                Presente = x.Presente,
            });
            set.AddRange(news);
            SaveChanges();
        }
    }
}
