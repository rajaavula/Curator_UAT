using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using DevExpress.Web;
using LeadingEdge.Curator.Core;
using LeadingEdge.Curator.Web.Admin.Models;
using DevExpress.Data;
using DevExpress.Web.Mvc;

namespace LeadingEdge.Curator.Web.Admin.Helpers
{
	public static class LabelsHelper
	{
		public static LabelsListEdit CreateModel()
		{
			var vm = new LabelsListEdit();
			if (vm.SI.UserGroup.IsOwner) vm.Companies.Add(new CompanyInfo { CompanyID = 0, Name = "ALL COMPANIES" });
			vm.Companies.Add(vm.SI.Company);
			vm.SelectedCompanyID = vm.Companies.FirstOrDefault().CompanyID;

			vm.Grids.Add("GrdMain",GetGridView("GrdMain", false, vm, App.Languages));
			vm.Grids["GrdMain"].Data = GetData(vm);

			return vm;
		}

		public static void UpdateModel(LabelsListEdit vm, LabelsListEdit cached, bool isExporting)
		{
			vm.Grids = cached.Grids;
			vm.Companies = cached.Companies;

			if (isExporting) return;

			vm.Grids["GrdMain"].Data = GetData(vm);
		}

		private static GridModel GetGridView(string name, bool exporting, LabelsListEdit vm, List<LanguageInfo> languages)
		{
			var grid = new GridModel();
			Setup.GridView(grid.Settings, name, "v1.6", vm.Name);
			grid.Settings.ClientSideEvents.FocusedRowChanged = "function(s,e) { Get(); }";
			grid.Settings.CallbackRouteValues = new { Area = "Admin", Controller = "Labels", Action = "GrdMainCallback" };
			grid.Settings.KeyFieldName = "LabelID";

			grid.Settings.HtmlDataCellPrepared += (sender, e) =>
			{
				if (e.DataColumn.FieldName == "LabelType")
				{
					e.Cell.HorizontalAlign = HorizontalAlign.Left;

					if (Convert.ToString(e.CellValue) != "DEFAULT")
					{
						e.Cell.ForeColor = Color.White;
						e.Cell.BackColor = ColorTranslator.FromHtml("#09B239");
					}
				}
			};

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "LabelType";
				s.Caption = grid.Label(200335); // Label Type
				s.Width = 100;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "LabelID";
				s.Caption = grid.Label(200334); // Label ID
				s.Width = 80;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "PlaceholderID";
				s.Caption = grid.Label(200331); // "Placeholder ID";
				s.Width = 200;
				s.SortIndex = 0;
				s.SortOrder = ColumnSortOrder.Ascending;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.ComboBox;
				s.FieldName = "LanguageID";
				s.Caption = grid.Label(200024); // Language
				s.Width = 120;
				s.SortIndex = 1;
				s.SortOrder = ColumnSortOrder.Ascending;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                var comboBoxProperties = (ComboBoxProperties)s.PropertiesEdit;
				Setup.ComboBox(comboBoxProperties);
				comboBoxProperties.Width = Unit.Percentage(100);
				comboBoxProperties.DataSource = languages;
				comboBoxProperties.ValueField = "LanguageID";
				comboBoxProperties.ValueType = typeof(string);
				comboBoxProperties.TextField = "LanguageName";
			});

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "LabelText";
				s.Caption = grid.Label(200332); // Label Text
				s.Width = 250;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			grid.Settings.Columns.Add(s =>
			{
				s.ColumnType = MVCxGridViewColumnType.TextBox;
				s.FieldName = "ToolTipText";
				s.Caption = grid.Label(200333); // Tool Tip Text
				s.Width = 250;
                s.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            });

			return grid;
		}

		public static LabelInfo GetLabelInfo(LabelsListEdit vm, int LabelID)
		{
			var data = (List<LabelInfo>)vm.Grids["GrdMain"].Data;

			return data.Find(x => x.LabelID == LabelID);
		}

		public static string Update(LabelsListEdit vm, int? labelID, string labelType, int placeholderID, string languageID, string labelText, string toolTipText)
		{
			string error = String.Empty;
			var info = new LabelInfo();				// Don't assign ID as it could be null

			info.CompanyID = vm.SelectedCompanyID;
			info.PlaceholderID = placeholderID;
			info.LanguageID = languageID;
			info.LabelText = labelText;
			info.ToolTipText = toolTipText;

			// Can't add default labels for another company
			if (vm.SelectedCompanyID != 0 && labelType == "DEFAULT") labelID = null;

			error = LabelManager.Save(labelID, info);
			if (!String.IsNullOrEmpty(error)) return error;

			// Replace label in App.Labels
			if (labelID != null)
			{
				App.Labels.RemoveAll(x => x.LabelID == labelID);
			}

			LabelInfo newLabel = LabelManager.GetLabel(vm.SelectedCompanyID, placeholderID, languageID);
			App.Labels.Add(newLabel);

			vm.Grids["GrdMain"].Data = LabelManager.GetLabels(vm.SelectedCompanyID, "EDITING");

			return error;
		}

		public static string Delete(LabelsListEdit vm, int labelID)
		{
			LabelInfo label = App.Labels.Find(x => x.LabelID == labelID);
			if (vm.SelectedCompanyID == 0 && vm.SI.UserGroup.IsOwner==false) return "You cannot delete a default label.";

			string error = LabelManager.Delete(labelID, vm.SelectedCompanyID);
			if (!String.IsNullOrEmpty(error)) return error;

			// Remove label in App.Labels
			App.Labels.RemoveAll(x => x.LabelID == labelID);

			vm.Grids["GrdMain"].Data = LabelManager.GetLabels(vm.SelectedCompanyID, "EDITING");

			return error;
		}

		public static List<LabelInfo> GetData(LabelsListEdit vm)
		{
			return LabelManager.GetLabels(vm.SelectedCompanyID, "EDITING");
		}
	}
}