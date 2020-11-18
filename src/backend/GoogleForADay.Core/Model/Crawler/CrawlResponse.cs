using System.Collections.Generic;

namespace GoogleForADay.Core.Model.Crawler
{
    public class CrawlResponse
    {
        public List<WebSiteInfo> WebInfos { get; set; }

        public double ComplexionTime { get; set; }
        public int ErrorCount { get; set; }
    }
}
