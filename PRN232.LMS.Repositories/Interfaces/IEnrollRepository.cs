using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories.Interfaces
{
    public interface IEnrollRepository
    {
        public List<Enrollment> GetEnrolls();
    }
}
