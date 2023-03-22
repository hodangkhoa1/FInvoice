using FinvoiceWeb.Models;
using System.Xml;

namespace FinvoiceWeb.Utils
{
    public class FunctionGetValueXML
    {
        public static Invoice GetEasyinvoiceValueXML(string XMLPath, UserInfo userInfo)
        {
            string idInvoiceForm = FunctionRandom.RandomCode(21);
            XmlDocument xmlDocument = new();
            xmlDocument.Load(XMLPath);
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//Invoice/Content");
            
            Invoice invoice = new()
            {
                IdAccount = userInfo.IdAccount,
                IdInvoiceForm = idInvoiceForm
            };
            SellerInvoice sellerInvoice = new();
            InvoiceForm invoiceForm = new()
            {
                IdInvoiceForm = idInvoiceForm
            };
            BuyerInvoice buyerInvoice = new()
            {
                Name = "Chưa có"
            };
            List<ItemInvoice> itemInvoiceList = new();

            foreach (XmlNode node in xmlNodeList)
            {
                XmlNode Key = node.SelectSingleNode("Key");
                if (Key != null)
                {
                    invoice.IdInvoice = Key.InnerText;
                    sellerInvoice.IdSeller = Key.InnerText;
                    buyerInvoice.IdBuyer = Key.InnerText;
                }

                XmlNode ArisingDate = node.SelectSingleNode("ArisingDate");
                if (ArisingDate != null)
                {
                    invoice.Date = DateTime.Parse(ArisingDate.InnerText);
                }

                XmlNode ComFax = node.SelectSingleNode("ComFax");
                if (ComFax != null)
                {

                }

                XmlNode ComName = node.SelectSingleNode("ComName");
                if (ComName != null)
                {
                    sellerInvoice.Name = ComName.InnerText;
                }

                XmlNode ComTaxCode = node.SelectSingleNode("ComTaxCode");
                if (ComTaxCode != null)
                {
                    sellerInvoice.TaxCode = ComTaxCode.InnerText;
                }

                XmlNode ComAddress = node.SelectSingleNode("ComAddress");
                if (ComAddress != null)
                {
                    sellerInvoice.Address = ComAddress.InnerText;
                }

                XmlNode ComPhone = node.SelectSingleNode("ComPhone");
                if (ComPhone != null)
                {
                    sellerInvoice.Phone = ComPhone.InnerText;
                }

                XmlNode ComEmail = node.SelectSingleNode("ComEmail");
                if (ComEmail != null)
                {
                    sellerInvoice.Email = ComEmail.InnerText;
                }

                XmlNode ComBankNo = node.SelectSingleNode("ComBankNo");
                if (ComBankNo != null)
                {
                    sellerInvoice.AccountBanking = ComBankNo.InnerText;
                }

                XmlNode ComBankName = node.SelectSingleNode("ComBankName");
                if (ComBankName != null)
                {
                    sellerInvoice.BankingName = ComBankName.InnerText;
                }

                XmlNode Ikey = node.SelectSingleNode("Ikey");
                if (Ikey != null)
                {

                }

                XmlNode ParentName = node.SelectSingleNode("ParentName");
                if (ParentName != null)
                {

                }

                XmlNode InvoiceName = node.SelectSingleNode("InvoiceName");
                if (InvoiceName != null)
                {
                    invoiceForm.NameInvoiceType = InvoiceName.InnerText;
                }

                XmlNode InvoicePattern = node.SelectSingleNode("InvoicePattern");
                if (InvoicePattern != null)
                {
                    invoiceForm.CodeForm = InvoicePattern.InnerText;
                }

                XmlNode SerialNo = node.SelectSingleNode("SerialNo");
                if (SerialNo != null)
                {
                    invoice.Series = SerialNo.InnerText;
                }

                XmlNode InvoiceNo = node.SelectSingleNode("InvoiceNo");
                if (InvoiceNo != null)
                {
                    invoice.InvoiceNo = InvoiceNo.InnerText;
                }

                XmlNode PaymentMethod = node.SelectSingleNode("PaymentMethod");
                if (PaymentMethod != null)
                {
                    invoice.PaymentMethod = PaymentMethod.InnerText;
                }

                XmlNode CusCode = node.SelectSingleNode("CusCode");
                if (CusCode != null)
                {
                    
                }

                XmlNode CusName = node.SelectSingleNode("CusName");
                if (CusName != null)
                {
                    buyerInvoice.Companyname = CusName.InnerText;
                }

                XmlNode CusTaxCode = node.SelectSingleNode("CusTaxCode");
                if (CusTaxCode != null)
                {
                    buyerInvoice.TaxCode = CusTaxCode.InnerText;
                }

                XmlNode CusPhone = node.SelectSingleNode("CusPhone");
                if (CusPhone != null)
                {

                }

                XmlNode CusAddress = node.SelectSingleNode("CusAddress");
                if (CusAddress != null)
                {
                    buyerInvoice.Address = CusAddress.InnerText;
                }

                XmlNode CusBankName = node.SelectSingleNode("CusBankName");
                if (CusBankName != null)
                {
                    buyerInvoice.BankingName = CusBankName.InnerText;
                }

                XmlNode CusBankNo = node.SelectSingleNode("CusBankNo");
                if (CusBankNo != null)
                {
                    buyerInvoice.AccountBanking = CusBankNo.InnerText;
                }

                XmlNode Total = node.SelectSingleNode("Total");
                if (Total != null)
                {
                    invoice.SubTotal = decimal.Parse(Total.InnerText);
                }

                XmlNode VATAmount = node.SelectSingleNode("VATAmount");
                if (VATAmount != null)
                {
                    invoice.VatAmount = decimal.Parse(VATAmount.InnerText);
                }

                XmlNode Amount = node.SelectSingleNode("Amount");
                if (Amount != null)
                {
                    invoice.TotalPayment = decimal.Parse(Amount.InnerText);
                }

                XmlNode AmountInWords = node.SelectSingleNode("AmountInWords");
                if (AmountInWords != null)
                {

                }

                XmlNode Buyer = node.SelectSingleNode("Buyer");
                if (Buyer != null)
                {

                }

                XmlNode VATRate = node.SelectSingleNode("VATRate");
                if (VATRate != null)
                {
                    invoice.TaxtRate = float.Parse(VATRate.InnerText);
                }

                XmlNode Note = node.SelectSingleNode("Note");
                if (Note != null)
                {

                }

                XmlNode CusEmails = node.SelectSingleNode("CusEmails");
                if (CusEmails != null)
                {

                }

                XmlNode amountInWordsL2 = node.SelectSingleNode("amountInWordsL2");
                if (amountInWordsL2 != null)
                {

                }

                XmlNodeList productList = node.SelectNodes("//Products/Product");
                foreach (XmlNode productNode in productList)
                {
                    ItemInvoice itemInvoice = new()
                    {
                        IdInvoice = invoice.IdInvoice
                    };

                    XmlNode Code = productNode.SelectSingleNode("Code");
                    if (Code != null)
                    {
                        itemInvoice.IdItem = Code.InnerText;
                    }
                    else
                    {
                        itemInvoice.IdItem = FunctionRandom.RandomCode(23);
                    }

                    XmlNode ProdName = productNode.SelectSingleNode("ProdName");
                    if (ProdName != null)
                    {
                        itemInvoice.Name = ProdName.InnerText;
                    }

                    XmlNode ProdPrice = productNode.SelectSingleNode("ProdPrice");
                    if (ProdPrice != null)
                    {
                        itemInvoice.UnitPrice = decimal.Parse(ProdPrice.InnerText);
                    }

                    XmlNode ProdQuantity = productNode.SelectSingleNode("ProdQuantity");
                    if (ProdQuantity != null)
                    {
                        itemInvoice.Quantity = int.Parse(ProdQuantity.InnerText);
                    }

                    XmlNode ProdType = productNode.SelectSingleNode("ProdType");
                    if (ProdType != null)
                    {

                    }

                    XmlNode ProdUnit = productNode.SelectSingleNode("ProdUnit");
                    if (ProdUnit != null)
                    {
                        itemInvoice.Unit = ProdUnit.InnerText;
                    }

                    XmlNode Pos = productNode.SelectSingleNode("Pos");
                    if (Pos != null)
                    {

                    }

                    XmlNode TotalItem = productNode.SelectSingleNode("Total");
                    if (TotalItem != null)
                    {

                    }

                    XmlNode AmountItem = productNode.SelectSingleNode("Amount");
                    if (AmountItem != null)
                    {
                        itemInvoice.Amount = decimal.Parse(AmountItem.InnerText);
                    }

                    itemInvoiceList.Add(itemInvoice);
                }
            }

            invoice.sellerInvoice = sellerInvoice;
            invoice.invoiceForm = invoiceForm;
            invoice.itemInvoiceList = itemInvoiceList;
            invoice.buyerInvoice = buyerInvoice;

            return invoice;
        }
    }
}
