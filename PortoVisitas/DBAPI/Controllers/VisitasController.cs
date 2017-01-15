using ClassLibrary.DTO;
using ClassLibrary.Models;
using DBAPI.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace DBAPI.Controllers
{
    public class VisitasController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: api/Visitas
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                List<Visita> listVisitas = await unitOfWork.VisitaRepository.FindVisitas();
                return Ok(unitOfWork.VisitaRepository.ConvertModelListToDTO(listVisitas));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // GET: api/Visitas/5
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                Visita visita = await unitOfWork.VisitaRepository.FindVisitaByIDAsync(id);
                return Ok(unitOfWork.VisitaRepository.ConvertModelToDTO(visita));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet]
        [Route("api/UserVisitas")]
        public async Task<IHttpActionResult> GetUserVisitas(string email)
        {
            try
            {
                List<Visita> visitasList = await unitOfWork.VisitaRepository.FindUserVisitasAsync(email);
                return Ok(unitOfWork.VisitaRepository.ConvertModelListToDTO(visitasList));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // POST: api/Visitas
        public async Task<IHttpActionResult> Post(VisitaDTO visitaDto)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                string problems = "";
                foreach (string error in allErrors)
                {
                    problems += " _____ " + error;
                }
                return BadRequest(ModelState);
            }

            try
            {
                Visita visita = unitOfWork.VisitaRepository.ConvertDTOToModel(visitaDto);
                Visita saved = await unitOfWork.VisitaRepository.CreateVisita(visita);
                VisitaDTO response = unitOfWork.VisitaRepository.ConvertModelToDTO(saved);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Exception Occured: " + ex.Message);
            }
        }

        // PUT: api/Visitas/5
        public async Task<IHttpActionResult> Put(int id, VisitaDTO visitaDto)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                string problems = "";
                foreach (string error in allErrors)
                {
                    problems += " _____ " + error;
                }
                return BadRequest("Invalid model object: " + problems);
            }

            if (id != visitaDto.VisitaID)
            {
                return BadRequest("ID received is not equal to Visita ID");
            }

            try
            {
                Visita visita = unitOfWork.VisitaRepository.ConvertDTOToModel(visitaDto);
                await unitOfWork.VisitaRepository.UpdateVisita(visita);
            }
            catch (Exception ex)
            {
                return BadRequest("Exception ocurred: " + ex.Message);
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // DELETE: api/Visitas/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            Visita visita = await unitOfWork.VisitaRepository.FindVisitaByIDAsync(id);

            if (visita == null)
            {
                return NotFound();
            }

            await unitOfWork.VisitaRepository.DeleteVisita(id);

            return Ok(visita);
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
