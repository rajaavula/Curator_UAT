using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Drawing;
using System.Xml;
using Cortex.Utilities;
using OfficeOpenXml;
using Image = System.Drawing.Image;
using DevExpress.Web.Mvc;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using GoogleResult = LeadingEdge.Curator.Core.GoogleResult;

namespace LeadingEdge.Curator.Core
{
    public static class U
    {
        public static string GetModelID(string modelname, string pageID)
        {
            return String.Format("{0}_{1}", modelname, pageID);
        }

        public static byte[] CreateZipArchive(Dictionary<string, byte[]> files)
        {
            byte[] zipBytes = null;

            try
            {
                using (var compressedFileStream = new MemoryStream())
                {
                    using (ZipArchive zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
                    {
                        foreach (var file in files)
                        {
                            var zipEntry = zipArchive.CreateEntry(file.Key);

                            using (var originalFileStream = new MemoryStream(file.Value))
                            {
                                using (var zipEntryStream = zipEntry.Open())
                                {
                                    originalFileStream.CopyTo(zipEntryStream);
                                }
                            }
                        }
                    }

                    zipBytes = compressedFileStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return zipBytes;
        }

        public static bool HasPermission(SessionInfo s, string code)
        {
            if (s == null || s.UserGroup == null) return false;

            if (s.UserGroup.UserGroupID == 0) return false;

            return s.UserGroupPermissions != null && s.UserGroupPermissions.Exists(x => x.Code == code);
        }
        public static bool HasPermission(List<UserGroupPermissionInfo> s, string code)
        {
            return s != null && s.Exists(x => x.Code == code);
        }

        public static string RenderPartialToString(string partial)
        {
            return RenderPartialToString(partial, new object());
        }

        // Usage: string ret = RenderPartialToString("~/Area/Views/MyController/MyPartial.ascx", model);
        public static string RenderPartialToString(string partial, object model)
        {
            {
                ViewPage viewPage = new ViewPage { ViewContext = new ViewContext() };

                viewPage.ViewData = new ViewDataDictionary(model);
                viewPage.Controls.Add(viewPage.LoadControl(partial));

                StringBuilder sb = new StringBuilder();

                using (StringWriter sw = new StringWriter(sb))
                {
                    using (HtmlTextWriter tw = new HtmlTextWriter(sw))
                    {
                        viewPage.RenderControl(tw);
                    }
                }

                return sb.ToString();
            }
        }

        public static void CopyPropertyValues(object original, object current)
        {
            if (original == null) throw new ArgumentNullException("original");
            if (current == null) throw new ArgumentNullException("current");

            try
            {
                Type type = current.GetType();

                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo pi in properties)
                {
                    if (!pi.PropertyType.IsValueType && pi.PropertyType != typeof(string)) continue;

                    if (pi.SetMethod == null || !pi.SetMethod.IsPublic) continue;

                    if (String.IsNullOrEmpty(pi.Name)) continue;

                    object oldValue = type.GetProperty(pi.Name).GetValue(original, null);

                    type.GetProperty(pi.Name).SetValue(current, oldValue);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public static string FormatValue(object value)
        {
            if (value == null) return String.Empty;

            string format = "{0}";

            Type t = value.GetType();

            if (t == typeof(DateTime))
            {
                format = "{0:dd/MM/yyyy HH:mm}";
            }

            else if (new List<Type> { typeof(decimal), typeof(float), typeof(long), typeof(double) }.Contains(t))
            {
                format = "{0:n2}";
            }

            else if (t == typeof(int))
            {
                format = "{0:n0}";
            }

            string result = String.Format(format, value);

            if (String.IsNullOrEmpty(result) == false)
            {
                result = result.ToUpper();
            }

            return result;
        }

        public static byte[] ResizeImage(byte[] bytes, Size size)
        {
            if (bytes == null) return null;

            byte[] newBytes = null;

            try
            {
                using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
                {
                    ms.Write(bytes, 0, bytes.Length);
                    Image image = Image.FromStream(ms, true);

                    int sourceWidth = image.Width;
                    int sourceHeight = image.Height;

                    float nPercent = 0;
                    float nPercentW = (size.Width / (float)sourceWidth);
                    float nPercentH = (size.Height / (float)sourceHeight);

                    nPercent = (nPercentH < nPercentW) ? nPercentH : nPercentW;

                    int destWidth = (int)(sourceWidth * nPercent);
                    int destHeight = (int)(sourceHeight * nPercent);

                    using (Bitmap thumb = new Bitmap(destWidth, destHeight))
                    {
                        Graphics g = Graphics.FromImage(thumb);
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(image, 0, 0, destWidth, destHeight);
                        g.Dispose();

                        MemoryStream ms2 = new MemoryStream();
                        thumb.Save(ms2, ImageFormat.Jpeg);
                        thumb.Dispose();
                        image.Dispose();
                        newBytes = (byte[])ms2.ToArray().Clone();
                        ms2.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return newBytes;
        }

        public static string FormatAddressList(List<string> addressList)
        {
            try
            {
                if (addressList == null || addressList.Count < 1) return null;

                return String.Join(";", addressList.ToArray());
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return null;
        }

        public static string[] SplitAddressList(string emails)
        {
            return String.IsNullOrEmpty(emails) ? new string[] { } : emails.Replace(" ", "").Split(';');
        }

        public static string GetDocumentContentType(string filename)
        {
            if (String.IsNullOrEmpty(filename)) return null;

            string ext = Path.GetExtension(filename);

            if (String.IsNullOrEmpty(ext)) return null;

            switch (ext)
            {
                case ".pdf": return "application/pdf";
                case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".xls": return "application/vnd.ms-excel";
                case ".doc": return "application/msword";
                case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".ppt": return "application/vnd.ms-powerpoint";
                case ".pptx": return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".rtf": return "text/rtf";
                case ".txt": return "text/plain";
                case ".xml": return "text/xml";
                case ".png": return "image/png";
                case ".gif": return "image/gif";
                case ".jpeg":
                case ".jpg": return "image/jpg";
            }

            return null;
        }

        public static string GetDocumentType(string filename)
        {
            if (String.IsNullOrEmpty(filename)) return null;

            string ext = Path.GetExtension(filename);

            if (String.IsNullOrEmpty(ext)) return null;

            switch (ext)
            {
                case ".pdf": return "PDF Document";
                case ".xls":
                case ".xlsx": return "MS Excel Document";
                case ".doc":
                case ".docx": return "MS Word Document";
                case ".rtf": return "Rich Text Document";
                case ".txt": return "Text Document";
                case ".png":
                case ".gif":
                case ".jpeg":
                case ".jpg": return "Image";
                case ".ppt":
                case ".pptx": return "MS Powerpoint Document";
                case ".xml": return "XML Document";
            }

            return "Document";
        }

        public static string GetDocumentTypeClass(string filename)
        {
            string documentType = GetDocumentType(filename);

            switch (documentType)
            {
                case "PDF Document": return "pdf";
                case "MS Excel Document": return "excel";
                case "MS Word Document": return "word";
                case "Rich Text Document": return "rtf";
                case "Text Document": return "txt";
            }

            return "doc";
        }

        public static string Trim(string text)
        {
            if (String.IsNullOrEmpty(text)) return null;

            return text.Trim();
        }

        public static DataTable ExcelWorksheetToDataTable(ExcelPackage excel, bool hasHeader)
        {
            DataTable dt = new DataTable();

            try
            {
                if (excel.Workbook.Worksheets.Count != 0)
                {
                    var sheet = excel.Workbook.Worksheets.First();
                    foreach (var firstRowCell in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                    {
                        dt.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                    }
                    var startRow = hasHeader ? 2 : 1;
                    for (var rowNum = startRow; rowNum <= sheet.Dimension.End.Row; rowNum++)
                    {
                        DataRow dr = dt.NewRow();

                        for (var colNum = 0; colNum < dt.Columns.Count; colNum++)
                        {
                            dr[colNum] = sheet.Cells[rowNum, colNum + 1].Value;
                        }

                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return dt;
        }

        public static void RemoveDuplicateReipients(List<string> to, List<string> cc, List<string> bcc)
        {
            // Remove duplicates
            to.RemoveAll(x => String.IsNullOrEmpty(x));
            cc.RemoveAll(x => to.Exists(y => y == x) || String.IsNullOrEmpty(x));
            bcc.RemoveAll(x => to.Exists(y => y == x) || cc.Exists(y => y == x) || String.IsNullOrEmpty(x));
        }

        public static FileContentResult OutputFileResult(byte[] bytes, string filename, string type)
        {
            switch (type)
            {
                case "Xlsx": return OutputXlsxResult(bytes, filename, false);

                case "Docx": return OutputDocxResult(bytes, filename, false);

                case "Pdf": return OutputPdfResult(bytes, filename, false);

                case "Image": return OutputImageResult(bytes, filename, false);
            }

            return null;
        }

        public static FileContentResult OutputPdfResult(byte[] bytes, string reportname, bool addtimestamp)
        {
            FileContentResult file = new FileContentResult(bytes, "application/pdf");

            string filename = Path.GetFileNameWithoutExtension(reportname);

            if (addtimestamp) filename = String.Format("{0} {1:HHmmss}", filename, DateTime.Now);

            file.FileDownloadName = String.Format("{0}.pdf", filename);

            return file;
        }

        public static FileContentResult OutputXlsxResult(byte[] bytes, string reportname, bool addtimestamp)
        {
            FileContentResult file = new FileContentResult(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            string filename = Path.GetFileNameWithoutExtension(reportname);

            if (addtimestamp) filename = String.Format("{0} {1:HHmmss}", filename, DateTime.Now);

            file.FileDownloadName = String.Format("{0}.xlsx", filename);

            return file;
        }

        public static FileContentResult OutputDocxResult(byte[] bytes, string reportname, bool addtimestamp)
        {
            FileContentResult file = new FileContentResult(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            string filename = Path.GetFileNameWithoutExtension(reportname);

            if (addtimestamp) filename = String.Format("{0} {1:HHmmss}", filename, DateTime.Now);

            file.FileDownloadName = String.Format("{0}.rtf", filename);

            return file;
        }

        public static FileContentResult OutputImageResult(byte[] bytes, string reportname, bool addtimestamp)
        {
            FileContentResult file = new FileContentResult(bytes, "image/png");

            string filename = Path.GetFileNameWithoutExtension(reportname);

            if (addtimestamp) filename = String.Format("{0} {1:HHmmss}", filename, DateTime.Now);

            file.FileDownloadName = String.Format("{0}.png", filename);

            return file;
        }

        public static void GenerateCaptchaImage(out byte[] imageBytes, out string captchaText)
        {
            captchaText = null;
            imageBytes = null;

            const int width = 95;
            const int height = 30;
            const string fontFamily = "Verdana";
            const int length = 4;
            Random random = new Random();

            string[] characterSet = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            for (int x = 0; x < length; x++)
            {
                int i = random.Next(0, characterSet.Length);
                captchaText += characterSet[i];
            }

            if (String.IsNullOrEmpty(captchaText)) return;

            // Create a new 32-bit bitmap image.
            using (Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                // Create a graphics object for drawing.
                Graphics g = Graphics.FromImage(bitmap);
                g.SmoothingMode = SmoothingMode.HighQuality;

                // AntiAlias;
                Rectangle rect = new Rectangle(0, 0, width, height);

                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    g.FillRectangle(brush, rect);
                }

                // Set up the text font.
                SizeF size;
                float fontSize = rect.Height + 1;
                Font font;

                // Adjust the font size until the text fits within the image.
                do
                {
                    fontSize--;
                    font = new Font(fontFamily, fontSize, FontStyle.Bold);
                    size = g.MeasureString(captchaText, font);
                }
                while (size.Width > rect.Width);

                font = new Font(fontFamily, fontSize + 6, FontStyle.Bold);

                // Set up the text format.
                using (StringFormat format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    // Create a path using the text and warp it randomly.
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddString(captchaText, font.FontFamily, (int)font.Style, font.Size, rect, format);

                        const float v = 4F;

                        PointF[] points =
                        {
                            new PointF(random.Next(rect.Width) / v, random.Next(rect.Height) / v),
                            new PointF(rect.Width - random.Next(rect.Width) / v, random.Next(rect.Height) / v),
                            new PointF(random.Next(rect.Width) / v, rect.Height - random.Next(rect.Height) / v),
                            new PointF(rect.Width - random.Next(rect.Width) / v, rect.Height - random.Next(rect.Height) / v)
                        };

                        using (Matrix matrix = new Matrix())
                        {
                            matrix.Translate(0F, 0F);
                            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);
                        }

                        // Draw the text.
                        using (HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.White, ColorTranslator.FromHtml("#4964be")))
                        {
                            g.FillPath(hatchBrush, path);
                        }

                        font.Dispose();
                    }
                }

                g.Dispose();

                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Gif);
                    imageBytes = ms.ToArray();
                }
            }
        }

        public static string Encrypt(string value)
        {
            TripleDES t = new TripleDES();
            return t.Encrypt(value);
        }

        public static string Decrypt(string value)
        {
            TripleDES t = new TripleDES();
            return t.Decrypt(value);
        }

        public static void Compress(DirectoryInfo directorySelected, string directoryPath)
        {
            foreach (FileInfo fileToCompress in directorySelected.GetFiles())
            {
                using (FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if ((File.GetAttributes(fileToCompress.FullName) &
                       FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                    {
                        using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                               CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);

                            }
                        }
                        FileInfo info = new FileInfo(directoryPath + "\\" + fileToCompress.Name + ".gz");
                        Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                        fileToCompress.Name, fileToCompress.Length.ToString(), info.Length.ToString());
                    }

                }
            }
        }

        public static class CSVExporter
        {
            public static void Convert(StreamWriter sw, DataTable data)
            {
                if (data == null) return;

                for (int i = 0; i < data.Columns.Count; i++)
                {
                    sw.Write(data.Columns[i].ColumnName);

                    if (i < (data.Columns.Count - 1))
                    {
                        sw.Write(",");
                        continue;
                    }

                    sw.Write("\r\n");
                }

                foreach (DataRow dr in data.Rows)
                {
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        object value = dr[i];
                        string output = null;

                        if (value == null || (value is INullable && ((INullable)value).IsNull))
                        {
                            output = String.Empty;
                        }
                        else if (value is DateTime)
                        {
                            if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                            {
                                output = ((DateTime)value).ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                output = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                        else
                        {
                            output = System.Convert.ToString(value);
                        }

                        if (output.Contains(",") || output.Contains("\"") || output.Contains("\n"))
                        {
                            output = '"' + output.Replace("\"", "\"\"") + '"';
                        }

                        sw.Write(output);

                        if (i < (data.Columns.Count - 1))
                        {
                            sw.Write(",");
                            continue;
                        }

                        sw.Write("\r\n");
                    }
                }
            }

            public static void Convert(StreamWriter sw, ICollection data)
            {
                if (data == null) return;

                Type[] types = data.GetType().GetGenericArguments();
                Type type = types[0];

                PropertyInfo[] props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                for (int i = 0; i < props.Length; i++)
                {
                    sw.Write(props[i].Name);

                    if (i < (props.Length - 1))
                    {
                        sw.Write(",");
                        continue;
                    }

                    sw.Write("\r\n");
                }

                foreach (var item in data)
                {
                    for (int i = 0; i < props.Length; i++)
                    {
                        object value = props[i].GetValue(item, null);
                        string output = null;

                        if (value == null || (value is INullable && ((INullable)value).IsNull))
                        {
                            output = String.Empty;
                        }
                        else if (value is DateTime)
                        {
                            if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                            {
                                output = ((DateTime)value).ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                output = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                        else
                        {
                            output = System.Convert.ToString(value);
                        }

                        if (output.Contains(",") || output.Contains("\"") || output.Contains("\n"))
                        {
                            output = '"' + output.Replace("\"", "\"\"") + '"';
                        }

                        sw.Write(output);

                        if (i < (props.Length - 1))
                        {
                            sw.Write(",");
                            continue;
                        }

                        sw.Write("\r\n");
                    }
                }
            }
        }

        public static DataTable XmlToDataTable(string xml, string recordRootName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            DataTable dt = new DataTable();

            XmlNodeList records = doc.GetElementsByTagName(recordRootName);

            foreach (XmlNode record in records)
            {
                if (record.NodeType != XmlNodeType.Element) continue;

                if (record.HasChildNodes == false) continue;

                XmlNodeList nodes = record.ChildNodes;

                DataRow drRow = dt.NewRow();

                TraverseNodes(nodes, drRow, dt.Columns, recordRootName);

                dt.Rows.Add(drRow);
            }

            return dt;
        }

        private static void TraverseNodes(XmlNodeList nodes, DataRow row, DataColumnCollection columns, string rootNode)
        {
            foreach (XmlNode node in nodes)
            {
                string columnName = node.Name;

                if (node.NodeType == XmlNodeType.Element)
                {
                    if (node.HasChildNodes)
                    {
                        TraverseNodes(node.ChildNodes, row, columns, rootNode);
                        continue;
                    }
                }
                else
                {
                    XmlNode parentNode = node.ParentNode;
                    if (parentNode == null || parentNode.NodeType != XmlNodeType.Element) continue;

                    XmlNode grandParentNode = parentNode.ParentNode;
                    if (grandParentNode == null || grandParentNode.NodeType != XmlNodeType.Element) continue;

                    columnName = parentNode.Name;

                    while (grandParentNode != null && grandParentNode.Name != rootNode)
                    {
                        columnName = String.Format("{0} {1}", grandParentNode.Name, columnName);
                        grandParentNode = grandParentNode.ParentNode;
                    }
                }

                if (columns.Contains(columnName) == false)
                {
                    columns.Add(columnName);
                }

                row[columnName] = Utils.ToDBValue(node.InnerText);
            }
        }

        public static double GetIntFromLatLong(string Lat)
        {
            double number;
            double.TryParse(Lat, out number);
            return number;
        }

        public static DateTime AddWorkingDays(DateTime date, int days)
        {
            int increment = days < 0 ? -1 : 1;
            DateTime result = date;

            while (days != 0)
            {
                result = result.AddDays(increment);

                if (result.DayOfWeek == DayOfWeek.Saturday) continue;
                if (result.DayOfWeek == DayOfWeek.Sunday) continue;

                days -= increment;
            }

            return result;
        }

        public static DateTime AddWorkingHours(DateTime date, int hours)
        {
            int day = hours < 0 ? -24 : 24;
            DateTime result = date;

            while (hours != 0)
            {
                int increment = Math.Abs(day) > Math.Abs(hours) ? hours : day;

                result = result.AddHours(increment);

                if (result.DayOfWeek == DayOfWeek.Saturday || result.DayOfWeek == DayOfWeek.Sunday) hours += day;

                hours -= increment;
            }

            return result;
        }

        public static MVCxGridViewCommandColumn AddSelectCheckBoxColumn(GridViewSettings settings)
        {
            MVCxGridViewCommandColumn column = new MVCxGridViewCommandColumn();

            column.Caption = " ";
            column.Name = "SelectCheckBoxColumn";
            column.Width = Unit.Pixel(35);
            column.Visible = true;
            column.VisibleIndex = 0;
            column.ShowSelectCheckbox = true;
            column.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
            column.FixedStyle = GridViewColumnFixedStyle.Left;

            settings.Columns.Add(column);

            return column;
        }

        public static DateTime? ParseDate(string dateString, params string[] formats)
        {
            if (String.IsNullOrEmpty(dateString)) return null;

            dateString = dateString.Trim();

            DateTime date;
            bool success = DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            return success ? (DateTime?)date : null;
        }

        public static string ParseHtml(string text)
        {
            if (String.IsNullOrEmpty(text)) return null;

            text = text.Replace("&nbsp;", " ").Trim();
            text = text.Replace(Environment.NewLine, " ").Trim();
            text = text.Replace("\t", " ").Trim();
            text = Regex.Replace(text, "\\s+", " ");

            return String.IsNullOrEmpty(text) ? null : text;
        }

        public static string ValidatePassword(int companyID, int? userID, string password, bool updated)
        {
            // Need to match at least two regex
            List<string> regexes = new List<string>();

            regexes.Add(@"^(?=.*[A-Z]).{6,}$"); //upperCase
            regexes.Add(@"^(?=.*[a-z]).{6,}$"); //lowerCase
            regexes.Add(@"^(?=.*\d).{6,}$");    //numeric
            regexes.Add(@"^(?=.*[$&+,:;=?@#|'<>.^*()%!-]).{6,}$");  //specialChars

            int matches = regexes.Count(x => Regex.IsMatch(password, x));

            if (matches < 2) return "The new password must be at least 6 characters long and contain at least two of the following character types: upper case, lower case, numeric, or special characters.";

            return null;
        }

        public static string GetStoredProcedureError(Exception ex, string defaultError)
        {
            if (ex != null && ex.Message.StartsWith(App.ErrorPrefix)) return ex.Message.Substring(App.ErrorPrefix.Length);

            return defaultError;
        }

        public static string CleanGridFilterExpression(string filter)
        {
            if (string.IsNullOrEmpty(filter) == false) filter = filter.Replace("'", "''");

            return filter;
        }

        public static string AppendGridViewClientSideEvent(string function, string appendedFunction)
        {
            if (string.IsNullOrWhiteSpace(function))
            {
                return string.Concat("function(s,e) { ", appendedFunction, " }");
            }

            return function.Insert(function.LastIndexOf('}'), string.Concat(appendedFunction, " "));
        }

        public static void UpdateLocationLatLong(SiteInfo address)
        {
            try
            {
                if (address == null) return;

                Thread.Sleep(250);
                string url = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&components=country:NZ&sensor=true&key=AIzaSyBcuS_wPID1WYiRSX9xpU_WmEOgVzfa14M", HttpUtility.UrlEncode(address.FormatedAddress));

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                string xml;

                using (WebResponse resp = request.GetResponse())
                {
                    using (Stream rStream = resp.GetResponseStream())
                    {
                        if (rStream == null) return;

                        using (StreamReader sr = new StreamReader(rStream))
                        {
                            xml = sr.ReadToEnd().Trim();
                        }
                    }
                }

                if (String.IsNullOrEmpty(xml)) return;

                GoogleResult.GeocodeResponse envelope = (GoogleResult.GeocodeResponse)Utils.DeserializeObject(xml, typeof(GoogleResult.GeocodeResponse));
                // Stop processing as over limit
                if (envelope.status == "OVER_QUERY_LIMIT")
                {
                    Log.Error(new Exception(address.Name + " geocode OVER_QUERY_LIMIT"));
                    address.Latitude = null;
                    address.Longitude = null;
                }
                else if (envelope.status != "OK")
                {
                    Log.Error(new Exception(address.Name + " result status " + envelope.status));
                    address.Latitude = null;
                    address.Longitude = null;
                }
                else
                {
                    // only use result if it is at least a street location
                    GoogleResult.MessageHeader result = envelope.result.FirstOrDefault(x => x.geometry.location_type != "APPROXIMATE");
                    if (result != null)
                    {
                        address.Latitude = Double.Parse(result.geometry.location.lat);
                        address.Longitude = Double.Parse(result.geometry.location.lng);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                address.Latitude = null;
                address.Longitude = null;
            }
        }

        public static DateTime? GetLocalTime(DateTime? utcDateTime)
        {
            if (utcDateTime == null) return null;

            return GetLocalTime(utcDateTime.Value);
        }

        public static DateTime GetLocalTime(DateTime utcDateTime)
        {
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(App.TimeZone);

            DateTime locaDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, zone);

            /*
            Console.WriteLine("The date and time are {0} {1}.", 
                                locaDateTime, 
                                zone.IsDaylightSavingTime(locaDateTime) ? zone.DaylightName : zone.StandardName);
            */

            return locaDateTime;
        }
    }
}
