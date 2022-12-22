using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cortex.Utilities;
using LeadingEdge.Curator.Core;

namespace LeadingEdge.Curator.Core
{
    public static class DocumentManager
    {
        public static DocumentInfo GetDocument(int companyID, int regionID, int documentID, bool includeBytes)
        {
            List<DocumentInfo> documents = GetDocuments(companyID, regionID, documentID, null, 0, null, includeBytes);

            if (documents.Count() == 1) return documents.First();

            return null;
        }

        public static List<DocumentInfo> GetDocuments(int companyID, int regionID, int? documentID, string referenceType, int referenceID, string fileType, bool includeBytes)
        {
            DB db = new DB(App.CuratorDBConn);

            var parameters = new[]
            {
                new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
                new SqlParameter("@RegionID", Utils.ToDBValue(regionID)),
                new SqlParameter("@DocumentID", Utils.ToDBValue(documentID)),
                new SqlParameter("@ReferenceType", Utils.ToDBValue(referenceType)),
                new SqlParameter("@ReferenceID", Utils.ToDBValue(referenceID)),
                new SqlParameter("@FileType", Utils.ToDBValue(fileType)),
                new SqlParameter("@IncludeBytes", Utils.ToDBValue(includeBytes))
            };

            DataTable dt = db.QuerySP("GetDocuments", parameters);
            if (db.Success == false) Log.Error(db.DBException);

            return new List<DocumentInfo>(from DataRow dr in dt.Rows select new DocumentInfo(dr));
        }

        public static Exception SaveDocument(DocumentInfo info)
        {
            DB db = new DB(App.CuratorDBConn);

            var parameters = new[]
            {
                new SqlParameter("@CompanyID", Utils.ToDBValue(info.CompanyID)),
                new SqlParameter("@RegionID", Utils.ToDBValue(info.RegionID)),
                new SqlParameter("@ReferenceType", Utils.ToDBValue(info.ReferenceType)),
                new SqlParameter("@ReferenceID", Utils.ToDBValue(info.ReferenceID)),
                new SqlParameter("@FileType", Utils.ToDBValue(info.FileType)),
                new SqlParameter("@Filename", Utils.ToDBValue(info.Filename)),
                new SqlParameter("@Bytes", Utils.ToDBValue(info.Bytes)),
                new SqlParameter("@UploadedByID", Utils.ToDBValue(info.UploadedByID))
            };

            db.QuerySP("SaveDocument", parameters);

            if (db.Success == false)
            {
                Log.Error(db.DBException);
            }
            else
            {
                SaveDocumentHistory(info, info.UploadedByID, null, info.Filename);
            }

            return db.DBException;
        }

        public static Exception DeleteDocument(int companyID, int regionID, int documentID, int userID)
        {
            DB db = new DB(App.CuratorDBConn);
            var info = DocumentManager.GetDocument(companyID, regionID, documentID, false);

            var parameters = new[]
            {
                new SqlParameter("@CompanyID", Utils.ToDBValue(companyID)),
                new SqlParameter("@RegionID", Utils.ToDBValue(regionID)),
                new SqlParameter("@DocumentID", Utils.ToDBValue(documentID))
            };

            db.QuerySP("DeleteDocument", parameters);

            if (db.Success == false)
            {
                Log.Error(db.DBException);
            }
            else
            {
                SaveDocumentHistory(info, userID, info.Filename, null);
            }

            return db.DBException;
        }
        public static void SaveDocumentHistory(DocumentInfo info, int userID, string oldValue, string newValue)
        {
            var placeHolderID = 200712; // File

            DB db = new DB(App.CuratorDBConn);
            var parameters = new[]
            {
                new SqlParameter("@CompanyID", Utils.ToDBValue(info.CompanyID)),
                new SqlParameter("@RegionID", Utils.ToDBValue(info.RegionID)),
                new SqlParameter("@ReferenceType", Utils.ToDBValue(info.ReferenceType)),
                new SqlParameter("@ReferenceID", Utils.ToDBValue(info.ReferenceID)),
                new SqlParameter("@UserID", Utils.ToDBValue(userID)),
                new SqlParameter("@PlaceholderID", Utils.ToDBValue(placeHolderID)),
                new SqlParameter("@OldValue", Utils.ToDBValue(oldValue)),
                new SqlParameter("@NewValue", Utils.ToDBValue(newValue)),
            };

            db.QuerySP("SaveChangeHistory", parameters);

            if (db.Success == false) Log.Error(db.DBException);
        }


    }
}
