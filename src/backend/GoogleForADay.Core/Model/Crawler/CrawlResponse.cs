using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GoogleForADay.Core.Model.Crawler
{
    public class CrawlResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public List<WebSiteInfo> WebInfos { get; set; }

        public int ComplexionTime { get; set; }
    }
}
