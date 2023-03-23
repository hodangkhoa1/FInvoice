using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using BAL.Utils;
using BAL.Validators;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinvoiceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountingSoftwareController : ControllerBase
    {
        private readonly IAccountingSoftwareService _accountingSoftwareService;

        public AccountingSoftwareController(IAccountingSoftwareService accountingSoftwareService)
        {
            _accountingSoftwareService = accountingSoftwareService;
        }

        #region Add Accounting Software
        /// <summary>
        /// UC-62
        /// Add Accounting Software with role admin
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///
        ///         {
        ///           "Name": "test",
        ///           "Logo": "import picture",
        ///         }
        ///         
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return list if the add is successful</response>
        /// <response code="400">If the Accounting Software name is exist</response>
        [HttpPost()]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAccountingSoftware([FromForm] AccountingSoftwareViewModel accountingSoftwareViewModel)
        {
            string errorMessage = "", accountingSoftwareID = FunctionRandom.RandomCode(21);
            bool status = false;
            AccountingSoftwareValidator accountingSoftwareValidator = new();

            try
            {
                if (accountingSoftwareViewModel == null)
                {
                    throw new Exception("Api's information has been corrupted, please try again or contact developer for more support");
                }

                var validation = accountingSoftwareValidator.Validate(accountingSoftwareViewModel);

                if (!validation.IsValid)
                {
                    var errors = new List<string>();

                    foreach (var error in validation.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }

                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = errors
                    });
                }

                AccountingSoftware accountingSoftware = new()
                {
                    IdAccountingSoftware = accountingSoftwareID,
                    Name = accountingSoftwareViewModel.Name,
                    Logo = accountingSoftwareViewModel.Logo,
                    ColorLogo = FunctionRandom.ColorAvatar(),
                    DefaultLogo = accountingSoftwareViewModel.Name.FirstOrDefault(),
                    Status = 1
                };

                bool checkAddAccountingSoftware = await _accountingSoftwareService.ActionAccountingSoftware(accountingSoftware, "AddAccountingSoftware");

                if (checkAddAccountingSoftware == true)
                {
                    errorMessage = "Add accounting software successfully!";
                    status = true;

                    return Ok(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
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

        #region Update Accounting Software
        /// <summary>
        /// UC-60
        /// Update Accounting Software with role Admin
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///
        ///         {
        ///           "IdAccountingSoftware": "123",
        ///           "Name": "Test",
        ///           "Logo": "Import image"
        ///         }
        ///         
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return list if the update is successful</response>
        /// <response code="400">If the accounting software name is exist</response>
        [HttpPost()]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAccountingSoftware([FromForm] EditAccountingSoftwareViewModel editAccountingSoftwareViewModel)
        {
            string errorMessage = "";
            bool status = false;
            EditAccountingSoftwareValidator editAccountingSoftwareValidator = new();

            try
            {
                if (editAccountingSoftwareViewModel == null)
                {
                    throw new Exception("Api's information has been corrupted, please try again or contact developer for more support");
                }

                var validation = editAccountingSoftwareValidator.Validate(editAccountingSoftwareViewModel);

                if (!validation.IsValid)
                {
                    var errors = new List<string>();

                    foreach (var error in validation.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }

                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = errors
                    });
                }

                AccountingSoftware accountingSoftware = new()
                {
                    IdAccountingSoftware = editAccountingSoftwareViewModel.IdAccountingSoftware,
                    Name = editAccountingSoftwareViewModel.Name,
                    Logo = editAccountingSoftwareViewModel.Logo
                };

                bool existingAccountingSoftware = await _accountingSoftwareService.ActionAccountingSoftware(accountingSoftware, "EditAccountingSoftware");

                if (existingAccountingSoftware == true)
                {
                    errorMessage = "Accounting software was updated successfully!";
                    status = true;

                    return Ok(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
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

        #region Edit Status Accounting Software
        /// <summary>
        /// UC-58
        /// Edit status Accounting Software with role admin
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///
        ///         {
        ///           "AccountingSoftwareID": "123",
        ///         }
        ///         
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return list if the update is successful</response>
        /// <response code="400">If the Accounting Software ID is not exist</response>
        [HttpGet("{ASID}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditStatusAccountingSoftware(string accountingSoftwareID, string action)
        {
            string errorMessage = "";
            bool status = false;

            try
            {
                if (string.IsNullOrEmpty(accountingSoftwareID))
                {
                    errorMessage = "Accounting Software id is required!";

                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
                }

                AccountingSoftware accountingSoftware = new()
                {
                    IdAccountingSoftware = accountingSoftwareID,
                    Status = action.Equals("Active") ? 1 : 2
                };

                bool checkUpdateAccountingSoftware = await _accountingSoftwareService.ActionAccountingSoftware(accountingSoftware, "EditStatus");

                if (checkUpdateAccountingSoftware == true)
                {
                    errorMessage = "The accounting software was updated successfully!";
                    status = true;

                    return Ok(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
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

        #region Delete Accounting Software
        /// <summary>
        /// UC-61
        /// Delete Accounting Software with role Admin
        /// </summary>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return list if the delete is successful</response>
        /// <response code="400">If the Accounting Software name is not exist</response>
        [HttpDelete("{ASID}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccountingSoftware(string accountingSoftwareID)
        {
            string errorMessage = "";
            bool status = false;

            try
            {
                if (string.IsNullOrEmpty(accountingSoftwareID))
                {
                    errorMessage = "Accounting Software id is required!";

                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
                }

                AccountingSoftware accountingSoftware = new()
                {
                    IdAccountingSoftware = accountingSoftwareID,
                    Status = 3
                };

                bool checkDeleteAccountingSoftware = await _accountingSoftwareService.ActionAccountingSoftware(accountingSoftware, "EditStatus");

                if (checkDeleteAccountingSoftware == true)
                {
                    errorMessage = "The Accounting Software was deleted successfully!";
                    status = true;

                    return Ok(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
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

        #region Get Accounting Software details
        /// <summary>
        /// UC-56
        /// Get Accounting Software details
        /// </summary>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return detail screen if the access is successful</response>
        /// <response code="404">If the detail is not found</response>
        [HttpGet]
        [ProducesResponseType(typeof(AccountingSoftwareInfoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAccountingSoftwareDetail(string accountingSoftwareID)
        {
            string errorMessage = "";
            bool status = false;

            try
            {
                if (string.IsNullOrEmpty(accountingSoftwareID))
                {
                    errorMessage = "Accounting Software id is required!";

                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
                }

                AccountingSoftware accountingSoftware = new()
                {
                    IdAccountingSoftware = accountingSoftwareID
                };

                AccountingSoftwareInfoViewModel getAccountingSoftwareInfoViewModel = await _accountingSoftwareService.GetAccountingSoftwareTask(accountingSoftware, "GetByID");

                if (getAccountingSoftwareInfoViewModel != null)
                {
                    status = true;

                    return new JsonResult(new
                    {
                        AccountingSoftwareInfo = getAccountingSoftwareInfoViewModel,
                        Status = status,
                        ErrorMessage = errorMessage
                    });
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

        #region Get all list
        /// <summary>
        /// UC-26
        /// Get all list Accounting Software
        /// </summary>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return list screen if the access is successful</response>
        /// <response code="404">If the list is not found</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<AccountingSoftware>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllListAccountingSoftware(int page)
        {
            string errorMessage = "";
            bool status = false;

            try
            {
                int index = page;

                List<AccountingSoftware> result = await _accountingSoftwareService.GetAllAccountingSoftware(new AccountingSoftware(), "PagingAccount");
                int getTotal = result.Count;

                if (result.Count != 0)
                {
                    result = result.Skip((index - 1) * 5).Take(5).ToList();
                }

                int endPage = getTotal / 5;

                if (getTotal % 5 != 0)
                {
                    endPage++;
                }

                return Ok(new
                {
                    EndPage = endPage,
                    CurrentPage = index,
                    Success = true,
                    Data = result
                });
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
