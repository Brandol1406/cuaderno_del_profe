using cuaderno_del_profe.server.Models;
using cuaderno_del_profe.server.Entities;
using Microsoft.EntityFrameworkCore;

namespace cuaderno_del_profe.server.Repositories
{
    public class EstudianteRepo : Repository<Estudiante, EstudianteModel>
    {
        public InscripcionRepo InscripcionRepo { get; set; }
        public EstudianteRepo(DbContext dbContext) : base(
            dbContext, 
            new ObjectsMapper<EstudianteModel, Estudiante>(m => new Estudiante() { 
                Apellidos = m.Apellidos,
                Direccion = m.Direccion,
                Email = m.Email,
                FechaNacimiento = m.FechaNacimiento,
                Fregistro = m.Fregistro ?? DateTime.Now,
                IdEstudiante = m.IdEstudiante,
                Matricula = m.Matricula,
                Nombres = m.Nombres,
                Sexo = m.Sexo,
                Telefono1 = m.Telefono1,
                Telefono2 = m.Telefono2,
            }), 
            (DB, filter) => from m in DB.Set<Estudiante>().Where(filter)
                            select new EstudianteModel()
                            {
                                Apellidos = m.Apellidos,
                                Direccion = m.Direccion,
                                Email = m.Email,
                                FechaNacimiento = m.FechaNacimiento,
                                Fregistro = m.Fregistro,
                                IdEstudiante = m.IdEstudiante,
                                Matricula = m.Matricula,
                                Nombres = m.Nombres,
                                Sexo = m.Sexo,
                                Telefono1 = m.Telefono1,
                                Telefono2 = m.Telefono2,
                            }
        )
        {
            InscripcionRepo = new InscripcionRepo(dbContext);
        }
        public override EstudianteModel GetFirst(Func<Estudiante, bool> filter)
        {
            var found = base.GetFirst(filter);

            if (found != null) found.Inscripciones = InscripcionRepo.Get(x => x.IdEstudiante == found.IdEstudiante).OrderBy(x => x.FInicioPeriodo).ToList();

            return found;
        }
        public override Estudiante Add(EstudianteModel model)
        {
            using (var trx = dbContext.Database.BeginTransaction())
            {
                try
                {
                    model.Matricula = ""; //El valor que tiene se limpia, para luego generarlo
                    model.Fregistro = DateTime.Now; //Fecha se registró
                    var created = base.Add(model);
                    created.Matricula = string.Concat(DateTime.Now.Year.ToString().Substring(2, 2), '-', created.IdEstudiante); //Se genera matricula (YY-#)
                    SaveChanges();

                    model.IdEstudiante = created.IdEstudiante;
                    InscripcionRepo.SaveInscripciones(model);

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
        public override void Edit(EstudianteModel model)
        {
            using (var trx = dbContext.Database.BeginTransaction())
            {
                try
                {
                    base.Edit(model);

                    InscripcionRepo.SaveInscripciones(model);

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
