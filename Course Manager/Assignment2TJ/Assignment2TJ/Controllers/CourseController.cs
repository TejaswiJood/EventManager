using Assignment2TJ.Entities;
using Assignment2TJ.Models;
using Assignment2TJ.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment2TJ.Controllers
{
	
	public class CourseController : AbstractBaseController
	{
		private readonly ICourseManagerService _courseManagerService;
		public CourseController(ICourseManagerService courseManagerService)
		{
			_courseManagerService = courseManagerService;
		}

		[HttpGet("/courses")]
		public IActionResult List()
		{
			Welcome();

			var coursesViewModel = new CoursesViewModel()
			{
				Courses = _courseManagerService.GetAllCourses(),
			};

			return View(coursesViewModel);
		}

		[HttpGet("/courses/{id:int}")]
		public IActionResult Manage(int id)
		{
			Welcome();

			var program = _courseManagerService.GetCourseById(id);

			if (program == null)
				return NotFound();


			var manageCourseViewModel = new ManageCourseViewModel()
			{
				Course = program,
				Student = new Student(),
				CountConfirmationNotSent = program.Students.Count(g => g.Status == EnrollmentConfirmationStatus.ConfirmationNotSent),
				CountConfirmationSent = program.Students.Count(g => g.Status == EnrollmentConfirmationStatus.ConfirmationSent),
				CountEnrollmentConfirmed = program.Students.Count(g => g.Status == EnrollmentConfirmationStatus.EnrollmentConfirmed),
				CountEnrollmentDeclined = program.Students.Count(g => g.Status == EnrollmentConfirmationStatus.EnrollmentDeclined)
			};
			return View(manageCourseViewModel);
		}

		[HttpGet("/courses/add")]
		public IActionResult Add()
		{
			Welcome();
			var courseViewModel = new CourseViewModel()
			{
				Course = new Course()
			};
			return View(courseViewModel);
		}

		[HttpPost("/courses/add")]
		public IActionResult Add(CourseViewModel model)
		{
			Welcome();
			if (!ModelState.IsValid) return View(model);

			_courseManagerService.AddCourse(model.Course);

			TempData["notify"] = $"{model.Course.CourseName} added successfully!";
			TempData["className"] = "success";

			return RedirectToAction("Manage", new { id = model.Course.CourseId });
		}

		[HttpGet("/courses/{id:int}/edit")]
		public IActionResult Edit(int id)
		{
			Welcome();
			var program = _courseManagerService.GetCourseById(id);
			if (program == null)
				return NotFound();

			var courseViewModel = new CourseViewModel()
			{
				Course = _courseManagerService.GetCourseById(id)
			};
			return View(courseViewModel);
		}

		[HttpPost("/courses/{id:int}/edit")]
		public IActionResult Edit(int id, CourseViewModel courseViewModel)
		{
			Welcome();
			if (!ModelState.IsValid)
				return View(courseViewModel);

			_courseManagerService.UpdateCourse(courseViewModel.Course);

			TempData["notify"] = $"{courseViewModel.Course.CourseName} updated successfully!";
			TempData["className"] = "info";

			return RedirectToAction("Manage", new { id });
		}

		[HttpGet("/thank-you/{response}")]
		public IActionResult ThankYou(string response)
		{
			Welcome();
			return View("ThankYou", response);
		}


		[HttpGet("/courses/{courseId:int}/enroll/{studentId:int}")]
		public IActionResult Enroll(int courseId, int studentId)
		{
			Welcome();
			var student = _courseManagerService.GetStudentById(courseId, studentId);
			if (student == null) { return NotFound(); }


			var enrollViewModel = new EnrollViewModel()
			{
				Student = student
			};
			return View(enrollViewModel);

		}

		[HttpPost("/courses/{courseId:int}/enroll/{studentId:int}")]
		public IActionResult Enroll(int courseId, int studentId, EnrollViewModel enrollViewModel)
		{
			Welcome();
			var student = _courseManagerService.GetStudentById(courseId, studentId);
			if (student == null) { return NotFound(); }
			if (ModelState.IsValid)
			{
				var status = enrollViewModel.Response == "yes" ? EnrollmentConfirmationStatus.EnrollmentConfirmed : EnrollmentConfirmationStatus.EnrollmentDeclined;

				_courseManagerService.UpdateConfirmationStatus(courseId, studentId, status);

				return RedirectToAction("ThankYou", new { response = enrollViewModel.Response });
			}
			else
			{
				enrollViewModel.Student = student;
				return View(enrollViewModel);
			}


		}

		[HttpPost("/courses/{courseId:int}/AddStudent")]
		public IActionResult AddStudent(int courseId, ManageCourseViewModel manageViewModel)
		{
			Welcome();
			Course? program;
			if (ModelState.IsValid)
			{
				program = _courseManagerService.AddStudentToCourseById(courseId, manageViewModel.Student);
				if (program == null) { return NotFound(); }
				TempData["notify"] = $"{manageViewModel.Student.StudentName} added to Student list!";
				TempData["className"] = "success";
				return RedirectToAction("Manage", new { id = courseId });
			}
			else
			{
				program = _courseManagerService.GetCourseById(courseId);
				if (program == null) { return NotFound(); }
				manageViewModel.Course = program;
				manageViewModel.CountConfirmationNotSent = program.Students.Count(g => g.Status == EnrollmentConfirmationStatus.ConfirmationNotSent);
				manageViewModel.CountConfirmationSent = program.Students.Count(g => g.Status == EnrollmentConfirmationStatus.ConfirmationSent);
				manageViewModel.CountEnrollmentConfirmed = program.Students.Count(g => g.Status == EnrollmentConfirmationStatus.EnrollmentConfirmed);
				manageViewModel.CountEnrollmentDeclined = program.Students.Count(g => g.Status == EnrollmentConfirmationStatus.EnrollmentDeclined);

				return View("Manage", manageViewModel);
			}
		}

		[HttpPost("/courses/{courseId:int}/enroll")]
		public IActionResult SendInvitation(int courseId)
		{
			_courseManagerService.SendEnrollmentEmailByCourseId(courseId, Request.Scheme, Request.Host.ToString());
			return RedirectToAction("Manage", new { id = courseId });
		}
	}
}
