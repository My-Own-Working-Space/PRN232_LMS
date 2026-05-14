using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Services.Models
{
    public class EnrollModel
    {
        public int EnrollmentId { get; set; }

        public Student Student { get; set; }

        public int CourseId { get; set; }

        public DateTime EnrollDate { get; set; }

        public string Status { get; set; }

        public Course Course { get; set; }

        public List<Grade> Grades { get; set; } = new List<Grade>();

        public Subject Subject { get; set; }

        public Semester Semester { get; set; }

    }
}
