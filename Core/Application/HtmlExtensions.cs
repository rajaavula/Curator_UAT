using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.Web.Mvc.UI;

namespace LeadingEdge.Curator.Core
{
	// INITIALISER
	public static class HtmlHelperExtensions
	{
		public static CortexExtensionFactory Cortex(this HtmlHelper h)
		{
			return new CortexExtensionFactory();
		}
	}

	// CONTROLS
	public class CortexExtensionFactory : ExtensionsFactory
	{
		// Form Begin/End
		public string BeginForm(string controller, string action, string pageID)
		{
			return BeginForm(null, controller, action, pageID);
		}

		public string BeginForm(string area, string controller, string action, string pageID)
		{
			string url = String.Format("/{0}/{1}", controller, action);

			if (String.IsNullOrEmpty(area) == false)
			{
				url = String.Format("/{0}{1}", area, url);
			}

			string html = String.Format("<form action=\"{0}\" method=\"post\">", url);

			if (!String.IsNullOrEmpty(pageID)) html += String.Format("<input type=\"hidden\" name=\"PageID\" id=\"PageID\" value=\"{0}\" />", pageID);
			
			return html;
		}

		public string EndForm()
		{
			return "</form>";
		}
		
		// Menu
		public MenuExtension Menu(string name)
		{
			return Menu(s => { s.Name = name; });
		}

		public MenuExtension Menu(Action<MenuSettings> action)
		{
			MenuSettings s = new MenuSettings();
			Setup.Menu(s);
			action.Invoke(s);

			return new MenuExtension(s);
		}

		// Label
		public LabelExtension Label(string name)
		{
			return Label(s => { s.Name = name; });
		}

		public LabelExtension Label(Action<LabelSettings> action)
		{
			LabelSettings s = new LabelSettings();
			Setup.Label(s);
			action.Invoke(s);

			return new LabelExtension(s);
		}

		// Textbox
		public TextBoxExtension TextBox(string name)
		{
			return TextBox(s => { s.Name = name; });
		}

		public TextBoxExtension TextBox(Action<TextBoxSettings> action)
		{
			TextBoxSettings s = new TextBoxSettings();
			Setup.TextBox(s);
			action.Invoke(s);

			return new TextBoxExtension(s);
		}

		// Memo
		public MemoExtension Memo(string name)
		{
			return Memo(s => { s.Name = name; });
		}

		public MemoExtension Memo(Action<MemoSettings> action)
		{
			MemoSettings s = new MemoSettings();
			Setup.Memo(s);
			action.Invoke(s);

			return new MemoExtension(s);
		}

		// Checkbox
		public CheckBoxExtension CheckBox(string name)
		{
			return CheckBox(s => { s.Name = name; });
		}

		public CheckBoxExtension CheckBox(Action<CheckBoxSettings> action)
		{
			CheckBoxSettings s = new CheckBoxSettings();
			Setup.CheckBox(s);
			action.Invoke(s);

			return new CheckBoxExtension(s);
		}

		// CheckCombo
		public DropDownEditExtension ComboList(string name, List<string> data)
		{
			return ComboList(s => { s.Name = name; }, data);
		}

		public DropDownEditExtension ComboList(Action<DropDownEditSettings> action, List<string> data)
		{
			DropDownEditSettings s = new DropDownEditSettings();
			ListBoxSettings l = new ListBoxSettings();
			l.Name = s.Name + "_checkComboListBox";
			l.ControlStyle.Border.BorderWidth = 0;
			l.ControlStyle.BorderBottom.BorderWidth = 1;
			l.ControlStyle.BorderBottom.BorderColor = Color.FromArgb(0xDCDCDC);
			l.Width = Unit.Percentage(100);

			l.Properties.SelectionMode = ListEditSelectionMode.CheckColumn;
			l.Properties.ClientSideEvents.SelectedIndexChanged = "OnListBoxSelectionChanged";

			string html = new ListBoxExtension(l).BindList(data).GetHtml().ToHtmlString();

			s.SetDropDownWindowTemplateContent(html);

			action.Invoke(s);

			return new DropDownEditExtension(s);
		}

		// Combobox
		public ComboBoxExtension ComboBox(string name)
		{
			return ComboBox(name, 1);
		}

		public ComboBoxExtension ComboBox(string name, int columns)
		{
			return ComboBox(s => { s.Name = name; });
		}

		public ComboBoxExtension ComboBox(Action<ComboBoxSettings> action)
		{
			return ComboBox(action, 1);
		}

		public ComboBoxExtension ComboBox(ComboBoxSettings settings, int columns)
		{
			Action<ComboBoxSettings> action = s => { };
			return ComboBox(settings, action, columns);
		}

		public ComboBoxExtension ComboBox(Action<ComboBoxSettings> action, int columns)
		{
			ComboBoxSettings s = new ComboBoxSettings();
			Setup.ComboBox(s);
			action.Invoke(s);

			if (columns > 1 && s.Properties.Columns.Count == 0)
			{
				s.Properties.TextFormatString = "{0}";
				ListBoxColumn c1 = new ListBoxColumn(s.Properties.ValueField) { Caption = "Code" };
				c1.Width = Unit.Pixel(90);
				s.Properties.Columns.Add(c1);

				ListBoxColumn c2 = new ListBoxColumn(s.Properties.TextField) { Caption = "Description" };
				c2.Width = Unit.Pixel(180);
				s.Properties.Columns.Add(c2);
			}

			return new ComboBoxExtension(s);
		}

