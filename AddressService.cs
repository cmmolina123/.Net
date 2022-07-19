using Sabio.Data.Providers;
using Sabio.Models.Domain.Addresses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using Sabio.Models.Requests.Addresses;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class AddressService : IAddressService
    {
        IDataProvider _data = null;

        //constructor function w/o params and return type
        public AddressService(IDataProvider data)
        {
            _data = data;
        }

        public int Add(AddressAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Sabio_Addresses_Insert]";
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
        public void Update(AddressUpdateRequest model)
        {
            string procName = "[dbo].[Sabio_Addresses_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@Id", model.Id);

                },
                returnParameters: null);

        }
        private static void AddCommonParams(AddressAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@LineOne", model.LineOne);
            col.AddWithValue("@SuiteNumber", model.SuiteNumber);
            col.AddWithValue("@City", model.City);
            col.AddWithValue("@State", model.State);
            col.AddWithValue("@PostalCode", model.PostalCode);
            col.AddWithValue("@IsActive", model.IsActive);
            col.AddWithValue("@Lat", model.Lat);
            col.AddWithValue("@Long", model.Long);
        }

        public Address Get(int id)
        {
            /*[dbo].[Sabio_Addresses_SelectById]

            @Id int

            Action<SqlParameterCollection> inputParamMapper,
            Action< IDataReader, short> singleRecordMapper,

            Action<SqlParameterCollection> returnParameters = null,
            Action< SqlCommand > cmdModifier = null)
             */

            string procName = "[dbo].[Sabio_Addresses_SelectById]";

            Address address = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {

                /* mapper - takes data in one shape and produces a second shape

                    int > param (int)

                 */

                paramCollection.AddWithValue("@Id", id);


            }, delegate (IDataReader reader, short set) //single Record Mapper
            {
                address = MapSingleAddress(reader);
            }

            );
            return address;
        }

        public List<Address> GetRandomAddresses()
        {
            List<Address> list = null;


            string procName = "[dbo].[Sabio_Addresses_SelectRandom50]";

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Address aAddress = MapSingleAddress(reader);

                if (list == null)
                {
                    list = new List<Address>();
                }

                list.Add(aAddress);
            }

           );
            return list;
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Sabio_Addresses_DeleteById]";
            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@Id", id);

                }, returnParameters: null);
        }

        private static Address MapSingleAddress(IDataReader reader)
        {
            Address aAddress = new Address();

            int startingIdex = 0;

            aAddress.Id = reader.GetSafeInt32(startingIdex++);
            aAddress.LineOne = reader.GetSafeString(startingIdex++);
            aAddress.SuiteNumber = reader.GetSafeInt32(startingIdex++);
            aAddress.City = reader.GetSafeString(startingIdex++);
            aAddress.State = reader.GetSafeString(startingIdex++);
            aAddress.PostalCode = reader.GetSafeString(startingIdex++);
            aAddress.IsActive = reader.GetSafeBool(startingIdex++);
            aAddress.Lat = reader.GetSafeDouble(startingIdex++);
            aAddress.Long = reader.GetSafeDouble(startingIdex++);
            return aAddress;
        }

    }

}
