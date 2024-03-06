using Microsoft.AspNetCore.Mvc;

namespace Assignment2TJ.Controllers
{
	public class AbstractBaseController : Controller
	{
		public void Welcome()
		{
			const string cookieName = "firstVisitDate";

			var firstVisitDate = HttpContext.Request.Cookies.ContainsKey(cookieName) &&
				DateTime.TryParse(HttpContext.Request.Cookies[cookieName], out var parsedDate) ?
				parsedDate : DateTime.Now;

			var welcomeMessage = HttpContext.Request.Cookies.ContainsKey(cookieName)
				? $"Welcome Back? You first visited this app on {firstVisitDate.ToShortDateString()}"
				: "Hey welcome to the Course Manager!";

			var cookieOptions = new CookieOptions
			{
				Expires = DateTime.Now.AddDays(30)
			};

			HttpContext.Response.Cookies.Append(cookieName,
				firstVisitDate.ToString(),
				cookieOptions);

			ViewData["WelcomeMessage"] = welcomeMessage;
		}
	}
}
