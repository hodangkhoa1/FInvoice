using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using BAL.Utils;
using BAL.Validators;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace FinvoiceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class XMLUserController : ControllerBase
    {
        public XMLUserController()
        {
            
        }

        #region Import XML
        /// <summary>
        /// UC-8
        /// Import XML with role user
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///
        ///         {
        ///           "File": "import file",
        ///         }
        ///         
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return if import is successful</response>
        /// <response code="400">If no file to import</response>
        [HttpPost()]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ImportXML([FromForm] ImportFileXML importFileXML)
        {
            string errorMessage = "";
            bool status = false;

            try
            {
                string path = importFileXML.File.FileName;
                try
                {
                    /*response = await _service.ImportCLasses(request, path);*/
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return BadRequest(new
            {
                Status = status,
                ErrorMessage = errorMessage
            });
        }
        #endregion
    }
}
