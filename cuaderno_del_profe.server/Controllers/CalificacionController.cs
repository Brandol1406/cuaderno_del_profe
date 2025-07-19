using cuaderno_del_profe.server.Entities;
using cuaderno_del_profe.server.Models;
using cuaderno_del_profe.server.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cuaderno_del_profe.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalificacionController : ControllerBase
    {
        private readonly Cuaderno_del_ProfeContext _context;
        private readonly CalificacionRepo repo;
        public CalificacionController(Cuaderno_del_ProfeContext context)
        {
            _context = context;
            repo = new CalificacionRepo(context);
        }

        [HttpGet]
        public IEnumerable<CalificacionModel> Get()
        {
            return repo.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<CalificacionModel> Get(int id)
        {
            var Calificacion = repo.GetFirst(x => x.IdCalificacion == id);

            if (Calificacion == null) return NotFound();

            return Calificacion;
        }

        [HttpPut("{id}")]
        public ActionResult<OperationResult> Put(int id, [FromBody] CalificacionModel model)
        {
            if (id != model.IdCalificacion)
            {
                return BadRequest();
            }

            try
            {
                repo.Edit(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new OperationResult(true, "Éxito al editar");
        }

        [HttpPost]
        public ActionResult<OperationResult> Post(CalificacionModel model)
        {
            Calificacion created;
            try
            {
                created = repo.Add(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new OperationResult(true, "Éxito al crear", created);
        }

        [HttpDelete("{id}")]
        public ActionResult<OperationResult> Delete(int id)
        {
            if (!ModelExists(id)) return NotFound();

            try
            {
                repo.Delete(id);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE constraint"))
                {
                    return new OperationResult(false, "No se puede eliminar porque tiene registros relacionados. Elimine todo con lo que este registro tiene relación primero");
                }

                throw ex;
            }

            return new OperationResult(true, "Éxito al eliminar");
        }

        private bool ModelExists(int id)
        {
            return repo.Any(e => e.IdCalificacion == id);
        }
    }
}
