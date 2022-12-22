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

		public static byte[] GetSendToSupplierBytes(SalesOrderInfo order, List<SalesOrderLineInfo> orderLineInfos, StoreInfo store) 
		{
			var report = new PushToSupplierReport();

			report.DataMember = null;
			report.DataSource = orderLineInfos;

            // Header
            Image storeLogo;

            using (var ms = new MemoryStream(store.Logo))
            {
               storeLogo = Image.FromStream(ms);
            }

			ImageSource storeLogoSource = new ImageSource(storeLogo, false);
            report.xrStoreLogo.ImageSource = storeLogoSource;

            report.xrOrderNumber.Text = order.PurchaseOrderNumber;
            report.xrDate.Text = DateTime.Now.ToString("dd-MM-yyy");
            report.xrCurrency.Text = orderLineInfos[0].CurrencyCode;
            report.xrCustomerFullName.Text = order.CustomerName;
            report.xrCustomerPhone.Text = order.CustomerPhone;
            report.xrCustomerEmail.Text = order.CustomerEmail;
			report.xrCustomerFullAddress.Text = orderLineInfos[0].ShippingAddress;
			report.xrStoreName.Text = store.StoreName;
            //report.xrStorePhone.Text = 
            //report.xrStoreEmail.Text =
            //report.xrStoreFullAddress.Text =

            // Table
            report.xrSKU.DataBindings.Add(new XRBinding("Text", null, "SupplierPartNumber"));
            report.xrName.DataBindings.Add(new XRBinding("Text", null, "ShortDescription"));
            report.xrCostPrice.DataBindings.Add(new XRBinding("Text", null, "ResellerBuyEx", "{0:n2}"));
            report.xrCostInclGST.DataBindings.Add(new XRBinding("Text", null, "ResellerBuyInc", "{0:n2}"));
            report.xrQty.DataBindings.Add(new XRBinding("Text", null, "Quantity", "{0:n2}"));
            report.xrTotal.DataBindings.Add(new XRBinding("Text", null, "SubtotalAmount", "{0:n2}"));

			// Totals
			decimal totalExlGst = orderLineInfos.Sum(x => x.ResellerBuyEx);
			decimal gstTotal = (totalExlGst * Convert.ToDecimal(1.10)) - totalExlGst;
            decimal orderTotal = totalExlGst + gstTotal;

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