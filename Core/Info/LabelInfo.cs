using System;
using System.Data;
using System.Web;
using Cortex.Utilities;

namespace LeadingEdge.Curator.Core
{
    public class LabelInfo
    {
        public static string MissingLabelText = "*** !!! ***";

        public int LabelID { get; set; }
        public int CompanyID { get; set; }
        public int PlaceholderID { get; set; }
        public string LanguageID { get; set; }
        public string LabelText { get; set; }
        public string ToolTipText { get; set; }
        public bool Error { get; set; }
        public string LabelType { get; set; }

        public LabelInfo() { }

        public LabelInfo Clone()
        {
            LabelInfo newLabel = new LabelInfo();
            newLabel.LabelID = LabelID;
            newLabel.CompanyID = CompanyID;
            newLabel.PlaceholderID = PlaceholderID;
            newLabel.LanguageID = LanguageID;
            newLabel.LabelText = LabelText;
            newLabel.ToolTipText = ToolTipText;
            newLabel.Error = Error;
            newLabel.LabelType = LabelType;

            return newLabel;
        }

        public LabelInfo(DataRow dr)
        {
            LabelID = Utils.FromDBValue<int>(dr["LabelID"]);
            CompanyID = Utils.FromDBValue<int>(dr["CompanyID"]);
            PlaceholderID = Utils.FromDBValue<int>(dr["PlaceholderID"]);
            LanguageID = Utils.FromDBValue<string>(dr["LanguageID"]);
            LabelText = Utils.FromDBValue<string>(dr["LabelText"]);
            ToolTipText = Utils.FromDBValue<string>(dr["ToolTipText"]);
            LabelType = Utils.FromDBValue<string>(dr["LabelType"]);
        }

        public string Format(bool editMode)
        {
            if (editMode && LabelText != MissingLabelText)
            {
                return String.Format("<var id=\"{0}\" class=\"edit-label\">{1}</var>", PlaceholderID, HttpUtility.HtmlEncode(LabelText));
            }

            if (String.IsNullOrEmpty(ToolTipText) == false)
            {
                return String.Format("<var id=\"{0}\" class=\"tool-tip\">{1}</var>", PlaceholderID, HttpUtility.HtmlEncode(LabelText));
            }

            return HttpUtility.HtmlEncode(LabelText);
        }
    }
}
