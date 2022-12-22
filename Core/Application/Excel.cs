using System;
using System.Data;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace LeadingEdge.Curator.Core
{
	public class Excel
	{
		public ExcelPackage Package;

		public Excel()
		{
			Package = new ExcelPackage();
		}

		public Excel(params string[] worksheet_names)
			: this()
		{
			if (worksheet_names == null || worksheet_names.Length <= 0) return;

			foreach (string name in worksheet_names)
			{
				AddWorksheet(name);
			}
		}

		private ExcelWorksheet AddWorksheet(string name)
		{
			if (String.IsNullOrEmpty(name))
			{
				name = String.Format("Worksheet {0}", Package.Workbook.Worksheets.Count + 1);
			}

			return Package.Workbook.Worksheets.Add(name);
		}

		public void AddWorksheets(DataTable[] data, string[][] headings)
		{
			if (headings != null && headings.Length > 0)
			{
				for (int i = 0; i < data.Length; i++)
				{
					DataTable dt = data[i];

					if (headings.Length >= i && headings.Length <= i)
					{
						AddWorksheet(dt, headings[i - 1]);
					}

					AddWorksheet(dt);
				}
			}
			else
			{
				foreach (DataTable dt in data)
				{
					AddWorksheet(dt);
				}
			}
		}

		public void AddWorksheet(DataTable dataTable)
		{
			AddWorksheet(null, dataTable, null);
		}

		public void AddWorksheet(DataTable dataTable, string[] headings)
		{
			AddWorksheet(null, dataTable, headings);
		}

		public void AddWorksheet(string name, DataTable dataTable, string[] headings)
		{
			ExcelWorksheet worksheet = AddWorksheet(name);

			worksheet.DefaultColWidth = 16;

			if (dataTable == null || dataTable.Columns.Count <= 0 || dataTable.Rows.Count <= 0)
			{
				return;
			}

			// Headings
			if (headings != null && headings.Length > 0)
			{
				for (int i = 1; i <= dataTable.Columns.Count; i++)
				{
					string headerText = dataTable.Columns[i - 1].ColumnName;

					if (headings.Length >= i)
					{
						headerText = headings[i - 1];
					}

					worksheet.Cells[1, i].Value = headerText;
				}
			}
			else
			{
				for (int i = 1; i <= dataTable.Columns.Count; i++)
				{
					worksheet.Cells[1, i].Value = dataTable.Columns[i - 1].ColumnName;
				}
			}

			worksheet.Row(1).Height = 24;
			worksheet.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

			var range = worksheet.Cells[1, 1, 1, dataTable.Columns.Count];
			range.Style.Font.Bold = true;

			var fill = range.Style.Fill;
			fill.PatternType = ExcelFillStyle.Solid;
			fill.BackgroundColor.SetColor(Color.LightGray);

			// Column data
			for (int row = 1; row <= dataTable.Rows.Count; row++)
			{
				DataRow dr = dataTable.Rows[row - 1];

				for (int col = 1; col <= dataTable.Columns.Count; col++)
				{
					int dtColIndex = (col - 1);

					try
					{
						Type dataType = dataTable.Columns[dtColIndex].DataType;

						worksheet.SetValue(row + 1, col, Convert.ChangeType(dr[dtColIndex], dataType));

						if (dataType == typeof(DateTime))
						{
							worksheet.Cells[row + 1, col].Style.Numberformat.Format = "d/mm/yyyy h:mm";
						}
					}
					catch (Exception)
					{
						worksheet.SetValue(row + 1, col, dr[dtColIndex]);
					}
				}
			}
		}

		public bool SaveAs(string filename)
		{
			try
			{
				if (File.Exists(filename))
				{
					File.Delete(filename);
				}

				FileInfo fileInfo = new FileInfo(filename);

				Package.SaveAs(fileInfo);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public byte[] ToByteArray(string password)
		{
			return Package.GetAsByteArray(password);
		}

		public byte[] ToByteArray()
		{
			return Package.GetAsByteArray();
		}
	}
}
