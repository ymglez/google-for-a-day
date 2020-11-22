using System.Diagnostics;
using System.Threading.Tasks;
using GoogleForADay.Core.Abstractions.Crawler;
using GoogleForADay.Core.Abstractions.Indexer;
using GoogleForADay.Core.Model.Indexer;

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

            var watch = new Stopwatch();

            watch.Start();
            var crawlResult = await Crawler.Crawl(url, depth);
            
            while (crawlResult.Item1 )
            {
                if (crawlResult.Item2 != null)
                    Indexer.Index(crawlResult.Item2, ref response);
                
                crawlResult = await Crawler.Next();
            }

            watch.Stop();
            response.ComplexionTime = watch.Elapsed.Seconds;

            Crawler.Reset();
            return response;
        }


       
    }
}
