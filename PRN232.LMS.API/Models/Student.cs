namespace PRN232.LMS.API.Models
{
    public class StudentRequest
    {
        public string? Search { get; set; }
        public string? Sort { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? Fields { get; set; }
        public string? Expand { get; set; }


    }
    public class StudentResponse
    {
        public bool success { get; set; } = false;
        public string message { get; set; }
        public object? data { get; set; } = null;
        public string? errors { get; set; } = null;
    }
}
