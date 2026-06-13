using System;
using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.API.Models.Requests
{
    public class CreateStudentRequest
    {
        [Required(ErrorMessage = "FullName is required")]
        [StringLength(100, ErrorMessage = "FullName cannot exceed 100 characters")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "DateOfBirth is required")]
        public DateTime DateOfBirth { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Phone number must be between 10 and 11 digits")]
        public string? Phone { get; set; }

        [FptuCode(ErrorMessage = "StudentCode must follow FPTU style (e.g., SE193418)")]
        public string? StudentCode { get; set; }
    }

    public class CreateCourseRequest
    {
        [Required(ErrorMessage = "CourseName is required")]
        [StringLength(100, ErrorMessage = "CourseName cannot exceed 100 characters")]
        public string CourseName { get; set; } = string.Empty;

        [Required(ErrorMessage = "SemesterId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "SemesterId must be greater than 0")]
        public int SemesterId { get; set; }
    }

    public class CreateEnrollmentRequest
    {
        [Required(ErrorMessage = "StudentId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "StudentId must be greater than 0")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "CourseId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CourseId must be greater than 0")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Active";
    }

    public class CreateSemesterRequest
    {
        [Required(ErrorMessage = "SemesterName is required")]
        [StringLength(50, ErrorMessage = "SemesterName cannot exceed 50 characters")]
        public string SemesterName { get; set; } = string.Empty;

        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required")]
        public DateTime EndDate { get; set; }
    }

    public class CreateSubjectRequest
    {
        [Required(ErrorMessage = "SubjectCode is required")]
        [StringLength(10, ErrorMessage = "SubjectCode cannot exceed 10 characters")]
        public string SubjectCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "SubjectName is required")]
        [StringLength(100, ErrorMessage = "SubjectName cannot exceed 100 characters")]
        public string SubjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Credit is required")]
        [Range(1, 10, ErrorMessage = "Credit must be between 1 and 10")]
        public int Credit { get; set; }
    }

    public class CreateCourseSubjectRequest
    {
        [Required(ErrorMessage = "CourseId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CourseId must be greater than 0")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "SubjectId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "SubjectId must be greater than 0")]
        public int SubjectId { get; set; }
    }

    public class CreateGradeRequest
    {
        [Required(ErrorMessage = "EnrollmentId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "EnrollmentId must be greater than 0")]
        public int EnrollmentId { get; set; }

        [Required(ErrorMessage = "SubjectId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "SubjectId must be greater than 0")]
        public int SubjectId { get; set; }

        [Range(0.0, 10.0, ErrorMessage = "MidtermScore must be between 0.0 and 10.0")]
        public decimal? MidtermScore { get; set; }

        [Range(0.0, 10.0, ErrorMessage = "FinalScore must be between 0.0 and 10.0")]
        public decimal? FinalScore { get; set; }

        [Range(0.0, 10.0, ErrorMessage = "TotalScore must be between 0.0 and 10.0")]
        public decimal? TotalScore { get; set; }

        [StringLength(2, ErrorMessage = "LetterGrade cannot exceed 2 characters")]
        public string? LetterGrade { get; set; }
    }
}
