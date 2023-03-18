using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using BAL.Utils;
using BAL.Validators;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinvoiceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class XMLTemplateController : ControllerBase
    {
        private readonly IInvoiceFormService _invoiceFormService;

        public XMLTemplateController(IInvoiceFormService invoiceFormService)
        {
            _invoiceFormService = invoiceFormService;
        }

        #region Add XML Template
        /// <summary>
        /// UC-43
        /// Add XML Template with role admin
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///
        ///         {
        ///           "CodeForm": "01GTKT0/001",
        ///           "NameInvoiceType": "Hóa đơn giá trị gia tăng",
        ///         }
        ///         
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return list if the add is successful</response>
        /// <response code="400">If the xml template is exist</response>
        [HttpPost()]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddXMLTemplate([FromBody] InvoiceFormViewModel invoiceFormViewModel)
        {
            string errorMessage = "", invoiceFormID = FunctionRandom.RandomCode(21);
            bool status = false;
            InvoiceFormValidator invoiceFormValidator = new();

            try
            {
                if (invoiceFormViewModel == null)
                {
                    throw new Exception("Api's information has been corrupted, please try again or contact developer for more support");
                }

                var validation = invoiceFormValidator.Validate(invoiceFormViewModel);

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

                InvoiceForm invoiceForm = new()
                {
                    IdInvoiceForm = invoiceFormID,
                    CodeForm = invoiceFormViewModel.CodeForm,
                    NameInvoiceType = invoiceFormViewModel.NameInvoiceType,
                    Status = 1
                };

                bool checkAddInvoiceForm = await _invoiceFormService.ActionInvoiceForm(invoiceForm, "AddInvoiceForm");

                if (checkAddInvoiceForm == true)
                {
                    errorMessage = "Add invoice form successfully!";
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

        #region Update XML Template
        /// <summary>
        /// UC-41
        /// Update XML Template with role Admin
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///
        ///         {
        ///           "IdInvoiceForm": "123",
        ///           "CodeForm": "01GTKT0/001",
        ///           "NameInvoiceType": "Hóa đơn giá trị gia tăng",
        ///         }
        ///         
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return list if the update is successful</response>
        /// <response code="400">If the XML Template code is exist</response>
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

                /*bool existingSupplier = await _supplierService.ActionSupplier(supplier, "EditSupplier");

                if (existingSupplier == true)
                {
                    errorMessage = "Supplier was updated successfully!";
                    status = true;

                    return Ok(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
                }*/
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
