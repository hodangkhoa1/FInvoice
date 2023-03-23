using BAL.Models;
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
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        #region Add Suppliers
        /// <summary>
        /// UC-54
        /// Add Suppliers with role admin
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
        /// <response code="400">If the Supplier name is exist</response>
        [HttpPost()]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSupplier([FromForm] SupplierViewModel supplierViewModel)
        {
            string errorMessage = "", supplierID = FunctionRandom.RandomCode(21);
            bool status = false;
            SupplierValidator supplierValidator = new();

            try
            {
                if (supplierViewModel == null)
                {
                    throw new Exception("Api's information has been corrupted, please try again or contact developer for more support");
                }

                var validation = supplierValidator.Validate(supplierViewModel);

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

                Supplier supplier = new()
                {
                    IdSupplier = supplierID,
                    Name = supplierViewModel.Name,
                    Logo = supplierViewModel.Logo,
                    ColorLogo = FunctionRandom.ColorAvatar(),
                    DefaultLogo = supplierViewModel.Name.FirstOrDefault(),
                    Status = 1
                };

                bool checkAddSupplier = await _supplierService.ActionSupplier(supplier, "AddSupplier");

                if (checkAddSupplier == true)
                {
                    errorMessage = "Add supplier successfully!";
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

        #region Update Suppliers
        /// <summary>
        /// UC-52
        /// Update Supplier with role Admin
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///
        ///         {
        ///           "IdSupplier": "123",
        ///           "Name": "Test",
        ///           "Logo": "Import image"
        ///         }
        ///         
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return list if the update is successful</response>
        /// <response code="400">If the suppliers name is exist</response>
        [HttpPost()]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSupplier([FromForm] EditSupplierViewModel editSupplierViewModel)
        {
            string errorMessage = "";
            bool status = false;
            EditSupplierValidator editSupplierValidator = new();

            try
            {
                if (editSupplierViewModel == null)
                {
                    throw new Exception("Api's information has been corrupted, please try again or contact developer for more support");
                }

                var validation = editSupplierValidator.Validate(editSupplierViewModel);

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

                Supplier supplier = new()
                {
                    IdSupplier = editSupplierViewModel.IdSupplier,
                    Name = editSupplierViewModel.Name,
                    Logo = editSupplierViewModel.Logo
                };

                bool existingSupplier = await _supplierService.ActionSupplier(supplier, "EditSupplier");

                if (existingSupplier == true)
                {
                    errorMessage = "Supplier was updated successfully!";
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

        #region Edit Status Suppliers
        /// <summary>
        /// UC-50
        /// Edit status Suppliers with role admin
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///
        ///         {
        ///           "IdSupplier": "123",
        ///         }
        ///         
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return list if the update is successful</response>
        /// <response code="400">If the Suppliers ID is not exist</response>
        [HttpGet("{SID}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditStatusSupplier(string supplierID, string action)
        {
            string errorMessage = "";
            bool status = false;

            try
            {
                if (string.IsNullOrEmpty(supplierID))
                {
                    errorMessage = "Supplier id is required!";

                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
                }

                Supplier supplier = new()
                {
                    IdSupplier = supplierID,
                    Status = action.Equals("Active") ? 1 : 2
                };

                bool checkUpdateSupplier = await _supplierService.ActionSupplier(supplier, "EditStatus");

                if (checkUpdateSupplier == true)
                {
                    errorMessage = "The supplier was updated successfully!";
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

        #region Delete Suppliers
        /// <summary>
        /// UC-53
        /// Delete Suppliers with role Admin
        /// </summary>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return list if the delete is successful</response>
        /// <response code="400">If the Supplier name is not exist</response>
        [HttpDelete("{SID}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSupplier(string supplierID)
        {
            string errorMessage = "";
            bool status = false;

            try
            {
                if (string.IsNullOrEmpty(supplierID))
                {
                    errorMessage = "Supplier id is required!";

                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
                }

                Supplier supplier = new()
                {
                    IdSupplier = supplierID,
                    Status = 3
                };

                bool checkDeleteSupplier = await _supplierService.ActionSupplier(supplier, "EditStatus");

                if (checkDeleteSupplier == true)
                {
                    errorMessage = "The Supplier was deleted successfully!";
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

        #region Get Supplier details
        /// <summary>
        /// UC-48
        /// Get Supplier details
        /// </summary>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return detail screen if the access is successful</response>
        /// <response code="404">If the detail is not found</response>
        [HttpGet]
        [ProducesResponseType(typeof(SupplierInfoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSupplierDetail(string supplierID)
        {
            string errorMessage = "";
            bool status = false;

            try
            {
                if (string.IsNullOrEmpty(supplierID))
                {
                    errorMessage = "Supplier id is required!";

                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
                }

                Supplier supplier = new()
                {
                    IdSupplier = supplierID
                };

                SupplierInfoViewModel getSupplierInfoViewModel = await _supplierService.GetSupplierTask(supplier, "GetByID");

                if (getSupplierInfoViewModel != null)
                {
                    status = true;

                    return new JsonResult(new
                    {
                        SupplierInfo = getSupplierInfoViewModel,
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
        /// Get all list Supplier
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

                List<Supplier> result = await _supplierService.GetAllSupplier(new Supplier(), "PagingAccount");
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
