using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Addresses;
using Sabio.Models.Requests.Addresses;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressApiController : BaseApiController
    {
        #region - Constructor -
        private IAddressService _service = null;
        private IAuthenticationService<int> _authService = null;
        public AddressApiController(IAddressService service
            , ILogger<AddressApiController> logger
            , IAuthenticationService<int> authService)

            : base(logger)
        {
            _service = service;
            _authService = authService;

        } 
        #endregion

        #region - GETS (Try - Catch Ex.) - 
        [HttpGet]//[HttpGet("")] concatenates ("api/addressess") - this implies HTTP GET api/addresses
        public ActionResult<ItemsResponse<Address>> GetAll()
        {
            int code = 200;
            BaseResponse response = null; // do not declare instance.
            try
            {
                List<Address> list = _service.GetRandomAddresses();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<Address> { Items = list };

                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }

        //api/widgets/{id:int} 
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Address>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Address address = _service.Get(id);

                // ItemResponse<Address> response = new ItemResponse<Address>();
                //response.Item = a;

                if (address == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Address> { Item = address };

                }
            }
            catch (SqlException sqlEx)

            {
                iCode = 500;
                response = new ErrorResponse($"SqlException Error: {sqlEx.Message}");
                base.Logger.LogError(sqlEx.ToString());


            }
            catch (ArgumentException argEx)

            {
                iCode = 500;
                response = new ErrorResponse($"ArgumentException Error: {argEx.Message}");

            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }


            return StatusCode(iCode, response);
        }
        #endregion

        #region - DELETE - 
        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null; // do not declare instance.

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
        #endregion

        #region - ADD -
        [HttpPost("")] //always pass in the user ID of the currently signed in user. 
        public ActionResult<ItemResponse<int>> Create(AddressAddRequest model)

        {
            int userId = _authService.GetCurrentUserId();

            //of new address
            int id = _service.Add(model, userId);

            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = id;


            return Ok(response);

        }
        #endregion

        #region - UPDATE - 
        [HttpPut("{id:int}")] //always pass in the user ID of the currently signed in user.
        public ActionResult<ItemResponse<int>> Update(AddressUpdateRequest model)

        {

            _service.Update(model);

            SuccessResponse response = new SuccessResponse();

            return Ok(response);

        }
        #endregion
    }
}
