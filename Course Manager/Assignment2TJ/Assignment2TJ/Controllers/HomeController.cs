using Assignment2TJ.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Assignment2TJ.Controllers
{
	public class HomeController : AbstractBaseController
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			Welcome();
			return View();
		}

		public IActionResult Privacy()
		{
			Welcome();
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}