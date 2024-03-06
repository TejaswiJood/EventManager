using Assignment2TJ.Entities;

namespace Assignment2TJ.Services
{
	public interface ICourseManagerService
	{
		public List<Course>? GetAllCourses();
		public Course? GetCourseById(int id);

		public int AddCourse(Course program);
		public void UpdateCourse(Course program);
		public Student? GetStudentById(int courseid, int studentId);
		public void UpdateConfirmationStatus(int courseid, int studentId, EnrollmentConfirmationStatus status);

		public Course? AddStudentToCourseById(int courseId, Student student);
		public void SendEnrollmentEmailByCourseId(int courseId, string scheme, string host);

	}
}
