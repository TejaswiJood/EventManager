using Assignment2TJ.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment2TJ.Entities
{
	public class CoursesManagerDbContext : DbContext
	{
		public CoursesManagerDbContext(DbContextOptions<CoursesManagerDbContext> options) : base(options) { }


		public DbSet<Course> Courses { get; set; }
		public DbSet<Student> Students { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Student>()
				.Property(student => student.Status)
				.HasConversion<string>().HasMaxLength(64);

			modelBuilder.Entity<Course>().HasData(
			new Course
			{
				CourseId = 1,
				CourseName = "Programming Microsoft Web Technologies",
				CourseInstructor = "Peter Madziak",
				StartDate = new DateTime(2024, 1, 18),
				RoomNumber = "1C09"
			});

			modelBuilder.Entity<Student>().HasData(
				new Student
				{
					StudentId = 1,
					StudentName = "Bart Simpson",
					StudentEmail = "bartsimpson@gmail.com",
					CourseId = 1
				});
		}
	}
}
