using System;
using System.Web.Http;
using System.Linq;
using System.Collections.Generic;

namespace TWAPI
{
	public static class WebApiConfig
	{

		public static void Register( HttpConfiguration config )
		{
			//any additional config 

			// Web API Routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute (
				name: "TWAPI",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}

