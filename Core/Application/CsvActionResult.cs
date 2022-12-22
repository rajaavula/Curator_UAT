using System.Collections;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Web.Mvc;

namespace LeadingEdge.Curator.Core.Application
{
	public class CSVResult : ActionResult
	{
		private readonly string Filename;
		private readonly object Data;

		public CSVResult(string filename, object data)
		{
			Filename = filename;
			Data = data;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (Data == null) return;

			bool zipExport;

			var response = context.HttpContext.Response;

			if (Data.GetType() == typeof (DataTable))
			{
				DataTable dt = (DataTable) Data;
				if (dt == null || dt.Rows.Count < 1) return;
				zipExport = (dt.Rows.Count > App.MaxXlsExportRows);
			}
			else
			{
				ICollection collection = (Data as ICollection);
				if (collection == null || collection.Count < 1) return;
				zipExport = (collection.Count > App.MaxXlsExportRows);
			}

			#region Standard CSV export

			if (zipExport == false) // No need to zip as file is small enough
			{
				string csvFilename = Path.ChangeExtension(Filename, ".csv");
				response.ContentType = "text/csv";
				response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}", csvFilename));

				using (var textWriter = new StreamWriter(response.OutputStream))
				{
					if (Data.GetType() == typeof(DataTable))
					{
						DataTable dt = (DataTable)Data;
						U.CSVExporter.Convert(textWriter, dt);
						return;
					}

					ICollection collection = (Data as ICollection);
					U.CSVExporter.Convert(textWriter, collection);
				}
			}

			#endregion

			#region Zipped CSV export

			string zipFilename = Path.ChangeExtension(Filename, ".zip");
			response.ContentType = "application/zip";
			response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}", zipFilename));

			// Create a zip archive
			using (var outputStream = new PositionWrapperStream(response.OutputStream))
			{
				using (var zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create, false))
				{
					ZipArchiveEntry zipEntry = zipArchive.CreateEntry(Filename);

					using (var zipEntryStream = zipEntry.Open())
					{
						using (var textWriter = new StreamWriter(zipEntryStream))
						{
							if (Data.GetType() == typeof (DataTable))
							{
								DataTable dt = (DataTable) Data;
								U.CSVExporter.Convert(textWriter, dt);
							}
							else
							{
								ICollection collection = (Data as ICollection);
								U.CSVExporter.Convert(textWriter, collection);
							}
						}
					}
				}
			}

			#endregion
		}
	}
}