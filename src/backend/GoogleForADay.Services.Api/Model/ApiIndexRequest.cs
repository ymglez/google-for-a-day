using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleForADay.Services.Api.Model
{
    public class ApiIndexRequest
    {
        public string Url { get; set; }
        public int Depth { get; set; } = 2;

    }
}
