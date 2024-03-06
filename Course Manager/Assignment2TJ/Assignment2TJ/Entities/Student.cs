using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Assignment2TJ.Entities
{
	public enum EnrollmentConfirmationStatus
	{
		ConfirmationNotSent = 0,
		ConfirmationSent = 1,
		EnrollmentConfirmed = 2,
		EnrollmentDeclined = 3
	}
	public class Student
	{
		public int StudentId { get; set; }

		[Required(ErrorMessage = "Student name is required")]
		public string? StudentName { get; set; }

		[Required(ErrorMessage = "Student email is required to send invitation")]
		[EmailAddress(ErrorMessage = "Must be a valid email")]
		public string? StudentEmail { get; set; }

		public EnrollmentConfirmationStatus Status { get; set; } = EnrollmentConfirmationStatus.ConfirmationNotSent;

		public int CourseId { get; set; }
		public Course? Course { get; set; }
	}
}
