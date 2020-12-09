using System;
using System.Threading.Tasks;
using GoogleForADay.Services.Api.Model;
using GoogleForADay.Services.Business.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GoogleForADay.Services.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngineController : ControllerBase
    {
        private SearchEngineController BusinessController { get; }

        public EngineController(SearchEngineController businessController)
        {
            BusinessController = businessController;
        }
        

        // GET: api/Engine/word
        [HttpGet("search/{q}", Name = "Get")]
        public ApiResponse GetSearch(string q)
        {
            var response = new ApiResponse();

            try
            {
                var businessResponse = BusinessController.Search(q);

                response.Code = 200;
                response.Message = "Ok";
                response.Data = businessResponse;

                return response;
            }
            catch (ArgumentException ae)
            {
                response.Code = 400;
                response.Message = ae.Message;
                return response;
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
                return response;
            }
        }

        // POST: api/Engine
        [HttpPost("index")]
        public async Task<ApiResponse> PostIndex([FromBody] ApiIndexRequest value)
        {
            var response = new ApiResponse();

            try
            {
                var businessResponse = await BusinessController.Index(value.Url, value.Depth);

                response.Code = 200;
                response.Message = "Ok";
                response.Data = businessResponse;

                return response;
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
                return response;
            }
        }

        [HttpPost("clear")]
        public ApiResponse PostClear([FromBody] object unused)
        {
            
            var response = new ApiResponse();

            try
            {
                var result = BusinessController.Repository.Clear();

                response.Code = 200;
                response.Message = "Ok";
                response.Data = result;

                return response;
            }
            catch (Exception e)
            {
                response.Code = 500;
                response.Message = e.Message;
                return response;
            }

        }

    }
}
