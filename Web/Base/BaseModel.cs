using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web;
using LeadingEdge.Curator.Core;
using DevExpress.Web.Mvc;
using System.Web.UI.WebControls;
using Cortex.Utilities;
using System.Drawing;

namespace LeadingEdge.Curator.Web
{
    [ModelBinder(typeof(DevExpressEditorsBinder))]
    public class BaseModel
    {
        public string Name { get { return GetType().Name; } }
        public string PageID { get; set; }
        public string Area { get; private set; }
        public string Controller { get; private set; }
        public string Action { get; private set; }
        public string AreaClassName => $"section--{Area}";
        public string ControllerClassName => $"group--{Area}-{Controller}";
        public string PageClassName => $"page--{Area}-{Controller}-{Action}";
        public FormInfo Form { get; set; }
        public bool IsPostback { get; private set; }
        public string ModelID { get { return U.GetModelID(GetType().Name, PageID); } }
        public Exception Exception { get; set; }
        public bool Success { get; set; }
        public bool HideMenu { get; set; }
        public Dictionary<string, GridModel> Grids { get; set; }
        public Dictionary<string, ChartModel> Charts { get; set; }
        public NavBarSettings NavBarSettings { get; set; }
        public PopupControlSettings DetailsPopupControlSettings { get; set; }
        public PopupControlSettings RegionsPopupControlSettings { get; set; }
        public PopupControlSettings BookmarksPopupControlSettings { get; set; }
        public string TranslatedName
        {
            get
            {
                if (Form == null) return App.PlatformName;

                return Form.Name;
            }
        }
        public SessionInfo SI
        {
            get { return (SessionInfo)HttpContext.Current.Session["sSessionInfo"]; }
            set { HttpContext.Current.Session["sSessionInfo"] = value; }
        }


        public BaseModel()
        {
            // Page level security
            var context = HttpContext.Current.Request.RequestContext;
            Area = Convert.ToString(context.RouteData.Values["area"]).ToLower();
            Controller = Convert.ToString(context.RouteData.Values["controller"]).ToLower();
            Action = Convert.ToString(context.RouteData.Values["action"]).ToLower();

            if (SI != null)
            {
                // Create a PageID if there isn't one already submitted in the form.
                if (HttpContext.Current.Request.Form["PageID"] == null)
                {
                    SI.LastPageID++;
                    PageID = Convert.ToString(SI.LastPageID);
                }

                bool defaultpages = ((Area == "" && Controller == "" && Action == "") ||
                    (Area == "home" && Controller == "home" && Action == "login") ||
                    (Area == "home" && Controller == "home" && Action == "logout") ||
                    (Area == "asset" && Controller == "movement" && Action == "login") ||
                    (Area == "device" && Controller == "admin" && Action == "login")); // Login

                if (SI != null && SI.User != null) // Once logged in
                {
                    if (!defaultpages) defaultpages = context.HttpContext.Request.IsAjaxRequest(); // All Ajax requests
                    if (!defaultpages) defaultpages = Area == "home"; // Home area
                    if (!defaultpages) defaultpages = (Area == "reporting" && Action == "index"); // Reporting page EDIT FOR CLIENT
                    if (!defaultpages) defaultpages = (Area == "admin" && Action == "index"); // Admin page
                    if (!defaultpages) defaultpages = (Action.ToLower().Contains("upload")); // All upload requests
                }

                if (false && !defaultpages) // then check security
                {
                    var form = SI.PermittedForms.Find(x => x.Area.ToLower() == Area.ToLower() && x.Controller.ToLower() == Controller.ToLower() && x.Action.ToLower() == Action.ToLower());
                    if (form == null)
                    {
                        throw new HttpException(403, "You are not permitted to view this page.");
                    }

                    Form = form;
                }
                else
                {
                    Form = FormManager.GetForm(Area, Controller, Action);
                }

                App.KeepAlive(SI, ModelID);
            }

            IsPostback = HttpContext.Current.Request.HttpMethod == "POST";
            Grids = new Dictionary<string, GridModel>();
            Charts = new Dictionary<string, ChartModel>();
            DetailsPopupControlSettings = GetAccountDetailsPopupControlSettings();
            RegionsPopupControlSettings = GetAccountRegionsPopupControlSettings();
            BookmarksPopupControlSettings = GetBookmarksPopupControlSettings();
            NavBarSettings = GetNavBarSettings();
        }

