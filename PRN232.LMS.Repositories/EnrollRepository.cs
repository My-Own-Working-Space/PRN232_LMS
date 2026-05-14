using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories
{
    public class EnrollRepository(LMSDatabaseContext _context) : IEnrollRepository
    {
        public List<Enrollment> GetEnrolls()
        {
            return _context.Enrollments
                .Include(e=>e.Student)
                .Include(e=>e.Course)
                    .ThenInclude(c => c.Semester)
                .Include(e => e.Grades)
                    .ThenInclude(g => g.Subject)
                .ToList();
        }
    }
}
