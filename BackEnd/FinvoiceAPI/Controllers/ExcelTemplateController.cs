using BAL.Models;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinvoiceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExcelTemplateController : ControllerBase
    {
        public ExcelTemplateController()
        {

        }

        #region Get Excel Template Details
        /// <summary>
        /// UC-32
        /// Get Excel Template Details
        /// </summary>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return Excel Template Detail screen if the access is successful</response>
        /// <response code="404">If the Excel Template is not found</response>
        [HttpGet]
        [ProducesResponseType(typeof(UserInfoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetExcelTemplateDetail(string excelTemplateID)
        {
            string errorMessage = "";
            bool status = false;

            try
            {
                if (string.IsNullOrEmpty(excelTemplateID))
                {
                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = "Something went wrong!"
                    });
                }
            }
            catch (Exception)
            {
                errorMessage = "Something went wrong!";
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
