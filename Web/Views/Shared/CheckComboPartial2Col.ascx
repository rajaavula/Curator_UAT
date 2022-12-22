<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CheckComboModel2Col>" %>
<script src="/Assets/Js/cx.js" type="text/javascript"></script><%

	Html.DevExpress().DropDownEdit(settings =>
	{
		settings.Name = Model.Name;
		settings.Text = Model.Text;		
		settings.Properties.DropDownWindowStyle.BackColor = Color.FromArgb(0xEDEDED);
		settings.Properties.Width = (Model.Width == 0 ? 100 : Model.Width);
		settings.Properties.DropDownWindowHeight = 200;

		settings.SetDropDownWindowTemplateContent(c =>
		{
			Html.DevExpress().ListBox(listBoxSettings =>
			{
				listBoxSettings.Name = Model.Name + "checkComboListBox";
				listBoxSettings.ControlStyle.Border.BorderWidth = 0;
				listBoxSettings.ControlStyle.BorderBottom.BorderWidth = 1;
				listBoxSettings.ControlStyle.BorderBottom.BorderColor = Color.FromArgb(0xDCDCDC);
				listBoxSettings.Width = Model.Width;
				listBoxSettings.Height = 200;
				
				ListBoxColumn c1 = new ListBoxColumn(listBoxSettings.Properties.ValueField) { Caption = "Code", FieldName = "Code" };
				c1.Width = Unit.Pixel(90);
				listBoxSettings.Properties.Columns.Add(c1);
				listBoxSettings.Properties.ValueField = "Code";
				if (!Model.ShowValue) c1.Visible = false;

				ListBoxColumn c2 = new ListBoxColumn(listBoxSettings.Properties.TextField) { Caption = "Description", FieldName = "Description" };
				c2.Width = Unit.Pixel(180);
				listBoxSettings.Properties.Columns.Add(c2);

				listBoxSettings.Properties.SelectionMode = ListEditSelectionMode.CheckColumn;
				listBoxSettings.Properties.ClientSideEvents.SelectedIndexChanged = "function (listBox, args) { cxOnListBoxSelectionChanged(" + Model.Name + ", listBox, args); }";
			}).BindList(Model.Data).Render();
		});
		settings.Properties.EnableAnimation = false;
		settings.Properties.ClientSideEvents.TextChanged = "function (dropDown, args) { cxSynchronizeListBoxValues(dropDown, " + Model.Name + "checkComboListBox, args); }";
		settings.Properties.ClientSideEvents.DropDown = "function (dropDown, args) { cxSynchronizeListBoxValues(dropDown, " + Model.Name + "checkComboListBox, args); }";
	}
	).GetHtml();

%>