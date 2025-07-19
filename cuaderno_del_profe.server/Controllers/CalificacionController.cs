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

        [HttpGet]
        [Route("GetRelationObjects")]
        public object GetRelationObjects()
        {
            var inscripcionRepo = new InscripcionRepo(_context);
            return new { 
                materias = _context.Materia,
                periodos = _context.Periodo,
                inscripciones = inscripcionRepo.Get()
            };
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
                if (!MateriaExists(model.IdMateria)) return new OperationResult(Field: nameof(model.IdMateria), $"La materia con el ID:{model.IdMateria} no existe");
                if (!EstudianteExists(model.IdEstudiante)) return new OperationResult(Field: nameof(model.IdEstudiante), $"El estudiante con el ID:{model.IdEstudiante} no existe");
                if (!PeriodoExists(model.IdPeriodo)) return new OperationResult(Field: nameof(model.IdPeriodo), $"El periodo con en ID:{model.IdPeriodo} no existe");

                if (!isCalificacionUnique(model)) return new OperationResult(false, "Ya se ha realizado esta calificación");

                repo.Edit(model);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw ex;
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
                if (!MateriaExists(model.IdMateria)) return new OperationResult(Field: nameof(model.IdMateria), $"La materia con el ID:{model.IdMateria} no existe");
                if (!EstudianteExists(model.IdEstudiante)) return new OperationResult(Field: nameof(model.IdEstudiante), $"El estudiante con el ID:{model.IdEstudiante} no existe");
                if (!PeriodoExists(model.IdPeriodo)) return new OperationResult(Field: nameof(model.IdPeriodo), $"El periodo con en ID:{model.IdPeriodo} no existe");

                if (!isCalificacionUnique(model)) return new OperationResult(false, "Ya se ha realizado esta calificación");

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
                    return new OperationResult(false, "No se puede eliminar porque tiene registros relacionados. Elimine todo con lo que este registro tiene relación primero");
                }

                throw ex;
            }

            return new OperationResult(true, "Éxito al eliminar");
        }
        //Verifica si existe el modelo por su ID
        private bool ModelExists(int id)
        {
            return repo.Any(e => e.IdCalificacion == id);
        }
        private bool MateriaExists(int id)
        {
            return _context.Materia.Any(x => x.IdMateria == id);
        }
        private bool EstudianteExists(int id)
        {
            return _context.Estudiante.Any(x => x.IdEstudiante == id);
        }
        private bool PeriodoExists(int id)
        {
            return _context.Periodo.Any(x => x.IdPeriodo == id);
        }
        //Verifica si esta calificacion es unica para a este estudiante, en este periodo y en esta materia
        private bool isCalificacionUnique(CalificacionModel model)
        {
            return !repo.Any(e => 
                e.IdPeriodo == model.IdPeriodo
                &&
                e.IdMateria == model.IdMateria
                &&
                e.IdEstudiante == model.IdEstudiante
                &&
                e.IdCalificacion != model.IdCalificacion
                );
        }
    }
}
