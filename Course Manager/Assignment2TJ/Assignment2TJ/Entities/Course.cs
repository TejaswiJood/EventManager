using System.ComponentModel.DataAnnotations;

namespace Assignment2TJ.Entities
{
	public class Course
	{
		public int CourseId { get; set; }

		[Required(ErrorMessage = "Course name is required")]
		public string? CourseName { get; set; }

		[Required(ErrorMessage = "Couse Instructor name is required")]
		public string? CourseInstructor { get; set; }

		[Required(ErrorMessage = "StartDate is required")]
		public DateTime? StartDate { get; set; }

		[Required(ErrorMessage = "Room number is required")]
		[RegularExpression(@"^\d[A-Z]\d{2}$", ErrorMessage = "Must be in a format (1A11)")]
		public string? RoomNumber { get; set; }

		public List<Student>? Students { get; set; }

	}
}
