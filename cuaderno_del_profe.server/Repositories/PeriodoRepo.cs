using cuaderno_del_profe.server.Models;
using cuaderno_del_profe.server.Entities;
using Microsoft.EntityFrameworkCore;

namespace cuaderno_del_profe.server.Repositories
{
    public class PeriodoRepo : Repository<Periodo, PeriodoModel>
    {
        public PeriodoRepo(DbContext dbContext) : base(
            dbContext, 
            new ObjectsMapper<PeriodoModel, Periodo>(m => new Periodo() { 
                IdPeriodo = m.IdPeriodo,
                Nombre = m.Nombre,
                Finicio = m.Finicio,
                Ffin = m.Ffin,
            }), 
            (DB, filter) => from m in DB.Set<Periodo>().Where(filter)
                            select new PeriodoModel()
                            {
                                IdPeriodo = m.IdPeriodo,
                                Nombre = m.Nombre,
                                Finicio = m.Finicio,
                                Ffin = m.Ffin,
                            }
        )
        {

        }
    }
}
