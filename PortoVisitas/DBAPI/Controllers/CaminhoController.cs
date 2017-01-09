using ClassLibrary.DTO;
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
    public class CaminhoController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: api/Caminho
        [Route("api/Caminho")]
        [ResponseType(typeof(List<CaminhoDTO>))]
        public async Task<IHttpActionResult> GetCaminho()
        {
            List<CaminhoDTO> list = await unitOfWork.CaminhoRepository.FindCaminhos();

            return Ok(list);
        }

    }
}
