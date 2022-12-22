using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace LeadingEdge.Curator.Core
{
	public static class ControllerExtensions
	{
		/// <summary>
		/// Captures the HTML output by a controller action that returns a ViewResult
		/// </summary>
		/// <typeparam name="TController">The type of controller to execute the action on</typeparam>
		/// <param name="controller">The controller</param>
		/// <param name="action">The action to execute</param>
		/// <returns>The HTML output from the view</returns>
		public static string CaptureActionHtml<TController>(this TController controller, Func<TController, ViewResult> action) where TController : Controller
		{
			return controller.CaptureActionHtml(controller, null, action);
		}

		/// <summary>
		/// Captures the HTML output by a controller action that returns a ViewResult
		/// </summary>
		/// <typeparam name="TController">The type of controller to execute the action on</typeparam>
		/// <param name="controller">The controller</param>
		/// <param name="masterPageName">The master page to use for the view</param>
		/// <param name="action">The action to execute</param>
		/// <returns>The HTML output from the view</returns>
		public static string CaptureActionHtml<TController>(this TController controller, string masterPageName, Func<TController, ViewResult> action) where TController : Controller
		{
			return controller.CaptureActionHtml(controller, masterPageName, action);
		}

		/// <summary>
		/// Captures the HTML output by a controller action that returns a ViewResult
		/// </summary>
		/// <typeparam name="TController">The type of controller to execute the action on</typeparam>
		/// <param name="controller">The current controller</param>
		/// <param name="targetController">The controller which has the action to execute</param>
		/// <param name="action">The action to execute</param>
		/// <returns>The HTML output from the view</returns>
		public static string CaptureActionHtml<TController>(this Controller controller, TController targetController, Func<TController, ViewResult> action) where TController : Controller
		{
			return controller.CaptureActionHtml(targetController, null, action);
		}

		/// <summary>
		/// Captures the HTML output by a controller action that returns a ViewResult
		/// </summary>
		/// <typeparam name="TController">The type of controller to execute the action on</typeparam>
		/// <param name="controller">The current controller</param>
		/// <param name="targetController">The controller which has the action to execute</param>
		/// <param name="masterPageName">The name of the master page for the view</param>
		/// <param name="action">The action to execute</param>
		/// <returns>The HTML output from the view</returns>
		public static string CaptureActionHtml<TController>(this Controller controller, TController targetController, string masterPageName, Func<TController, ViewResult> action) where TController : Controller
		{
			if (controller == null) throw new ArgumentNullException("controller");
			if (controller.ControllerContext == null) throw new ArgumentNullException("controller.ControllerContext");
			if (targetController == null) throw new ArgumentNullException("targetController");
			if (action == null) throw new ArgumentNullException("action");

			ControllerContext controllerContext = controller.ControllerContext;
			object oldController = null;
			HttpContext existingContext = null;

			try
			{
				// pass the current controller context to the executing controller
				targetController.ControllerContext = controllerContext;

				// replace the current context with a new context that writes to a string writer
				existingContext = HttpContext.Current;

				StringWriter writer = new StringWriter();
				HttpResponse response = new HttpResponse(writer);
				HttpContext context = new HttpContext(existingContext.Request, response) { User = existingContext.User };
				HttpContext.Current = context;

				// we have to set the controller route value to the name of the controller we want to execute
				// because the ViewLocator class uses this to find the correct view
				oldController = controllerContext.RouteData.Values["controller"];
				controllerContext.RouteData.Values["controller"] = typeof(TController).Name.Replace("Controller", "");

				// execute the action
				ViewResult viewResult = action(targetController);

				// change the master page name
				if (masterPageName != null)
				{
					viewResult.MasterName = masterPageName;
				}

				// execute the result
				viewResult.ExecuteResult(controllerContext);

				return writer.ToString();
			}
			finally
			{
				// restore the old route data
				controllerContext.RouteData.Values["controller"] = oldController;

				// restore the old context
				HttpContext.Current = existingContext;
			}
		}
	}
}
