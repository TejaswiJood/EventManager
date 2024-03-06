using Assignment2TJ.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;


namespace Assignment2TJ.Services
{
	public class CourseManagerService : ICourseManagerService
	{
		private readonly IConfiguration _configuration;

		private readonly CoursesManagerDbContext _coursesManagerDbContext;
		public CourseManagerService(CoursesManagerDbContext coursesManagerDbContext, IConfiguration configuration)
		{
			_coursesManagerDbContext = coursesManagerDbContext;
			_configuration = configuration;

		}
		public List<Course>? GetAllCourses()
		{
			return _coursesManagerDbContext.Courses.Include(e => e.Students).OrderByDescending(e => e.StartDate).ToList();
		}

		public Course? GetCourseById(int id)
		{
			return _coursesManagerDbContext.Courses.Include(e => e.Students).FirstOrDefault(e => e.CourseId == id);
		}

		public int AddCourse(Course program)
		{
			_coursesManagerDbContext.Courses.Add(program);
			_coursesManagerDbContext.SaveChanges();

			return program.CourseId;
		}
		public void UpdateCourse(Course program)
		{
			_coursesManagerDbContext.Courses.Update(program);
			_coursesManagerDbContext.SaveChanges();
		}
		public Student? GetStudentById(int courseid, int studentId)
		{
			return _coursesManagerDbContext.Students
				.Include(g => g.Course)
				.FirstOrDefault(g => g.StudentId == studentId && g.CourseId == courseid);
		}
		public void UpdateConfirmationStatus(int courseid, int studentId, EnrollmentConfirmationStatus status)
		{
			var student = GetStudentById(courseid, studentId);
			if (student != null)
			{
				student.Status = status;
				_coursesManagerDbContext.SaveChanges();
			}
		}
		public Course? AddStudentToCourseById(int courseId, Student student)
		{
			var program = GetCourseById(courseId);
			if (program == null) return null;
			program.Students.Add(student);
			_coursesManagerDbContext.SaveChanges();
			return program;
		}
		public void SendEnrollmentEmailByCourseId(int courseId, string scheme, string host)
		{
			var program = GetCourseById(courseId);
			if (program == null) return;
			var students = program.Students
				.Where(g => g.Status == EnrollmentConfirmationStatus.ConfirmationNotSent)
				.ToList();
			try
			{
				var smtpHost = _configuration["SmtpSettings:Host"];
				var smtpPort = _configuration["SmtpSettings:Port"];
				var fromAddress = _configuration["SmtpSettings:FromAddress"];
				var fromPassword = _configuration["SmtpSettings:FromPassword"];

				using var smtpClient = new SmtpClient(smtpHost);
				smtpClient.Port = string.IsNullOrEmpty(smtpPort) ? 587 : Convert.ToInt32(smtpPort);
				smtpClient.Credentials = new NetworkCredential(fromAddress, fromPassword);
				smtpClient.EnableSsl = true;

				foreach (var g in students)
				{
					var responseUrl = $"{scheme}://{host}/events/{courseId}/enroll/{g.StudentId}";
					var mailMessage = new MailMessage
					{
						From = new MailAddress(fromAddress),
						Subject = $"[Action Required] Confirm \"{g?.Course?.CourseName}\" Enrollment",
						Body = CreateBody(g, responseUrl),
						IsBodyHtml = true,
					};
					if (g.StudentEmail == null) return;
					mailMessage.To.Add(g.StudentEmail);
					smtpClient.Send(mailMessage);
					g.Status = EnrollmentConfirmationStatus.ConfirmationSent;
				}
				_coursesManagerDbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		private static string CreateBody(Student student, string responseUrl)
		{
			return $@"
                        <h1>Hello{student.StudentName}: </h1>
                        <p>
                            Your request to enroll in the course {student?.Course?.CourseName}
                            in the room {student?.Course?.RoomNumber} starting {student?.Course?.StartDate:d}
                            with Professor {student?.Course?.CourseInstructor}
                        </p>
                        <a href={responseUrl}>Confirm Your Enrollment</a>
                        <p>The Course Manager App</p>
            ";
		}
	}
}
