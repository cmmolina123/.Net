using Sabio.Models;
using Sabio.Models.Domain.CodeChallenge;
using Sabio.Models.Requests.CodeChallenge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public interface ICourseService
    {
        Course GetCourseById(int id);

        int AddCourse(CourseAddRequest model, int currentUser);

        void UpdateCourse(CourseUpdateRequest model);

        void DeleteStudent(int id);

        Paged<Course> GetCoursesByPage(int pageIndex, int pageSize);
    }
}
