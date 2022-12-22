
using System;
using System.Collections.Generic;
using System.Data;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
	public class DocumentInfo
	{
		public int DocumentID { get; set; }
		public int CompanyID { get; set; }
		public int RegionID { get; set; }
		public string ReferenceType { get; set; }
		public int ReferenceID { get; set; }
		public string FileType { get; set; }
		public string Filename { get; set; }
		public byte[] Bytes { get; set; }
		public DateTime Uploaded { get; set; }
		public int UploadedByID { get; set; }
		public string UploadedBy { get; set; }
		public bool DeleteItem { get; set; }

		public DocumentInfo() { }

		public DocumentInfo(DataRow dr)
		{
			DocumentID = Utils.FromDBValue<int>(dr["DocumentID"]);
			CompanyID = Utils.FromDBValue<int>(dr["CompanyID"]);
			RegionID = Utils.FromDBValue<int>(dr["RegionID"]);
			ReferenceType = Utils.FromDBValue<string>(dr["ReferenceType"]);
			ReferenceID = Utils.FromDBValue<int>(dr["ReferenceID"]);
			FileType = Utils.FromDBValue<string>(dr["FileType"]);
			Filename = Utils.FromDBValue<string>(dr["Filename"]);
			Bytes = Utils.FromDBValue<byte[]>(dr["Bytes"]);
			Uploaded = U.GetLocalTime(Utils.FromDBValue<DateTime>(dr["Uploaded"]));
			UploadedByID = Utils.FromDBValue<int>(dr["UploadedByID"]);
			UploadedBy = Utils.FromDBValue<string>(dr["UploadedBy"]);
		}

		public DocumentInfo(DocumentInfo info)
		{
			DocumentID = info.DocumentID;
			CompanyID = info.CompanyID;
			RegionID = info.RegionID;
			ReferenceType = info.ReferenceType;
			ReferenceID = info.ReferenceID;
			FileType = info.FileType;
			Filename = info.Filename;
			Bytes = info.Bytes;
			Uploaded = info.Uploaded;
			UploadedByID = info.UploadedByID;
			UploadedBy = info.UploadedBy;
			DeleteItem = info.DeleteItem;
		}
	}
}
