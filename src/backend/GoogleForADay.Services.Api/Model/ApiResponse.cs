using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleForADay.Services.Api.Model
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public object Data { get; set; }
    }
}