        public string GetInstanceID(string objectname)
        {
            string modelname = GetType().Name;

            return String.Format("{0}_{1}_{2}", modelname, objectname, PageID);
        }

        public string MenuLink(string cssClass, string href, string area, string text, bool openInNewTab, string iconClass)
        {
            bool isSelected = false;

            if (area.In("Home", "Logout", "Bookmarks", "Help", "Contact") == false && SI.PermittedForms.Exists(x => x.Area == area) == false) return null;

            if (area.ToLower() == Area) isSelected = true;

            string target = string.Empty;
            if (openInNewTab) target = " target=\"_blank\"";

            return String.Format("<div class=\"{0} menu-item{1}\"><a href=\"{2}\"{4}><i class=\"{5}\"></i> <span>{3}</span></a></div>", cssClass, (isSelected ? " selected" : String.Empty), href, text, target, iconClass);
        }

        public NavBarSettings GetNavBarSettings()
        {
            NavBarSettings settings = new NavBarSettings();
            Setup.NavBar(settings, "NavMain");
            
            AddNavBarGroup(settings.Groups, "Home", "/Home", "fa-light fa-house-chimney", 200001);
            AddNavBarGroup(settings.Groups, "Orders", null, "fa-light fa-cart-shopping", 201002);
            AddNavBarGroup(settings.Groups, "Products", null, "fa-light fa-cubes-stacked", 200010);
            AddNavBarGroup(settings.Groups, "Admin", null, "fa-light fa-screwdriver-wrench", 200002);
            //AddNavBarGroup(settings.Groups, "Bookmarks", null, "fa-light fa-stars", 200005);
            //AddNavBarGroup(settings.Groups, "Help", "/Help", "fa-light fa-circle-question", 200006);
            //AddNavBarGroup(settings.Groups, "Contact", "/Contact", "fa-light fa-comments-question", 300097);

            return settings;
        }

        internal void AddNavBarGroup(MVCxNavBarGroupCollection groups, string area, string href, string iconClass, int groupPlaceholder)
        {
            var forms = SI.PermittedForms.FindAll(x => x.Area == area && x.Action != "Index" && x.DisplayOrder >= 0).OrderBy(x => x.DisplayOrder);
            if (forms.Count() == 0 && area.In("Home", "Logout", "Bookmarks", "Help", "Contact") == false) return;

            var group = new MVCxNavBarGroup();
            group.Name = $"menu-{area}";
            group.HeaderStyle.CssClass = "dx-navbar-group-header";

            if (href != null)
            {
                group.NavigateUrl = href;
                group.HeaderStyle.HoverStyle.BackColor = ColorTranslator.FromHtml(App.GlobalThemeBaseColor);       
            }

            string template;
            if (href == null)
            {
                template = $"<i class=\"{iconClass} dx-navbar-icon\"></i><span>{LabelText(groupPlaceholder)}</span>";
            }
            else
            {
                template = $"<div class=\"dx-navbar-group-header-selectable\"><a href=\"{href}\"><i class=\"{iconClass} dx-navbar-icon\"></i><span>{LabelText(groupPlaceholder)}</span></a></div>";
            }

            group.SetHeaderTemplateContent(template);


            foreach (var form in forms)
            {
                var item = new MVCxNavBarItem();   
                string url = $"/{form.Area}/{form.Controller}/{form.Action}";    
                item.NavigateUrl = url;
                item.SetTextTemplateContent($"<a href=\"{url}\"><span class=\"dx-navbar-item\">{LabelText(form.PlaceholderID)}</span></a>");
                
                group.Items.Add(item);
            }

            groups.Add(group);
        }


