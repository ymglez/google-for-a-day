using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GoogleForADay.Core.Abstractions.Crawler;
using GoogleForADay.Core.Abstractions.Indexer;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model.Indexer;
using GoogleForADay.Core.Model.Store;

namespace GoogleForADay.Infrastructure.Indexer
{
    public class IndexerManager : IndexerManagerBase
    {

        public IndexerManager(IWebSiteCrawler crawler, IPageIndexer indexer) 
            : base(crawler, indexer)
        {
        }

        public override async Task<IndexResponse> Index(string url, int depth = 2)
        {
            var response = new IndexResponse();

            var crawlResult = await Crawler.Crawl(url, depth);

            while (crawlResult.Item1)
            {
                Indexer.Index(crawlResult.Item2, ref response);
                crawlResult = await Crawler.Next();
            }

            return response;
        }

       
    }
}
