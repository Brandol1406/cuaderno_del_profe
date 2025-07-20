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
    public class AsistenciaController : ControllerBase
    {
        private readonly Cuaderno_del_ProfeContext _context;
        private readonly AsistenciaRepo repo;
        public AsistenciaController(Cuaderno_del_ProfeContext context)
        {
            _context = context;
            repo = new AsistenciaRepo(context);
        }

        [HttpGet]
        public IEnumerable<AsistenciaModel> Get()
        {
            return repo.Get();
        }

        [HttpGet("{id}")]
        public ActionResult<AsistenciaModel> Get(int id)
        {
            var Asistencia = repo.GetFirst(x => x.IdAsistencia == id);

            if (Asistencia == null) return NotFound();

            return Asistencia;
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
        public ActionResult<OperationResult> Put(int id, [FromBody] AsistenciaModel model)
        {
            if (id != model.IdAsistencia)
            {
                return BadRequest();
            }

            try
            {
                if (!MateriaExists(model.IdMateria)) return BadRequest(new OperationResult(Field: nameof(model.IdMateria), $"La materia con el ID:{model.IdMateria} no existe"));
                if (!PeriodoExists(model.IdPeriodo)) return BadRequest(new OperationResult(Field: nameof(model.IdPeriodo), $"El periodo con en ID:{model.IdPeriodo} no existe"));
                if (!FechaDentroDePeriodo(model)) return BadRequest(new OperationResult(Field: nameof(model.Fecha), $"La fecha no está dentro del periodo seleccionado"));

                if (!isAsistenciaUnique(model)) return BadRequest(new OperationResult(false, "Ya se ha realizado este registro de asistencia"));

                if (!HayDuplicidades(model)) return BadRequest(new OperationResult(nameof(model.asistenciasEstudiantes), "Existen duplicidades, favor revisar"));

                repo.Edit(model);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ModelExists(id))
                {
                    return NotFound();
                }

                if (ex.InnerException != null && ex.InnerException.Message.Contains("FOREIGN KEY constraint"))
                {
                    return BadRequest(new OperationResult(false, "Uno o mas de los estudiantes no es valido"));
                }

                throw ex;
            }

            return new OperationResult(true, "Éxito al editar");
        }

        [HttpPost]
        public ActionResult<OperationResult> Post(AsistenciaModel model)
        {
            Asistencia created;
            try
            {
                if (!MateriaExists(model.IdMateria)) return BadRequest(new OperationResult(Field: nameof(model.IdMateria), $"La materia con el ID:{model.IdMateria} no existe"));
                if (!PeriodoExists(model.IdPeriodo)) return BadRequest(new OperationResult(Field: nameof(model.IdPeriodo), $"El periodo con en ID:{model.IdPeriodo} no existe"));
                if (!FechaDentroDePeriodo(model)) return BadRequest(new OperationResult(Field: nameof(model.Fecha), $"La fecha no está dentro del periodo seleccionado"));

                if (!isAsistenciaUnique(model)) return BadRequest(new OperationResult(false, "Ya se ha realizado este registro de asistencia"));

                if (!HayDuplicidades(model)) return BadRequest(new OperationResult(nameof(model.asistenciasEstudiantes), "Existen duplicidades, favor revisar"));

                created = repo.Add(model);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("FOREIGN KEY constraint"))
                {
                    return BadRequest(new OperationResult(false, "Uno o mas de los estudiantes no es valido"));
                }

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
            return repo.Any(e => e.IdAsistencia == id);
        }
        private bool MateriaExists(int id)
        {
            return _context.Materia.Any(x => x.IdMateria == id);
        }
        private bool PeriodoExists(int id)
        {
            return _context.Periodo.Any(x => x.IdPeriodo == id);
        }
        private bool FechaDentroDePeriodo(AsistenciaModel model)
        {
            return _context.Periodo.Any(x => x.Finicio <= model.Fecha && x.Ffin >= model.Fecha && x.IdPeriodo == model.IdPeriodo);
        }
        private bool HayDuplicidades(AsistenciaModel model)
        {
            if (model.asistenciasEstudiantes == null || model.asistenciasEstudiantes.Count == 0) return false;

            return model.asistenciasEstudiantes.GroupBy(x => x.IdEstudiante).Any(x => x.Count() > 1);
        }
        //Verifica si esta Asistencia es unica para a este estudiante, en este periodo y en esta materia
        private bool isAsistenciaUnique(AsistenciaModel model)
        {
            return !repo.Any(e => 
                e.IdPeriodo == model.IdPeriodo
                &&
                e.IdMateria == model.IdMateria
                &&
                e.Fecha == model.Fecha
                &&
                e.IdAsistencia != model.IdAsistencia
                );
        }
    }
}
