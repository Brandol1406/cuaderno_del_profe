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
    public class EstudiantesController : ControllerBase
    {
        private readonly Cuaderno_del_ProfeContext _context;
        private readonly EstudianteRepo repo;
        public EstudiantesController(Cuaderno_del_ProfeContext context)
        {
            _context = context;
            repo = new EstudianteRepo(context);
        }

        [HttpGet]
        public IEnumerable<EstudianteModel> Get()
        {
            return repo.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<EstudianteModel> Get(int id)
        {
            var estudiante = repo.GetFirst(x => x.IdEstudiante == id);

            if (estudiante == null) return NotFound();

            return estudiante;
        }

        [HttpPut("{id}")]
        public ActionResult<OperationResult> Put(int id, [FromBody] EstudianteModel model)
        {
            if (id != model.IdEstudiante)
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
        public ActionResult<OperationResult> Post(EstudianteModel model)
        {
            Estudiante created;
            try
            {
                model.Fregistro = DateTime.Now;
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
            return repo.Any(e => e.IdEstudiante == id);
        }
    }
}
