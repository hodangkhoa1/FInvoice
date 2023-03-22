using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FinvoiceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class XMLUserController : ControllerBase
    {
        private readonly IInvoiceFormService _invoiceFormService;
        private readonly IInvoiceService _invoiceService;
        private readonly IBuyerService _buyerService;
        private readonly ISellerService _sellerService;
        private readonly IItemService _itemService;

        public XMLUserController(IInvoiceFormService invoiceFormService, IInvoiceService invoiceService, IBuyerService buyerService, ISellerService sellerService, IItemService itemService)
        {
            _invoiceFormService = invoiceFormService;
            _invoiceService = invoiceService;
            _buyerService = buyerService;
            _sellerService = sellerService;
            _itemService = itemService;
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
        public async Task<IActionResult> ImportXML([FromBody] InvoiceViewModel importFileXML)
        {
            string errorMessage = "";
            bool status = false;

            try
            {
                if (importFileXML == null)
                {
                    throw new Exception("Api's information has been corrupted, please try again or contact developer for more support");
                }

                InvoiceForm invoiceForm = new()
                {
                    IdInvoiceForm = importFileXML.invoiceForm.IdInvoiceForm,
                    CodeForm = importFileXML.invoiceForm.CodeForm,
                    NameInvoiceType = importFileXML.invoiceForm.NameInvoiceType,
                    Status = importFileXML.invoiceForm.Status
                };

                Invoice invoice = new()
                {
                    IdInvoice = importFileXML.IdInvoice,
                    Series = importFileXML.Series,
                    InvoiceNo = importFileXML.InvoiceNo,
                    Title = importFileXML.Title,
                    Date = importFileXML.Date,
                    PaymentMethod = importFileXML.PaymentMethod,
                    TaxtRate = importFileXML.TaxtRate,
                    VatAmount = importFileXML.VatAmount,
                    TotalPayment = importFileXML.TotalPayment,
                    ImportedDate = importFileXML.ImportedDate,
                    Source = importFileXML.Source,
                    SubTotal = importFileXML.SubTotal,
                    Status = 1,
                    IdAccount = importFileXML.IdAccount,
                    IdInvoiceForm = importFileXML.IdInvoiceForm
                };

                Buyer buyer = new()
                {
                    IdBuyer = importFileXML.buyerInvoice.IdBuyer,
                    Name = importFileXML.buyerInvoice.Name,
                    Companyname = importFileXML.buyerInvoice.Companyname,
                    TaxCode = importFileXML.buyerInvoice.TaxCode,
                    Address = importFileXML.buyerInvoice.Address,
                    AccountBanking = importFileXML.buyerInvoice.AccountBanking,
                    BankingName = importFileXML.buyerInvoice.BankingName
                };

                Seller seller = new()
                {
                    IdSeller = importFileXML.sellerInvoice.IdSeller,
                    Name = importFileXML.sellerInvoice.Name,
                    Phone = importFileXML.sellerInvoice.Phone,
                    TaxCode = importFileXML.sellerInvoice.TaxCode,
                    Address = importFileXML.sellerInvoice.Address,
                    Email = importFileXML.sellerInvoice.Email,
                    AccountBanking = importFileXML.sellerInvoice.AccountBanking,
                    BankingName = importFileXML.sellerInvoice.BankingName,
                };

                bool checkAddInvoiceForm = await _invoiceFormService.ActionInvoiceForm(invoiceForm, "AddInvoiceForm");

                if (checkAddInvoiceForm == true)
                {
                    bool checkAddInvoice = await _invoiceService.ActionInvoice(invoice, "AddInvoice");

                    if (checkAddInvoice == true)
                    {
                        await _buyerService.ActionBuyer(buyer, "AddBuyer");
                        await _sellerService.ActionSeller(seller, "AddSeller");

                        for (int i = 0; i < importFileXML.itemInvoiceList.Count; i++)
                        {
                            Item item = new()
                            {
                                IdItem = importFileXML.itemInvoiceList[i].IdItem,
                                Name = importFileXML.itemInvoiceList[i].Name,
                                Unit = importFileXML.itemInvoiceList[i].Unit,
                                Quantity = importFileXML.itemInvoiceList[i].Quantity,
                                UnitPrice = importFileXML.itemInvoiceList[i].UnitPrice,
                                Amount = importFileXML.itemInvoiceList[i].Amount,
                                IdInvoice = importFileXML.itemInvoiceList[i].IdInvoice,
                            };

                            await _itemService.ActionItem(item, "AddItem");
                        }
                    }
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

        #region Get XML Import
        /// <summary>
        /// UC-9
        /// Get XML Import
        /// </summary>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return list import XML screen if the access is successful</response>
        /// <response code="404">If the profile is not found</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetXMLImport(string userID)
        {
            string errorMessage = "";
            bool status = false;
            var result = new List<string>();

            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = "Something went wrong!"
                    });
                }

                Invoice invoice = new()
                {
                    IdAccount = userID
                };

                List<Invoice> invoiceList = await _invoiceService.GetInvoiceList(invoice, "GetAllInvoiceByUserID");

                if (invoiceList.Count == 0)
                {
                    errorMessage = "List invoice is empty!";
                }
                else
                {
                    for (int i = 0; i < invoiceList.Count; i++)
                    {
                        result.Add(invoiceList[i].Title);
                    }
                }

                status = true;
            }
            catch (Exception)
            {
                errorMessage = "Something went wrong!";
            }

            return new JsonResult(new
            {
                Data = result,
                Status = status,
                ErrorMessage = errorMessage
            });
        }
        #endregion
    }
}
