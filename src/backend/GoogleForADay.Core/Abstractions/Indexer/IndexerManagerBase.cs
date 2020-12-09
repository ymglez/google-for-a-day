using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GoogleForADay.Core.Abstractions.Crawler;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model.Indexer;
using GoogleForADay.Core.Model.Store;

namespace GoogleForADay.Core.Abstractions.Indexer
{
    /// <summary>
    /// Base class for index web pages
    /// </summary>
    public abstract class IndexerManagerBase
    {
        
        protected IWebSiteCrawler Crawler { get; }
        protected IPageIndexer Indexer { get; }

        protected IndexerManagerBase( IWebSiteCrawler crawler, IPageIndexer indexer)
        {
            Crawler = crawler;
            Indexer = indexer;
        }

        public abstract Task<IndexResponse> Index(string url, int depth = 2);
    }
}
