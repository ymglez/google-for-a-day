using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GoogleForADay.Core.Abstractions.Crawler;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model.Crawler;
using GoogleForADay.Core.Model.Indexer;
using GoogleForADay.Core.Model.Store;

namespace GoogleForADay.Core.Abstractions.Indexer
{
    /// <summary>
    /// Save info from a crawled web page
    /// </summary>
    public interface IPageIndexer
    {
        /// <summary>
        /// Index algorithm
        /// </summary>
        /// <param name="info"></param>
        /// <param name="response"></param>
        void Index(WebSiteInfo info, ref IndexResponse response);
    }
}
