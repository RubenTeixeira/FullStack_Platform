using ClassLibrary.DTO;
using ClassLibrary.Models;
using DBAPI.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace DBAPI.Controllers
{
    public class PercursoController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: api/Percurso
        [ResponseType(typeof(List<PercursoDTO>))]
        public async Task<IHttpActionResult> GetPercurso()
        {
            List<Percurso> listPercursos = await unitOfWork.PercursoRepository.FindPercursos();

            List<PercursoDTO> list = unitOfWork.PercursoRepository.ConvertModelListToDTO(listPercursos);

            return Ok(list);
        }

        [Authorize]
        [Route("api/PercursoUser")]
        [ResponseType(typeof(List<PercursoDTO>))]
        public async Task<IHttpActionResult> GetUserPercurso(string email)
        {
            List<Percurso> listPercursos = await unitOfWork.PercursoRepository.FindPercursosByUser(email);

            List<PercursoDTO> list = unitOfWork.PercursoRepository.ConvertModelListToDTO(listPercursos);

            return Ok(list);
        }

        // GET: api/Percurso/5
        [ResponseType(typeof(PercursoDTO))]
        public async Task<IHttpActionResult> GetPercurso(int id)
        {
            Percurso percurso = await unitOfWork.PercursoRepository.FindPercursoByIDAsync(id);

            if (percurso == null)
            {
                return NotFound();
            }

            PercursoDTO dto = unitOfWork.PercursoRepository.ConvertModelToDTO(percurso);

            return Ok(dto);
        }

        // PUT: api/Percurso/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPercurso(int id, Percurso percurso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != percurso.PercursoID)
            {
                return BadRequest();
            }

            await unitOfWork.PercursoRepository.UpdatePercurso(percurso);

            try
            {
                await unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!unitOfWork.PercursoRepository.PercursoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Percurso
        [ResponseType(typeof(PercursoDTO))]
        public async Task<IHttpActionResult> PostPercurso(Percurso percurso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Percurso saved = await unitOfWork.PercursoRepository.CreatePercurso(percurso);

                var dto = unitOfWork.PercursoRepository.ConvertModelToDTO(saved);

                return CreatedAtRoute("DefaultApi", new { id = percurso.PercursoID }, dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Percurso/5
        [ResponseType(typeof(Percurso))]
        public async Task<IHttpActionResult> DeletePercurso(int id)
        {
            Percurso percurso = await unitOfWork.PercursoRepository.FindPercursoByIDAsync(id);

            if (percurso == null)
            {
                return NotFound();
            }

            await unitOfWork.PercursoRepository.DeletePercurso(id);

            return Ok(percurso);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}