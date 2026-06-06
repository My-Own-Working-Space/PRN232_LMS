using System.Runtime.Serialization;
using System.Dynamic;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Models
{
    [KnownType(typeof(PagedResult<object>))]
    [KnownType(typeof(ExpandoObject))]
    [KnownType(typeof(LoginResponse))]
    [KnownType(typeof(StudentModel))]
    [KnownType(typeof(EnrollModel))]
    [KnownType(typeof(CourseModel))]
    [KnownType(typeof(SemesterModel))]
    [KnownType(typeof(SubjectModel))]
    [KnownType(typeof(CourseSubjectModel))]
    [KnownType(typeof(GradeModel))]
    [KnownType(typeof(List<StudentModel>))]
    [KnownType(typeof(List<EnrollModel>))]
    [KnownType(typeof(List<CourseModel>))]
    [KnownType(typeof(List<SemesterModel>))]
    [KnownType(typeof(List<SubjectModel>))]
    [KnownType(typeof(List<CourseSubjectModel>))]
    [KnownType(typeof(List<GradeModel>))]
    [KnownType(typeof(List<object>))]
    [KnownType(typeof(string[]))]
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public object? Errors { get; set; }
    }
}
