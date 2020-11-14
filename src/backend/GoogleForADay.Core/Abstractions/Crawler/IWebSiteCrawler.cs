using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GoogleForADay.Core.Model.Crawler;

namespace GoogleForADay.Core.Abstractions.Crawler
{
    public interface IWebSiteCrawler
    {
        Task<CrawlResponse> Crawl(string url, int depth );
    }
}
