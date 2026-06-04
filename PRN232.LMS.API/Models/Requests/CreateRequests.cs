namespace PRN232.LMS.API.Models.Requests
{
    public class CreateStudentRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }

    public class CreateCourseRequest
    {
        public string CourseName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
    }

    public class CreateEnrollmentRequest
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string Status { get; set; } = "Active";
    }

    public class CreateSemesterRequest
    {
        public string SemesterName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CreateSubjectRequest
    {
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int Credit { get; set; }
    }

    public class CreateCourseSubjectRequest
    {
        public int CourseId { get; set; }
        public int SubjectId { get; set; }
    }

    public class CreateGradeRequest
    {
        public int EnrollmentId { get; set; }
        public int SubjectId { get; set; }
        public decimal? MidtermScore { get; set; }
        public decimal? FinalScore { get; set; }
        public decimal? TotalScore { get; set; }
        public string? LetterGrade { get; set; }
    }
}
