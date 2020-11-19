using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GoogleForADay.Core.Model.Crawler;

namespace GoogleForADay.Core.Abstractions.Crawler
{
    /// <summary>
    /// Crawl web page
    /// </summary>
    public interface IWebSiteCrawler
    {
        /// <summary>
        /// Crawl root page of the given url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="depth"></param>
        /// <returns>true if start and root data</returns>
        Task<Tuple<bool, WebSiteInfo>> Crawl(string url, int depth );

        /// <summary>
        /// Continue for next url
        /// </summary>
        /// <returns>true if there are more, the crawled data for current url </returns>
        Task<Tuple<bool, WebSiteInfo>> Next();

        /// <summary>
        /// Start fresh 
        /// </summary>
        void Reset();
    }
}