        public string Label(int placeholderID)
        {
            var label = App.GetLabel(SI.User.CompanyID, SI.Language.LanguageID, placeholderID);

            return label.Format(SI.LabelEditMode);
        }

        public string LabelText(int placeholderID)
        {
            var label = App.GetLabel(SI.User.CompanyID, SI.Language.LanguageID, placeholderID);

            return label.LabelText;
        }

        public bool IsPermittedArea(string area)
        {
            return SI.PermittedForms.Exists(x => x.Area == area);
        }

        public bool IsPermittedFormGroup(string area, string formGroup)
        {
            return SI.PermittedForms.Exists(x => x.Area == area && x.Group == formGroup);
        }

        public string RenderLinks(string area, string formGroup)
        {
            string links = String.Empty;

            foreach (FormInfo form in SI.PermittedForms.FindAll(x => x.Area == area && x.Group == formGroup).OrderBy(x => x.DisplayOrder))
            {
                if (form.DisplayOrder < 0) continue;    // Don't display 

                bool exists = SI.Bookmarks.Exists(x => x.FormID == form.FormID);

                links += String.Format
                (
                    "<li><a href=\"/{0}/{1}/{2}/{3}\" target=\"_self\">{4}</a><a class=\"q-link {5}\" href=\"{6}\"></a></li>",
                    form.Area, form.Controller, form.Action, form.Parameters, form.Name, (exists ? "remove" : "add"), form.FormID
                );
            }

            return links;
        }

        public bool HasPermission(string id)
        {
            return U.HasPermission(SI, id);
        }

        public PopupControlSettings GetAccountDetailsPopupControlSettings()
        {
            PopupControlSettings settings = new PopupControlSettings();
            Setup.PopupControl(settings);
            settings.Name = "ppAccountDetails";
            settings.HeaderText = "Account options";
            settings.ShowHeader = false;
            settings.ControlStyle.CssClass = "cortex-popup";
            settings.Width = Unit.Pixel(384);
            settings.ControlStyle.HorizontalAlign = HorizontalAlign.Left;
            settings.CallbackRouteValues = new { area = "Home", controller = "Home", action = "Details" };
            settings.ClientSideEvents.EndCallback = "function(s,e) { ppAccountDetails.Show(); }";
            return settings;
        }

        public PopupControlSettings GetAccountRegionsPopupControlSettings()
        {
            PopupControlSettings settings = new PopupControlSettings();
            Setup.PopupControl(settings);
            settings.Name = "ppAccountRegions";
            settings.HeaderText = "Account regions";
            settings.ShowHeader = false;
            settings.ControlStyle.CssClass = "cortex-popup";
            settings.Width = Unit.Pixel(325);
            settings.ControlStyle.HorizontalAlign = HorizontalAlign.Left;
            settings.CallbackRouteValues = new { area = "Home", controller = "Home", action = "Regions" };
            settings.ClientSideEvents.EndCallback = "function(s,e) { ppAccountRegions.Show(); }";
            return settings;
        }

        public PopupControlSettings GetBookmarksPopupControlSettings()
        {
            PopupControlSettings settings = new PopupControlSettings();
            Setup.PopupControl(settings);
            settings.Name = "ppBookmarks";
            settings.HeaderText = "Bookmarks";
            settings.ShowHeader = false;
            settings.Modal = false;
            settings.ControlStyle.CssClass = "cortex-popup";
            settings.ControlStyle.HorizontalAlign = HorizontalAlign.Left;
            settings.CallbackRouteValues = new { area = "Home", controller = "Home", action = "Bookmarks" };
            settings.ClientSideEvents.EndCallback = "function(s,e) { ppBookmarks.Show(); }";
            settings.CloseAction = CloseAction.OuterMouseClick;
            //settings.PopupHorizontalAlign = PopupHorizontalAlign.RightSides;
            settings.PopupVerticalAlign = PopupVerticalAlign.Above;
            //settings.PopupVerticalOffset = 100;
            return settings;
        }

    }
}
