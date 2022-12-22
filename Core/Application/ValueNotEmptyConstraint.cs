using System;
using System.Web;
using System.Web.Routing;

namespace LeadingEdge.Curator.Core
{
	public class ValueNotEmptyConstraint : IRouteConstraint
	{
		public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
		{
			string strValue = Convert.ToString(values[parameterName]);
			return (String.IsNullOrEmpty(strValue) == false);
		}
	}
}
