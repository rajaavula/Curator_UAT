using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;
using DevExpress.XtraReports.UI;
using LeadingEdge.Curator.Core.Reports;
using DevExpress.XtraPrinting.Drawing;
using System.Linq;

namespace LeadingEdge.Curator.Core
{
	public static class ReportGenerator
	{
		public static byte[] GetReportBytes(XtraReport report, string type)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				switch (type)
				{
					case "Xlsx":
						report.ExportToXlsx(stream);
						break;

					case "Image":
						report.ExportToImage(stream, ImageFormat.Png);
						break;

					case "Docx":
						report.ExportToRtf(stream);
						break;

					default:
						report.ExportToPdf(stream);
						break;
				}

				stream.Position = 0;
				return stream.ToArray();
			}
		}

		public static byte[] GetSendToSupplierBytes(SalesOrderInfo order, List<SalesOrderLineInfo> lines, StoreInfo store) 
		{
			var report = new PushToSupplierReport();

            report.DataMember = null;
			report.DataSource = lines;

            // Header
            if (store.Logo != null) 
			{
                Image storeLogo;
                using (var ms = new MemoryStream(store.Logo))
                {
                    storeLogo = Image.FromStream(ms);
                }

                ImageSource storeLogoSource = new ImageSource(storeLogo, false);
                report.xrStoreLogo.ImageSource = storeLogoSource;
            }

            var customer = new List<string>
            {
                order.CustomerName,
                order.CustomerPhone,
                order.CustomerEmail,
                lines[0].ShippingAddress
            };

            customer.RemoveAll(string.IsNullOrWhiteSpace);

			report.xrOrderNumber.Text = order.PurchaseOrderNumber;
            report.xrDate.Text = DateTime.Now.ToString("dd-MM-yyy");
            report.xrCurrency.Text = lines[0].CurrencyCode;
            report.xrCustomer.Text = string.Join(Environment.NewLine, customer);
			report.xrStoreName.Text = store.StoreName;
			report.xrDeliveryInstructions.Text = lines[0].DeliveryInstructions;
			//report.xrStorePhone.Text = 
			//report.xrStoreEmail.Text =
			//report.xrStoreFullAddress.Text =

            // Table
            report.xrSKU.DataBindings.Add(new XRBinding("Text", null, "SupplierPartNumber"));
            report.xrName.DataBindings.Add(new XRBinding("Text", null, "ShortDescription"));
            report.xrCostPrice.DataBindings.Add(new XRBinding("Text", null, "PurchaseCostEx", "{0:n2}"));
            report.xrCostInclGST.DataBindings.Add(new XRBinding("Text", null, "PurchaseCostInc", "{0:n2}"));
            report.xrQty.DataBindings.Add(new XRBinding("Text", null, "Quantity"));
            report.xrTotal.DataBindings.Add(new XRBinding("Text", null, "TotalPurchaseCostEx", "{0:n2}"));

			// Totals
			var totalExlGst = lines.Sum(x => x.TotalPurchaseCostEx);
			var totalFreightLines = lines.Where(x => x.FreightLine == true).Sum(x => x.TotalPurchaseCostEx);
			var totalSupplierLines = lines.Where(x => x.FreightLine == false).Sum(x => x.TotalPurchaseCostEx);
            var gstTotal = (totalSupplierLines * Convert.ToDecimal(1.10)) + totalFreightLines - totalExlGst;
            var orderTotal = totalExlGst + gstTotal;

            report.xrTotalExlGst.Text = string.Format("{0:N2}", totalExlGst);
			report.xrGST.Text = string.Format("{0:N2}", gstTotal);
			report.xrOrderTotal.Text = string.Format("{0:N2}", orderTotal);

            report.CreateDocument();

			using (MemoryStream ms = new MemoryStream())
			{
				report.ExportToPdf(ms);
				return ms.ToArray();
			}
		}
	}
}