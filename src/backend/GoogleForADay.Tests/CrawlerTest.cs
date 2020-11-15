using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleForADay.Core.Abstractions.Crawler;
using GoogleForADay.Core.Abstractions.Indexer;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model.Store;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GoogleForADay.Tests
{
    public class CrawlerTest : BaseTest
    {
        [Fact]
        public void TestSimplePage()
        {
            var crawler = ServiceProvider.GetService<IWebSiteCrawler>();
            

            var response = crawler.Crawl("https://github.com/ymglez/google-for-a-day", 2)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            if (response.Item1)
                crawler.Next();

        }


        
    }
}
