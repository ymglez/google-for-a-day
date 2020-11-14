using System;
using System.Collections.Generic;
using System.Text;
using GoogleForADay.Core.Model.Crawler;

namespace GoogleForADay.Core.Abstractions.Crawler
{
    public interface IWebSiteCrawler
    {
        CrawlResponse Crawl(string url, int depth = 2);
    }
}
