using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.LMS.Services.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }

        public string? Email { get; set; }

        public DateTime DateOfBirth { get; set; }

    }
}
