using Assignment2TJ.Entities;

namespace Assignment2TJ.Models
{
	public class ManageCourseViewModel
	{
		public Course? Course { get; set; }
		public Student? Student { get; set; }

		public int CountConfirmationNotSent { get; set; }
		public int CountConfirmationSent { get; set; }
		public int CountEnrollmentConfirmed { get; set; }
		public int CountEnrollmentDeclined { get; set; }

	}
}
