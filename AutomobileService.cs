using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Automobiles;
using Sabio.Models.Requests.Automobiles;
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
    public class AutomobileService : IAutomobileService
    {
        IDataProvider _data = null;

        public AutomobileService(IDataProvider data)
        {
            _data = data;
        }
        #region - GETS -

        public Automobile GetCar(int id)
        {
            string procName = "[dbo].[Automobiles_SelectByIdV3]";

            Automobile carro = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);


            }, delegate (IDataReader reader, short set)
            {
                int startingIdex = 0;
                carro = MapSingleAutomobile(reader, ref startingIdex);

                string carroString = reader.GetSafeString(startingIdex);

                if (!string.IsNullOrEmpty(carroString))
                {
                    carro.Features = reader.DeserializeObject<List<AutoFeature>>(startingIdex++); //var from List

                }

            }

            );
            return carro;
        }

        public List<Automobile> GetAllCars()
        {
            List<Automobile> list = null;

            string procName = "[dbo].[Automobiles_SelectAllV3]";

            _data.ExecuteCmd(procName, inputParamMapper: null
                , singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIdex = 0;
                    Automobile anAutomobile = MapSingleAutomobile(reader, ref startingIdex);

                    string autoFeaturesString = reader.GetSafeString(startingIdex);

                    if (!string.IsNullOrEmpty(autoFeaturesString))
                    {
                        anAutomobile.Features = reader.DeserializeObject<List<AutoFeature>>(startingIdex++);
                    }
                    if (list == null)
                    {
                        list = new List<Automobile>();
                    }
                    list.Add(anAutomobile);

                });

            return list;

        }

        #endregion

        #region - Add / Update -
        public int AddCar(AutomobileAddRequest model, int currentUser)
        {
            int id = 0;

            string procName = "[dbo].[Automobiles_InsertV2]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);

                //col.AddWithValue("@UserId", currentUser);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);
            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;
                int.TryParse(oId.ToString(), out id);

                Console.WriteLine("");
            }
            );
            return id;
        }

        public void Update(AutomobileUpdateRequest model)
        {
            string procName = "[dbo].[Automobiles_UpdateV2]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@Id", model.Id);
                },
                returnParameters: null);

        } 
        #endregion

        #region - Pagination -
        public Paged<Automobile> GetAllPagination(int pageIndex, int pageSize)
        {
            Paged<Automobile> pagedResult = null;
            List<Automobile> result = null;

            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[Automobiles_PaginationV3]",
            inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@PageIndex", pageIndex);
                parameterCollection.AddWithValue("@PageSize", pageSize);

            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIdex = 0;
                Automobile carro = MapSingleAutomobile(reader, ref startingIdex);

                string carroString = reader.GetSafeString(startingIdex);

                if (!string.IsNullOrEmpty(carroString))
                {
                    carro.Features = reader.DeserializeObject<List<AutoFeature>>(startingIdex++);
                }

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(startingIdex++);
                }

                if (result == null)
                {
                    result = new List<Automobile>();
                }
                result.Add(carro);
            });

            if (result != null)
            {
                pagedResult = new Paged<Automobile>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;

        }

        public Paged<Automobile> SearchPagination(int pageIndex, int pageSize, string query)
        {
            Paged<Automobile> pagedResult = null;

            List<Automobile> result = null;
            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[Automobiles_Search_PaginationV3]",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                    parameterCollection.AddWithValue("@Query", query);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIdex = 0;
                    Automobile carro = MapSingleAutomobile(reader, ref startingIdex);

                    string carroString = reader.GetSafeString(startingIdex);

                    if (!string.IsNullOrEmpty(carroString))
                    {
                        carro.Features = reader.DeserializeObject<List<AutoFeature>>(startingIdex++);
                    }

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIdex++);
                    }

                    if (result == null)
                    {
                        result = new List<Automobile>();
                    }
                    result.Add(carro);

                }

                );

            if (result != null)
            {
                pagedResult = new Paged<Automobile>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;


        } 
        #endregion

        private static Automobile MapSingleAutomobile(IDataReader reader, ref int startingIdex)
        {
            Automobile anAutomobile = new Automobile();
            anAutomobile.PrimaryImage = new AutoImage();

            anAutomobile.Id = reader.GetSafeInt32(startingIdex++);
            anAutomobile.PrimaryImage.Url = reader.GetSafeString(startingIdex++);
            anAutomobile.PrimaryImage.Id = reader.GetSafeInt32(startingIdex++);
            anAutomobile.PrimaryImage.TypeId = reader.GetSafeInt32(startingIdex++);
            anAutomobile.Make = reader.GetSafeString(startingIdex++);
            anAutomobile.Model = reader.GetSafeString(startingIdex++);
            anAutomobile.Year = reader.GetSafeInt32(startingIdex++);
            anAutomobile.Color = reader.GetSafeString(startingIdex++);
            anAutomobile.Type = reader.GetSafeString(startingIdex++);
            anAutomobile.DateCreated = reader.GetSafeDateTime(startingIdex++);
            anAutomobile.DateModified = reader.GetSafeDateTime(startingIdex++);

            return anAutomobile;
        }

        private static void AddCommonParams(AutomobileAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Make", model.Make);
            col.AddWithValue("@Model", model.Model);
            col.AddWithValue("@Year", model.Year);
            col.AddWithValue("@Color", model.Color);
            col.AddWithValue("@Type", model.Type);
            col.AddWithValue("@TypeId", model.PrimaryImage.TypeId);
            col.AddWithValue("@PrimaryImageUrl", model.PrimaryImage.Url);
        }

        //dont go past here//
    }
   
}

