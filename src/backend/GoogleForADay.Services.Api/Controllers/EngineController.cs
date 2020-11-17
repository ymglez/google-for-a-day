using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleForADay.Services.Api.Model;
using GoogleForADay.Services.Business.Controllers;
using Microsoft.AspNetCore.Http;
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
                if (businessResponse != null)
                {
                    response.Code = 200;
                    response.Message = "Ok";
                    response.Data = businessResponse;
                }
                else
                {
                    response.Code = 404;
                    response.Message = "Not found";
                }

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
        public void PostIndex([FromBody] string value)
        {
        }

        [HttpPost("clear")]
        public void PostClear([FromBody] string value)
        {
        }

    }
}
