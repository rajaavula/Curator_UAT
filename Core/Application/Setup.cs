using System;
using System.Web;
using System.Drawing;
using System.Web.UI.WebControls;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraCharts;
using Font = System.Drawing.Font;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeadingEdge.Curator.Core
{
    public static class Setup
    {
        public static Color GlobalBaseColor = ColorTranslator.FromHtml(App.GlobalThemeBaseColor);
        public static Color DefaultForeColor = ColorTranslator.FromHtml("#919191");
        public static string DefaultFont = "Montserrat";
        public static int DefaultWidth = 280;
        public static int DefaultHeight = 38;

        public static SessionInfo SI
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session == null) return null;

                return (SessionInfo)HttpContext.Current.Session["sSessionInfo"];
            }
            set
            {
                if (HttpContext.Current == null || HttpContext.Current.Session == null) return;

                HttpContext.Current.Session["sSessionInfo"] = value;
            }
        }
        public static void ValidationSettings(ValidationSettings s)
        {
            s.Display = Display.Static;
            s.ErrorTextPosition = ErrorTextPosition.Right;
            s.ErrorDisplayMode = ErrorDisplayMode.ImageWithText;
        }

        public static void Menu(MenuSettings s)
        {
            s.AllowSelectItem = true;
            s.EnableHotTrack = true;
            s.EnableAnimation = true;
            s.AppearAfter = 200;
            s.DisappearAfter = 200;
        }

        public static void Label(LabelSettings s)
        {
        }

        public static void TextBox(TextBoxSettings s)
        {
            s.ControlStyle.CssClass = "dx-controls-default";
            s.DisabledStyle.CssClass = "dx-controls-disabled";
            s.Properties.ReadOnlyStyle.CssClass = "dx-controls-read-only";
            s.Width = DefaultWidth;

            ValidationSettings(s.Properties.ValidationSettings);
        }

        public static void CheckBox(CheckBoxSettings s)
        {
            ValidationSettings(s.Properties.ValidationSettings);
            s.ControlStyle.ForeColor = Color.Black;
        }

        public static void Memo(MemoSettings s)
        {
            s.ControlStyle.CssClass = "memo";
            s.DisabledStyle.CssClass = "memo-disabled";
            s.Properties.ReadOnlyStyle.CssClass = "memo-read-only";
            s.Width = 400;

            ValidationSettings(s.Properties.ValidationSettings);
        }

        public static void ComboBox(ComboBoxSettings s)
        {
            s.Properties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            s.Properties.IncrementalFilteringDelay = App.IncrementalFilteringDelay;
            s.Properties.CallbackPageSize = App.CallbackPageSize;
            s.Properties.ItemStyle.Height = Unit.Pixel(38);
            s.Properties.ItemStyle.Paddings.PaddingLeft = Unit.Pixel(10);
            s.Properties.ItemStyle.Paddings.PaddingRight = Unit.Pixel(10);
            
            s.ControlStyle.CssClass = "dx-controls-default";
            s.DisabledStyle.CssClass = "dx-controls-disabled";
            s.Properties.ReadOnlyStyle.CssClass = "dx-controls-read-only";
            s.Width = DefaultWidth;            
            s.Properties.ItemStyle.Wrap = DefaultBoolean.True;

            ValidationSettings(s.Properties.ValidationSettings);
        }

        public static void ComboBox(ComboBoxProperties p)       // For in-grid combos
        {
            p.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            p.IncrementalFilteringDelay = App.IncrementalFilteringDelay;
            p.CallbackPageSize = App.CallbackPageSize;
            p.Width = DefaultWidth; 
        }

        public static void DateEdit(DateEditSettings s)
        {
            s.Width = Unit.Pixel(164);
            s.ControlStyle.CssClass = "dx-controls-default";
            s.DisabledStyle.CssClass = "dx-controls-disabled";
            s.Properties.ReadOnlyStyle.CssClass = "dx-controls-read-only";

            //s.Properties.DropDownButton.Image.Url = "/Assets/img/date-edit-calendar.png";

            ValidationSettings(s.Properties.ValidationSettings);
        }

        public static void TimeEdit(TimeEditSettings s)
        {
            s.Width = Unit.Pixel(100);
            s.ControlStyle.CssClass = "dx-controls-default";
            s.DisabledStyle.CssClass = "dx-controls-disabled";
            s.Properties.ReadOnlyStyle.CssClass = "dx-controls-read-only";

            ValidationSettings(s.Properties.ValidationSettings);
        }

        public static void SpinEdit(SpinEditSettings s)
        {
            s.Width = Unit.Pixel(164);
            s.ControlStyle.CssClass = "dx-controls-default";
            s.DisabledStyle.CssClass = "dx-controls-disabled";
            s.Properties.ReadOnlyStyle.CssClass = "dx-controls-read-only";

            ValidationSettings(s.Properties.ValidationSettings);
        }

        public static void CheckBoxCombo(CheckComboSettings s, string name)
        {
            var dropdown = s.DropDownSettings;

            dropdown.Name = name;
            dropdown.ControlStyle.CssClass = "dx-controls-default";
            dropdown.DisabledStyle.CssClass = "dx-controls-disabled";
            dropdown.Properties.ReadOnlyStyle.CssClass = "dx-controls-read-only";
            dropdown.Width = DefaultWidth;
            dropdown.Height = DefaultHeight;

            dropdown.Properties.AnimationType = AnimationType.None;

            // The below tries to use text to find IDs. Messy when there are same descriptions on items
            //dropdown.Properties.ClientSideEvents.TextChanged = string.Concat("function (dropDown, args) { cxSynchronizeListBoxValues(dropDown, ", dropdown.Name, "_ListBox, args); }");
            //dropdown.Properties.ClientSideEvents.DropDown = string.Concat("function (dropDown, args) { cxSynchronizeListBoxValues(dropDown, ", dropdown.Name, "_ListBox, args); }");

            var listbox = s.ListBoxSettings;
            listbox.Name = string.Concat(dropdown.Name, "_ListBox");
            listbox.Width = DefaultWidth;
            listbox.Height = Unit.Pixel(250);
            listbox.Properties.ItemStyle.Height = DefaultHeight;           
            listbox.Properties.ItemStyle.Wrap = DefaultBoolean.True;
            listbox.ControlStyle.Border.BorderWidth = 0;
            listbox.ControlStyle.BorderBottom.BorderWidth = 1;
            listbox.ControlStyle.CssClass = "dx-controls-default";
            listbox.DisabledStyle.CssClass = "dx-controls-disabled";
            listbox.Properties.ReadOnlyStyle.CssClass = "dx-controls-read-only";
            listbox.Properties.SelectionMode = ListEditSelectionMode.CheckColumn;
            listbox.Properties.ClientSideEvents.SelectedIndexChanged = string.Concat("function (listBox, args) { cxOnListBoxSelectionChanged(", dropdown.Name, ", listBox, args); }");
        }

        /* Button designs */

        // Default settings please
        public static void Button(ButtonSettings s, bool showLoadingPanel)
        {
            s.Width = 100;
            s.Height = Unit.Pixel(DefaultHeight);
            s.UseSubmitBehavior = true;
            s.Style.Add("border-radius", "5px");

            // On hover use global forecolor and border on white
            s.Styles.Style.HoverStyle.BackColor = Color.White;
            s.Styles.Style.HoverStyle.ForeColor = GlobalBaseColor;
            s.Styles.Style.HoverStyle.BorderColor = GlobalBaseColor;
            s.Styles.Style.HoverStyle.BorderWidth = 1;

            if (showLoadingPanel) s.ClientSideEvents.Click = @"function (s, e) { ShowLoadingPanel(); } ";
        }

        public static void NeutralButton(ButtonSettings s, bool showLoadingPanel, bool permission = true)
        {
            Button(s, showLoadingPanel);

            //Design changes
            var backColor = ColorTranslator.FromHtml("#6b6b6b");

            s.ClientVisible = permission;
            s.Styles.Style.BackColor = Color.White;
            s.Styles.Style.BorderWidth = 1;

            s.Styles.Style.PressedStyle.BackColor = GlobalBaseColor;
            s.Styles.Style.PressedStyle.ForeColor = ColorTranslator.FromHtml("#FFF");
            s.Styles.Style.PressedStyle.BorderWidth = 1;
            s.Styles.Style.PressedStyle.BorderColor = GlobalBaseColor;
        }

        public static void AssignButton(ButtonSettings s, bool showLoadingPanel, bool permission = true)
        {
            Button(s, showLoadingPanel);

            //Design changes
            var backColor = ColorTranslator.FromHtml("#6b6b6b");
            s.ClientVisible = permission;
            s.Styles.Style.BackColor = Color.White;
            s.Styles.Style.BorderWidth = 1;
            s.Styles.Style.PressedStyle.BackColor = Color.White;
            s.Styles.Style.PressedStyle.ForeColor = GlobalBaseColor;
            s.Styles.Style.PressedStyle.BorderColor = GlobalBaseColor;
            s.Styles.Style.PressedStyle.BorderWidth = 1;
        }

        public static void PositiveButton(ButtonSettings s, bool showLoadingPanel, bool permission = true)
        {
            Button(s, showLoadingPanel);

            //Design changes
            s.ClientVisible = permission;

            s.Styles.Style.BackColor = GlobalBaseColor;
            s.Styles.Style.ForeColor = ColorTranslator.FromHtml("#FFF");
            s.Styles.Style.BorderWidth = 1;
            s.Styles.Style.BorderColor = GlobalBaseColor;

            s.Styles.Style.PressedStyle.BackColor = GlobalBaseColor;
            s.Styles.Style.PressedStyle.ForeColor = ColorTranslator.FromHtml("#FFF");
            s.Styles.Style.PressedStyle.BorderWidth = 1;
            s.Styles.Style.PressedStyle.BorderColor = GlobalBaseColor;
        }

        public static void NegativeButton(ButtonSettings s, bool showLoadingPanel, bool permission = true)
        {
            Button(s, showLoadingPanel);

            //Design changes
            s.Styles.Style.BackColor = Color.White;
            s.Styles.Style.BorderWidth = 1;

            s.Styles.Style.PressedStyle.BackColor = Color.White;
            s.Styles.Style.PressedStyle.ForeColor = GlobalBaseColor;
            s.Styles.Style.PressedStyle.BorderColor = GlobalBaseColor;
            s.Styles.Style.PressedStyle.BorderWidth = 1;
        }

        public static void PageControl(PageControlSettings s)
        {
            s.EnableTheming = false;
            s.ActivateTabPageAction = ActivateTabPageAction.Click;
            s.EnableHotTrack = true;
            s.Width = Unit.Percentage(100);
            s.SaveStateToCookies = false;
            s.TabAlign = TabAlign.Left;
            s.TabPosition = TabPosition.Top;
            s.Styles.Tab.Font.Bold = true;
            s.Styles.Tab.Font.Size = FontUnit.Parse("14px");
            s.Styles.Content.BackColor = ColorTranslator.FromHtml("#F3F4F6");
            s.Styles.Content.BorderLeft.BorderWidth = 0;
            s.Styles.Content.BorderRight.BorderWidth = 0;
            s.Styles.Content.BorderBottom.BorderWidth = 0;
            s.Styles.Content.BorderTop.BorderWidth = 1;
            s.Styles.Content.BorderTop.BorderColor = ColorTranslator.FromHtml("#CFCFD1");
            s.Styles.Tab.BackColor = ColorTranslator.FromHtml("#F3F4F6");                       // Light gray bg
            s.Styles.Tab.ForeColor = ColorTranslator.FromHtml("#2A2F34");                       // Dark gray
            s.Styles.Tab.Border.BorderWidth = 0;
            s.Styles.Tab.HoverStyle.BorderLeft.BorderWidth = 0;
            s.Styles.Tab.HoverStyle.BorderRight.BorderWidth = 0;
            s.Styles.Tab.HoverStyle.BorderBottom.BorderWidth = 2;
            s.Styles.Tab.HoverStyle.BorderBottom.BorderColor = GlobalBaseColor;
            s.Styles.ActiveTab.BackColor = ColorTranslator.FromHtml("#F3F4F6");
            s.Styles.ActiveTab.ForeColor = GlobalBaseColor;
            s.Styles.ActiveTab.BorderLeft.BorderWidth = 0;
            s.Styles.ActiveTab.BorderRight.BorderWidth = 0;
            s.Styles.ActiveTab.BorderBottom.BorderWidth = 2;
            s.Styles.ActiveTab.BorderBottom.BorderColor = GlobalBaseColor;
            s.ClientSideEvents.ActiveTabChanged = "function(s,e) { if (window.onresize) window.onresize(); }";
            s.ClientSideEvents.Init = "function(s,e) { try { RegisterTabControl(s); } catch(e) {} }";
        }

        public static void PopupControl(PopupControlSettings s)
        {
            s.AppearAfter = 0;
            s.AllowDragging = true;
            s.AllowResize = false;
            s.Modal = true;
            s.CloseAction = CloseAction.None;
            s.ShowCloseButton = false;
            s.ShowShadow = true;
            s.PopupAnimationType = AnimationType.Fade;
            s.PopupAction = PopupAction.LeftMouseClick;
            s.PopupHorizontalAlign = PopupHorizontalAlign.WindowCenter;
            s.PopupVerticalAlign = PopupVerticalAlign.WindowCenter;
            s.AutoUpdatePosition = true;
        }

        public static void GridView(GridViewSettings s, string name, string version, string modelname)
        {
            GridView(s, name, version, modelname, true, true);
        }

        public static void GridView(GridViewSettings s, string name, string version, string modelname, bool commandColumnHeader)
        {
            GridView(s, name, version, modelname, true, commandColumnHeader);
        }

        public static void GridView(GridViewSettings s, string name, string version, string modelname, bool autoResize, bool commandColumnHeader)
        {
            // General
            s.Name = name;
            //s.Width = Unit.Percentage(100);
            s.SettingsBehavior.AllowFocusedRow = true;
            s.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
            s.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
            s.SettingsResizing.ColumnResizeMode = ColumnResizeMode.Control;
            s.CallbackRouteValues = new { Area = "Home", Controller = "Home", Action = "BaseGridViewCallback" };

            // Design adjustments
            var blackForeColor = ColorTranslator.FromHtml("#2A2F34");
            var redRowBackColor = ColorTranslator.FromHtml(App.GlobalThemeBaseColor);
            var redRowForeColor = ColorTranslator.FromHtml("#FFF");
            var focusedRowBackcolor = ColorTranslator.FromHtml("#FDB525");
            var focusedRowForecolor = ColorTranslator.FromHtml("#FFF");

            s.ClientVisible = false;    // Will be shown after grid resize to fit content pane

            s.ControlStyle.CssClass = "dx-grid-default";
            s.Styles.Cell.Paddings.PaddingTop = Unit.Pixel(16);  // make the rows fatter
            s.Styles.Cell.Paddings.PaddingBottom = Unit.Pixel(16);
            s.Styles.Cell.Font.Size = FontUnit.Parse("14px");

            s.Styles.GroupPanel.Paddings.Padding = Unit.Pixel(16);
            s.Styles.GroupPanel.ForeColor = blackForeColor;

            s.Styles.Header.Paddings.PaddingTop = Unit.Pixel(6);
            s.Styles.Header.Paddings.PaddingBottom = Unit.Pixel(6);
            s.Styles.Header.BackColor = ColorTranslator.FromHtml("#2A2F34");
            s.Styles.Header.ForeColor = ColorTranslator.FromHtml("#FFF");
            s.Styles.Header.Wrap = DefaultBoolean.True;
            s.Styles.Header.Font.Size = FontUnit.Parse("12px");
            s.Styles.Header.Font.Bold = true;

            s.SettingsBehavior.AllowEllipsisInText = true;

            s.Styles.FilterRow.Paddings.Padding = Unit.Pixel(5);
            s.Styles.FilterRow.BackColor = ColorTranslator.FromHtml("#f4f4f4");

            s.Styles.FocusedRow.BackColor = redRowBackColor;
            s.Styles.FocusedRow.ForeColor = redRowForeColor;
            s.Styles.FocusedRow.Font.Bold = true;
            s.Styles.FocusedRow.Font.Size = FontUnit.Parse("13px");
            s.Styles.FocusedGroupRow.BackColor = redRowBackColor;
            s.Styles.FocusedGroupRow.ForeColor = redRowForeColor;

            s.Styles.AlternatingRow.BackColor = ColorTranslator.FromHtml("#F3F4F6");

            s.StylesPager.PageNumber.ForeColor = blackForeColor;
            s.StylesPager.CurrentPageNumber.BackColor = ColorTranslator.FromHtml("#FFF");
            s.StylesPager.CurrentPageNumber.ForeColor = redRowBackColor;

            // Clientside events
            s.ClientSideEvents.CallbackError = "function(s, e) { e.handled = true; }";
            s.ClientSideEvents.Init = string.Concat("function(s,e) { Grid_Init(s, '", modelname, "', ", autoResize ? "true" : "false", "); }");
            
            s.ClientSideEvents.BeginCallback = string.Concat("function(s,e) { Grid_BeginCallback(s, '", modelname, "'); }");
            s.ClientSideEvents.EndCallback = string.Concat("function(s,e) { Grid_EndCallback(s, ", autoResize ? "true" : "false", "); }");

            s.KeyboardSupport = true;

            // Panels
            s.Settings.ShowFilterRow = true;
            s.Settings.ShowGroupPanel = true;
            s.Settings.ShowFilterRowMenu = true;
            s.Settings.ShowFooter = true;
            s.SettingsPager.PageSize = 10;
            s.SettingsPager.NumericButtonCount = 10;
            s.SettingsPager.AllButton.Visible = false;
            s.SettingsPager.Mode = GridViewPagerMode.ShowPager;
            s.SettingsPager.AlwaysShowPager = true;
            s.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            s.Settings.ShowHeaderFilterButton = true;
            s.SettingsPager.PageSizeItemSettings.Visible = true;
            s.SettingsPager.PageSizeItemSettings.Items = new string[] { "10", "25", "50", "100" };

            string layoutKey = modelname + s.Name + version;
            if (!SI.Layout.ContainsKey(layoutKey))
            {
                //delete old layout versions
                List<string> keysToRemove = SI.Layout.Keys.Where(k => k.StartsWith(modelname + s.Name)).ToList();

                foreach (string key in keysToRemove)
                {
                    SI.Layout.Remove(key);
                }

                //create new layout
                SI.Layout.Add(layoutKey, "");
            }

            s.ClientLayout = (si, e) =>
            {
                switch (e.LayoutMode)
                {
                    case ClientLayoutMode.Loading:
                        //Load Last ClientLayout Via First Load
                        if (SI.Layout[layoutKey] != "")
                        {

                            e.LayoutData = SI.Layout[layoutKey];
                        }
                        break;
                    case ClientLayoutMode.Saving:
                        //Save Last ClientLayout Automatically
                        string layoutData = e.LayoutData;
                        List<string> data = layoutData.Split('|').ToList();
                        List<string> removeValues = new List<string>();

                        for (int i = 0; i < data.Count(); i++)
                        {
                            if (data[i].ToLower().Contains("filter"))
                            {
                                // Remove the filter item and the next item which has the filtered values
                                removeValues.Add(data[i]);
                                removeValues.Add(data[i + 1]);
                                i++;
                            }
                            if (data[i].ToLower().StartsWith("page"))
                            {
                                data[i] = "page1";
                            }
                        }
                        string newLayout = "";
                        foreach (string item in data)
                        {
                            newLayout += item + "|";
                        }
                        layoutData = newLayout;
                        foreach (string value in removeValues)
                        {
                            layoutData = layoutData.Replace(value + "|", string.Empty);
                        }

                        SI.Layout[layoutKey] = layoutData;
                        break;
                }
            };

            // Exporter
            s.SettingsExport.Landscape = true;
            s.SettingsExport.DetailHorizontalOffset = 100;
            s.SettingsExport.PreserveGroupRowStates = true;
            s.SettingsExport.Styles.Cell.Font.Name = DefaultFont;
            s.SettingsExport.Styles.Header.Font.Name = DefaultFont;
            s.SettingsExport.Styles.Header.BackColor = s.Styles.Header.BackColor;
            s.SettingsExport.Styles.Header.ForeColor = s.Styles.Header.ForeColor;
            s.SettingsExport.Styles.GroupRow.Font.Name = DefaultFont;
            s.SettingsExport.Styles.GroupFooter.Font.Name = DefaultFont;
            s.SettingsExport.Styles.Footer.Font.Name = DefaultFont;

            // Command column
            s.CommandColumn.Visible = true;
            s.CommandColumn.VisibleIndex = 0;
            if (commandColumnHeader) s.CommandColumn.Caption = GetGridViewExportHeaderTemplate(s.Name, s.SettingsCookies.CookiesID, null);
            s.CommandColumn.Width = Unit.Pixel(80);
            s.CommandColumn.ShowClearFilterButton = true;
            s.CommandColumn.FixedStyle = GridViewColumnFixedStyle.Left;
        }

        public static string GetGridViewExportHeaderTemplate(string name, string cookiesID, string text)
        {
            var template = new StringBuilder();
            template.Append("<table id='grid-column-links' cellpadding='0' cellspacing='0'><tr><td>");
            template.AppendFormat("<input type='submit' name='ExportButton.{0}' title='Export to Excel' class=\"far fa-file-excel command-column-button\" value=\"&#xf1c3\"/>", name);
            template.Append("</td><td style='padding-left: 6px;'>");
            template.AppendFormat("<i class=\"far fa-history command-column-button\" alt='Reset' title='Reset grid layout' onclick=\"ResetGrid('{0}');\" />", cookiesID);
            template.Append("</td>");

            if (string.IsNullOrEmpty(text) == false) template.AppendFormat("<td>{0}</td>", text);            
            
            template.Append("</tr></table>");

            return template.ToString();
        }

        public static void Chart(ChartControlSettings s, string name)
        {
            // General
            s.Name = name;
            s.Width = 1000;
            s.Height = 600;
            s.EmptyChartText.Text = "No data";
            s.EmptyChartText.TextColor = Color.Gray;
            s.EmptyChartText.Font = new Font(s.EmptyChartText.Font.FontFamily.Name, 10F);
            s.Theme = App.Theme;
            s.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
            s.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
            s.Legend.Direction = LegendDirection.LeftToRight;
            s.CrosshairEnabled = DefaultBoolean.False;
            s.PaletteName = "Mixed";
        }

        public static void NavBar(NavBarSettings s, string name)
        {
            s.Name = name;    
            s.Width = Unit.Percentage(100);
            s.ControlStyle.CssClass = "dx-navbar";
            s.Styles.GroupHeader.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            s.Styles.Item.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            s.Styles.Item.HoverStyle.BackColor = GlobalBaseColor;
            s.Styles.Item.SelectedStyle.BackColor = GlobalBaseColor;
            s.Images.Collapse.Url = "~/Assets/img/chevron-up-solid.svg";
            s.Images.Collapse.Height = 18;
            s.Images.Collapse.Width = 18;
            s.Images.Expand.Url = "~/Assets/img/chevron-down-solid.svg";
            s.Images.Expand.Height = 18;
            s.Images.Expand.Width = 18;              
        }

        public static void UploadControl(UploadControlSettings s, string name)
        {            
            s.Name = name;
            s.ShowProgressPanel = true;            
            s.Width = Unit.Pixel(DefaultWidth);
            s.Styles.BrowseButton.CssClass = "dx-controls-default";
        }
    }
}