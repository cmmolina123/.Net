using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Requests.Friends;
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
    public class FriendService : IFriendServiceV2
    {
        IDataProvider _data = null;

        public FriendService(IDataProvider data)
        {
            _data = data;
        }

        #region - Get - 
        public FriendV2 GetV3(int id)
        {
            string procName = "[dbo].[Friends_SelectByIdV3]";

            FriendV2 amigo = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);


            }, delegate (IDataReader reader, short set)
            {
                int startingIdex = 0;
                amigo = MapSingleFriendV2(reader, ref startingIdex);

                string skillstring = reader.GetSafeString(startingIdex);

                if (!string.IsNullOrEmpty(skillstring))
                {
                    amigo.Skill = reader.DeserializeObject<List<Skills>>(startingIdex++);

                }

            }

            );
            return amigo;
        }

        public List<FriendV2> GetAllV3()
        {
            List<FriendV2> list = null;


            string procName = "[dbo].[Friends_SelectAllV3]";

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIdex = 0;
                FriendV2 aFriendV3 = MapSingleFriendV2(reader, ref startingIdex);

                string skillstring = reader.GetSafeString(startingIdex);

                if (!string.IsNullOrEmpty(skillstring))
                {
                    aFriendV3.Skill = reader.DeserializeObject<List<Skills>>(startingIdex++);

                }

                if (list == null)
                {
                    list = new List<FriendV2>();
                }

                list.Add(aFriendV3);

            });
            return list;
        }


        private static FriendV2 MapSingleFriendV2(IDataReader reader, ref int startingIdex)
        {
            FriendV2 aFriendV2 = new FriendV2();
            aFriendV2.PrimaryImage = new Image();

            aFriendV2.Id = reader.GetSafeInt32(startingIdex++);
            aFriendV2.PrimaryImage.Url = reader.GetSafeString(startingIdex++);
            aFriendV2.PrimaryImage.Id = reader.GetSafeInt32(startingIdex++);
            aFriendV2.PrimaryImage.TypeId = reader.GetSafeInt32(startingIdex++);
            aFriendV2.Title = reader.GetSafeString(startingIdex++);
            aFriendV2.Bio = reader.GetSafeString(startingIdex++);
            aFriendV2.Summary = reader.GetSafeString(startingIdex++);
            aFriendV2.Headline = reader.GetSafeString(startingIdex++);
            aFriendV2.Slug = reader.GetSafeString(startingIdex++);
            aFriendV2.StatusId = reader.GetSafeInt32(startingIdex++);
            aFriendV2.DateCreated = reader.GetSafeDateTime(startingIdex++);
            aFriendV2.DateModified = reader.GetSafeDateTime(startingIdex++);
            aFriendV2.UserId = reader.GetSafeInt32(startingIdex++);

            return aFriendV2;
        }

        #endregion

        #region - Add/Update/Delete -

        public int AddV2(FriendAddRequestV2 model, int currentUser)
        {
            int id = 0;

            string procName = "[dbo].[Friends_InsertV2]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParamsV2(model, col);

                    col.AddWithValue("@UserId", currentUser);

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

        public void UpdateV2(FriendUpdateRequestV2 model, int userId)
        {
            string procName = "[dbo].[Friends_UpdateV2]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParamsV2(model, col);
                    col.AddWithValue("@UserId", userId);
                    col.AddWithValue("@Id", model.Id);
                },
                returnParameters: null);

        }

        public void DeleteV2(int newFriendId)
        {
            string procName = "[dbo].[Friends_DeleteV2]";
            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", newFriendId);

            }, returnParameters: null);
        }

        private static void AddCommonParamsV2(FriendAddRequestV2 model, SqlParameterCollection col)
        {
            col.AddWithValue("@Title", model.Title);
            col.AddWithValue("@Bio", model.Bio);
            col.AddWithValue("@Summary", model.Summary);
            col.AddWithValue("@Headline", model.Headline);
            col.AddWithValue("@Slug", model.Slug);
            col.AddWithValue("@StatusId", model.StatusId);
            col.AddWithValue("@TypeId", model.PrimaryImage.TypeId);
            col.AddWithValue("@PrimaryImageUrl", model.PrimaryImage.Url);
            // col.AddWithValue("@UserId", model.UserId);

        }

        #endregion

        #region - Pagination -
        public Paged<FriendV2> GetAllV3Pagination(int pageIndex, int pageSize)
        {

            Paged<FriendV2> pagedResult = null;

            List<FriendV2> result = null;

            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[Friends_PaginationV3]",
           inputParamMapper: delegate (SqlParameterCollection parameterCollection)
           {
               parameterCollection.AddWithValue("@PageIndex", pageIndex);
               parameterCollection.AddWithValue("@PageSize", pageSize);
           },
           singleRecordMapper: delegate (IDataReader reader, short set)
           {

               int startingIdex = 0;
               FriendV2 amigo = MapSingleFriendV2(reader, ref startingIdex);

               string skillstring = reader.GetSafeString(startingIdex);

               if (!string.IsNullOrEmpty(skillstring))
               {
                   amigo.Skill = reader.DeserializeObject<List<Skills>>(startingIdex++);

               }

               if (totalCount == 0)
               {
                   totalCount = reader.GetSafeInt32(startingIdex++);
               }


               if (result == null)
               {
                   result = new List<FriendV2>();
               }

               result.Add(amigo);
           }

       );
            if (result != null)
            {
                pagedResult = new Paged<FriendV2>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }


        public Paged<FriendV2> SearchPaginationV3(int pageIndex, int pageSize, string query)
        {

            Paged<FriendV2> pagedResult = null;

            List<FriendV2> result = null;

            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[Friends_Search_PaginationV3]",
           inputParamMapper: delegate (SqlParameterCollection parameterCollection)
           {
               parameterCollection.AddWithValue("@PageIndex", pageIndex);
               parameterCollection.AddWithValue("@PageSize", pageSize);
               parameterCollection.AddWithValue("@Query", query);
           },
           singleRecordMapper: delegate (IDataReader reader, short set)
           {

               int startingIdex = 0;
               FriendV2 amigo = MapSingleFriendV2(reader, ref startingIdex);

               string skillstring = reader.GetSafeString(startingIdex);

               if (!string.IsNullOrEmpty(skillstring))
               {
                   amigo.Skill = reader.DeserializeObject<List<Skills>>(startingIdex++);

               }

               if (totalCount == 0)
               {
                   totalCount = reader.GetSafeInt32(startingIdex++);
               }


               if (result == null)
               {
                   result = new List<FriendV2>();
               }

               result.Add(amigo);
           }

       );
            if (result != null)
            {
                pagedResult = new Paged<FriendV2>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }


        #endregion


    }
}

