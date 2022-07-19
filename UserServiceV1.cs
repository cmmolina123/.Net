using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Users;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class UserServiceV1 : IUserServiceV1
    {
        IDataProvider _data = null;

        public UserServiceV1(IDataProvider data)
        {
            _data = data;
        }

        #region - ADD/UPDATE/DELETE -
        public int Add(UserAddRequest model)
        {
            int id = 0;

            string procName = "[dbo].[Users_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);

                    //and 1 Output

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

        public void Update(UserUpdateRequest model)
        {
            string procName = "[dbo].[Users_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@Id", model.Id);

                },
                returnParameters: null);

        }

        private static void AddCommonParams(UserAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@FirstName", model.FirstName);
            col.AddWithValue("@LastName", model.LastName);
            col.AddWithValue("@Email", model.Email);
            col.AddWithValue("@AvatarUrl", model.AvatarUrl);
            col.AddWithValue("@TenantId", model.TenantId);
            col.AddWithValue("@Password", model.Password);
           // col.AddWithValue("@PasswordConfirm", model.PasswordConfirm);

        }

        public void Delete(int Id)
        {
            string procName = "[dbo].[Users_Delete]";
            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", Id);

            }, returnParameters: null);
        }

        #endregion

        #region - GETS -

        public User Get(int id)
        {
            string procName = "[dbo].[Users_SelectById]";

            User userV1 = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);


            }, delegate (IDataReader reader, short set)
            {
                userV1 = MapSingleUser(reader);
            }

            );
            return userV1;
        }

        public List<User> GetAll()
        {
            List<User> list = null;


            string procName = "[dbo].[Users_SelectAll]";

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                User aUser = MapSingleUser(reader);

                if (list == null)
                {
                    list = new List<User>();
                }

                list.Add(aUser);
            }

           );
            return list;
        }

        private static User MapSingleUser(IDataReader reader)
        {
            User aUserV1 = new User();

            int startingIdex = 0;

            aUserV1.Id = reader.GetSafeInt32(startingIdex++);
            aUserV1.FirstName = reader.GetSafeString(startingIdex++);
            aUserV1.LastName = reader.GetSafeString(startingIdex++);
            aUserV1.Email = reader.GetSafeString(startingIdex++);
            aUserV1.AvatarUrl = reader.GetSafeString(startingIdex++);
            aUserV1.TenantId = reader.GetSafeString(startingIdex++);
            //aUserV1.DateCreated = reader.GetSafeDateTime(startingIdex++);
            //aUserV1.DateModified = reader.GetSafeDateTime(startingIdex++);

            return aUserV1;
        }
        #endregion
    }
}
