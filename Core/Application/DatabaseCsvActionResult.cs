using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Web.Mvc;

namespace LeadingEdge.Curator.Core.Application
{
	public class DatabaseCSVResult : ActionResult
	{
		private readonly DatabaseCsvParameters Data;

		public DatabaseCSVResult(DatabaseCsvParameters parameters)
		{
			Data = parameters;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (Data == null) return;

			var response = context.HttpContext.Response;

			// Zipped CSV Export
			string zipFilename = Path.ChangeExtension(Data.Filename, ".zip");

			response.ContentType = "application/zip";

			response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}", zipFilename));

			using (SqlConnection sqlConnection = new SqlConnection(Data.ConnectionString))
			{
				try
				{
					sqlConnection.Open();

					using (SqlCommand sqlCommand = new SqlCommand(Data.StoredProcedureName, sqlConnection))
					{
						sqlCommand.CommandTimeout = 1200;

						sqlCommand.Parameters.Clear();

						if (Data.SqlParameters != null && Data.SqlParameters.Length > 0)
						{
							sqlCommand.Parameters.AddRange(Data.SqlParameters);
						}

						sqlCommand.CommandType = CommandType.StoredProcedure;

						bool includedHeaders = false;

						using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
						{
							// Create a zip archive
							using (PositionWrapperStream outputStream = new PositionWrapperStream(response.OutputStream))
							{
								using (ZipArchive zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create, false))
								{
									ZipArchiveEntry zipEntry = zipArchive.CreateEntry(Data.Filename);

									using (Stream zipEntryStream = zipEntry.Open())
									{
										using (StreamWriter sw = new StreamWriter(zipEntryStream))
										{
											while (dataReader.Read())
											{
												if (includedHeaders == false)
												{
													for (int i = 0; i < dataReader.FieldCount; i++)
													{
														sw.Write(dataReader.GetName(i));

														if (i < (dataReader.FieldCount - 1))
														{
															sw.Write(",");
															continue;
														}

														sw.Write("\r\n");
													}

													includedHeaders = true;
												}

												for (int i = 0; i < dataReader.FieldCount; i++)
												{
													object value = dataReader[i];
													string output = null;

													if (value == null || (value is INullable && ((INullable) value).IsNull))
													{
														output = String.Empty;
													}
													else if (value is DateTime)
													{
														if (((DateTime) value).TimeOfDay.TotalSeconds == 0)
														{
															output = ((DateTime) value).ToString("yyyy-MM-dd");
														}
														else
														{
															output = ((DateTime) value).ToString("yyyy-MM-dd HH:mm:ss");
														}
													}
													else
													{
														output = Convert.ToString(value);
													}

													if (output.Contains(",") || output.Contains("\"") || output.Contains("\n"))
													{
														output = '"' + output.Replace("\"", "\"\"") + '"';
													}

													sw.Write(output);

													if (i < (dataReader.FieldCount - 1))
													{
														sw.Write(",");
														continue;
													}

													sw.Write("\r\n");
												}
											}
										}
									}
								}
							}
						}
					}
				}
				finally
				{
					sqlConnection.Close();
				}
			}
		}
	}
}
