using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.CodeChallenge;
using Sabio.Models.Requests.CodeChallenge;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class CourseService : ICourseService
    {
        IDataProvider _data = null;

        public CourseService(IDataProvider data)
        {
            _data = data;
        }

        public Course GetCourseById(int id)
        {
            string procName = "[dbo].[Courses_SelectById]";

            Course clase = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);


            }, delegate (IDataReader reader, short set)
            {
                int startingIdex = 0;
                clase = MapSingleCourse(reader, ref startingIdex);

            }

            );

            return clase;

        }
        
        public void UpdateCourse(CourseUpdateRequest model)
        {
            string procName = "[dbo].[Courses_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@Id", model.Id);
                },
                returnParameters: null);

        }

        public int AddCourse(CourseAddRequest model, int currentUser)
        {
            int id = 0;

            string procName = "[dbo].[Courses_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);

                    // col.AddWithValue("@UserId", currentUser);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);

                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;

                    int.TryParse(oId.ToString(), out id);

                    Console.WriteLine("");
                });
            return id;

        }

        public void DeleteStudent(int id)
        {
            string procName = "[dbo].[Students_Delete]";
            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);

            }, returnParameters: null);
        }

        public Paged<Course> GetCoursesByPage(int pageIndex, int pageSize)
        {
            Paged<Course> pagedResult = null;
            List<Course> result = null;

            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[Courses_Pagination]",
            inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@PageIndex", pageIndex);
                parameterCollection.AddWithValue("@PageSize", pageSize);

            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIdex = 0;
                Course clase = MapSingleCourse(reader, ref startingIdex);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIdex++);
                }

                if (result == null)
                {
                    result = new List<Course>();
                }
                result.Add(clase);
            });

            if (result != null)
            {
                pagedResult = new Paged<Course>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;

        }
        private static Course MapSingleCourse(IDataReader reader, ref int startingIndex)
        {
            Course onCourse = new Course();

            onCourse.Teachers = new Teacher();
            onCourse.Seasons = new SeasonTerm();
            onCourse.Students = new List<Student>();

                

            onCourse.Id = reader.GetSafeInt32(startingIndex++);
            onCourse.Name = reader.GetSafeString(startingIndex++);
            onCourse.Description = reader.GetSafeString(startingIndex++);
            onCourse.Seasons.Term = reader.GetSafeString(startingIndex++);
            onCourse.Teachers.Name = reader.GetSafeString(startingIndex++);
            onCourse.Students = reader.DeserializeObject<List<Student>>(startingIndex++);

            return onCourse;

        }

        private static void AddCommonParams(CourseAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@SeasonTermId", model.SeasonTermId);
            col.AddWithValue("@TeacherId", model.TeacherId);
        }

        //YOU SHALL NOT PASS THIS!
    }
}
