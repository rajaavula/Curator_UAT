<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HomeBookmarks>" %>
<div id="bookmarks-container">
<%
	if (Model.SI.Bookmarks != null && Model.SI.Bookmarks.Count > 0)
	{
		if (Model != null && Model.SI.Bookmarks.Count > 0)
		{
			List<string> distinctAreas = Model.SI.Bookmarks.Select(x => x.Area).Distinct().ToList();

			foreach (string area in distinctAreas)
			{
				%><div class="section">
					<h3><%= area.ToUpper() %></h3>
					<div class="content">
						<ul><%
						foreach (FormInfo p in Model.SI.Bookmarks.Where(x => x.Area == area))
						{
							%><li>
								<a href="<%= String.Format("/{0}/{1}/{2}/{3}", p.Area, p.Controller, p.Action, p.Parameters) %>" ><%= Model.Label(p.PlaceholderID) %></a>
							</li><%
						}
						%></ul><%
					%></div><%
				%></div><%
			}
		}

		%><div class="clear"></div><%
	}
	else
	{
		%><p class="nowrap">None available.</p><%
	}
%>
</div>