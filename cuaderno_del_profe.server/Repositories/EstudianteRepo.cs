using cuaderno_del_profe.server.Models;
using cuaderno_del_profe.server.Entities;
using Microsoft.EntityFrameworkCore;

namespace cuaderno_del_profe.server.Repositories
{
    public class EstudianteRepo : Repository<Estudiante, EstudianteModel>
    {
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

        }
        public override Estudiante Add(EstudianteModel model)
        {
            model.Matricula = ""; //El valor que tiene se limpia, para luego generarlo
            model.Fregistro = DateTime.Now; //Fecha se registró
            var created = base.Add(model);
            created.Matricula = string.Concat(DateTime.Now.Year.ToString().Substring(2, 2), '-', created.IdEstudiante); //Se genera matricula (YY-#)
            SaveChanges();
            return created;
        }
        public override void Edit(EstudianteModel model)
        {
            base.Edit(model);
        }
    }
}