		public ComboBoxExtension ComboBox(ComboBoxSettings s, Action<ComboBoxSettings> action, int columns)
		{
			Setup.ComboBox(s);
			action.Invoke(s);

			if (columns > 1 && s.Properties.Columns.Count == 0)
			{
				s.Properties.TextFormatString = "{0}";
				ListBoxColumn c1 = new ListBoxColumn(s.Properties.ValueField) { Caption = "Code" };
				c1.Width = Unit.Pixel(90);
				s.Properties.Columns.Add(c1);

				ListBoxColumn c2 = new ListBoxColumn(s.Properties.TextField) { Caption = "Description" };
				c2.Width = Unit.Pixel(250);
				s.Properties.Columns.Add(c2);
			}

			return new ComboBoxExtension(s);
		}

		// DateEdit
		public DateEditSettings DateEdit(string name)
		{
			return null;// DateEdit(s => { s.Name = name; });
		}

		public DateEditExtension DateEdit(Action<DateEditSettings> action)
		{
			DateEditSettings s = new DateEditSettings();
			Setup.DateEdit(s);
			action.Invoke(s);

			return new DateEditExtension(s);
		}

		// TimeEdit
		public TimeEditExtension TimeEdit(Action<TimeEditSettings> action)
		{
			TimeEditSettings s = new TimeEditSettings();
			Setup.TimeEdit(s);
			action.Invoke(s);

			return new TimeEditExtension(s);
		}

		// Textbox
		public SpinEditExtension SpinEdit(string name)
		{
			return SpinEdit(s => { s.Name = name; });
		}

		public SpinEditExtension SpinEdit(Action<SpinEditSettings> action)
		{
			SpinEditSettings s = new SpinEditSettings();
			Setup.SpinEdit(s);
			action.Invoke(s);

			return new SpinEditExtension(s);
		}

		// Button
		public ButtonExtension Button(string name, bool permission = true)
		{
			return Button(s => { s.Name = name; });
		}

		public ButtonExtension Button(Action<ButtonSettings> action, bool permission = true)
		{
			ButtonSettings s = new ButtonSettings();
			Setup.NeutralButton(s, false);
			action.Invoke(s);

			return new ButtonExtension(s);
		}

		public ButtonExtension NeutralButton(Action<ButtonSettings> action, bool permission = true)
		{
			return NeutralButton(false, action, permission);
		}

		public ButtonExtension NeutralButton(bool showLoadingPanel, Action<ButtonSettings> action,bool permission = true)
		{
			ButtonSettings s = new ButtonSettings();
			Setup.NeutralButton(s, showLoadingPanel, permission);
			action.Invoke(s);

			return new ButtonExtension(s);
		}

		public ButtonExtension AssignButton(bool showLoadingPanel, Action<ButtonSettings> action, bool permission = true)
		{
			ButtonSettings s = new ButtonSettings();
			Setup.AssignButton(s, showLoadingPanel, permission);
			action.Invoke(s);

			return new ButtonExtension(s);
		}

		public ButtonExtension PositiveButton(Action<ButtonSettings> action,bool permission = true)
		{
			return PositiveButton(false, action, permission);
		}

		public ButtonExtension PositiveButton(bool showLoadingPanel, Action<ButtonSettings> action, bool permission =true)
		{
			ButtonSettings s = new ButtonSettings();
			Setup.PositiveButton(s, showLoadingPanel, permission);
			action.Invoke(s);

			return new ButtonExtension(s);
		}

		public ButtonExtension NegativeButton(Action<ButtonSettings> action, bool permission = true)
		{
			return NegativeButton(false, action, permission);
		}

		public ButtonExtension NegativeButton(bool showLoadingPanel, Action<ButtonSettings> action, bool permission = true)
		{
			ButtonSettings s = new ButtonSettings();
			Setup.NegativeButton(s, showLoadingPanel, permission);
			action.Invoke(s);

			return new ButtonExtension(s);
		}

		public ButtonExtension RefreshButton(Action<ButtonSettings> action, bool showLoadingPanel)
		{
			ButtonSettings s = new ButtonSettings();
			Setup.NeutralButton(s, showLoadingPanel, true);
			action.Invoke(s);

			return new ButtonExtension(s);
		}
		
		// Page Control
		public PageControlExtension PageControl(string name)
		{
			return PageControl(s => { s.Name = name; });
		}

		public PageControlExtension PageControl(Action<PageControlSettings> action)
		{
			PageControlSettings s = new PageControlSettings();
			Setup.PageControl(s);
			action.Invoke(s);

			return new PageControlExtension(s);
		}

		// Page Control
		public PopupControlExtension PopupControl(string name)
		{
			return PopupControl(s => { s.Name = name; });
		}

		public PopupControlExtension PopupControl(Action<PopupControlSettings> action)
		{
			PopupControlSettings s = new PopupControlSettings();
			Setup.PopupControl(s);
			action.Invoke(s);

			return new PopupControlExtension(s);
		}
	}
}
