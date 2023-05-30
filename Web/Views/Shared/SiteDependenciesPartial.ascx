<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script type="text/javascript">var ModelID = '<%=Model.ModelID %>';</script>

<script type="text/javascript" src="/Assets/Lib/jquery/dist/jquery.min.js"></script>
<script type="text/javascript" src="/Assets/Lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
<script type="text/javascript" src="/Assets/Js/site.js?<%= App.Version %>"></script>
<script data-jsd-embedded data-key="9414b0f4-b5e1-4234-8ec0-142860572807" data-base-url=https://jsd-widget.atlassian.com src=https://jsd-widget.atlassian.com/assets/embed.js></script>
<%
Html.DevExpress().RenderStyleSheets(Page,
    new StyleSheet { ExtensionSuite = ExtensionSuite.NavigationAndLayout, Theme = App.Theme },
    new StyleSheet { ExtensionSuite = ExtensionSuite.Editors, Theme = App.Theme },
    new StyleSheet { ExtensionSuite = ExtensionSuite.HtmlEditor, Theme = App.Theme },
    new StyleSheet { ExtensionSuite = ExtensionSuite.GridView, Theme = App.Theme },
    new StyleSheet { ExtensionSuite = ExtensionSuite.PivotGrid, Theme = App.Theme },
    new StyleSheet { ExtensionSuite = ExtensionSuite.Chart, Theme = App.Theme },
    new StyleSheet { ExtensionSuite = ExtensionSuite.Scheduler, Theme = App.Theme });
Html.DevExpress().RenderScripts(Page,
    new Script { ExtensionSuite = ExtensionSuite.NavigationAndLayout },
    new Script { ExtensionSuite = ExtensionSuite.Editors },
    new Script { ExtensionSuite = ExtensionSuite.HtmlEditor },
    new Script { ExtensionSuite = ExtensionSuite.GridView },
    new Script { ExtensionSuite = ExtensionSuite.PivotGrid },
    new Script { ExtensionSuite = ExtensionSuite.Chart },
    new Script { ExtensionSuite = ExtensionSuite.Scheduler });
%>