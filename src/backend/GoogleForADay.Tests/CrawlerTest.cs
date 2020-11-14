using System;
using System.Collections.Generic;
using System.Text;
using GoogleForADay.Core.Abstractions.Crawler;
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

            var response = crawler.Crawl("https://github.com/ymglez/google-for-a-day");
        }
    }
}
