using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Users;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/users")] // route to be used in postman
    [ApiController]

    public class UserApiControllerV1 : BaseApiController
    {
        #region - Constructor - 
        private IUserServiceV1 _service = null;
        private IAuthenticationService<int> _authService = null;
        public UserApiControllerV1(IUserServiceV1 service
            , ILogger<UserApiControllerV1> logger
            , IAuthenticationService<int> authService)

            : base(logger)
        {

            _service = service;
            _authService = authService;
        }

        #endregion

        [HttpGet]
        public ActionResult<ItemsResponse<User>> Get()

        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                List<User> list = _service.GetAll();
                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<User> { Items = list };
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

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<User>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                User user = _service.Get(id);

                if (user == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<User> { Item = user };
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
                response = new ErrorResponse($"SqlException Error: {argEx.Message}");
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

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

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(UserAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model);

                ItemResponse<int> response = new ItemResponse<int>() {Item = id};

                result = Created201(response);
            }
            catch (Exception ex)
            {

                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
            
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(UserUpdateRequest model)

        {
            int code = 200;
            BaseResponse response = null;

            try

            {
                _service.Update(model);
                response = new SuccessResponse();

            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);

        
        }


    }

}


     
    
         


        
    


