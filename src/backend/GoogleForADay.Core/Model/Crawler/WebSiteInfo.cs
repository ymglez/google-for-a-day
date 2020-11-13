using System.Collections.Generic;
using GoogleForADay.Core.Model.Store;

namespace GoogleForADay.Core.Model.Crawler
{
    public class WebSiteInfo
    {
        public string Url { get; set; }

        public string Tittle { get; set; }

        public IDictionary<string, int> Words { get; set; }

    }
}
