using cuaderno_del_profe.server.Entities;
using cuaderno_del_profe.server.Models;
using cuaderno_del_profe.server.Repositories;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin,Professor")]
    public class MateriaController : ControllerBase
    {
        private readonly Cuaderno_del_ProfeContext _context;
        private readonly MateriaRepo repo;
        public MateriaController(Cuaderno_del_ProfeContext context)
        {
            _context = context;
            repo = new MateriaRepo(context);
        }

        [HttpGet]
        public IEnumerable<MateriaModel> Get()
        {
            return repo.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<MateriaModel> Get(int id)
        {
            var Materia = repo.GetFirst(x => x.IdMateria == id);

            if (Materia == null) return NotFound();

            return Materia;
        }

        [HttpPut("{id}")]
        public ActionResult<OperationResult> Put(int id, [FromBody] MateriaModel model)
        {
            if (id != model.IdMateria)
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
        public ActionResult<OperationResult> Post(MateriaModel model)
        {
            Materia created;
            try
            {
                created = repo.Add(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new OperationResult(true, "Éxito al crear");
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
                    return BadRequest(new OperationResult(false, "No se puede eliminar porque tiene registros relacionados. Elimine todo con lo que este registro tiene relación primero"));
                }

                throw ex;
            }

            return new OperationResult(true, "Éxito al eliminar");
        }

        private bool ModelExists(int id)
        {
            return repo.Any(e => e.IdMateria == id);
        }
    }
}
