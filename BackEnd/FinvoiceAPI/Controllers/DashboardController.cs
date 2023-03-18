using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinvoiceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;

        public DashboardController(IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
        }

        #region Get Probability
        /// <summary>
        /// UC25-1
        /// Get Probability
        /// </summary>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return dashboard screen if the access is successful</response>
        /// <response code="404"></response>
        [HttpGet]
        [ProducesResponseType(typeof(DashboardViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProbability()
        {
            string errorMessage = "";
            bool status = false;

            try
            {
                Role getRole = await _roleService.GetRole(new Role()
                {
                    Name = "User"
                }, "GetRoleName");

                int totalUser = _accountService.Count(new Account()
                {
                    UserRole = getRole.IdRole
                }, null);

                int totalExcelTemplates = 0;
                int totalXMLTemplates = 0;
                int totalSuppliers = 0;
                int totalAccountingSoftwares = 0;

                DashboardViewModel dashboardViewModel = new()
                {
                    TotalUsers = totalUser,
                    TotalExcelTemplates = totalExcelTemplates,
                    TotalXMLTemplates = totalXMLTemplates,
                    TotalSuppliers = totalSuppliers,
                    TotalAccountingSoftwares = totalAccountingSoftwares
                };

                if (dashboardViewModel != null)
                {
                    status = true;

                    return new JsonResult(new
                    {
                        UserInfo = dashboardViewModel,
                        Status = status,
                        ErrorMessage = errorMessage
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
