using System;
using GoogleForADay.Core.Abstractions.Crawler;
using GoogleForADay.Core.Abstractions.Indexer;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model.Store;
using GoogleForADay.Infrastructure.Crawler;
using GoogleForADay.Infrastructure.Indexer;
using GoogleForADay.Infrastructure.Store.LightningDB;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleForADay.Tests
{
    public class BaseTest
    {
        protected IServiceProvider ServiceProvider;

        public BaseTest()
        {
            ServiceProvider = new ServiceCollection()
                .AddSingleton<IKeyValueRepository<Keyword>, LightningRepository<Keyword>>()
                .AddTransient<IWebSiteCrawler, HtmlAgilityCrawler>()
                .AddSingleton<PageIndexer, InvertedIndexer>()
                .BuildServiceProvider();
        }

        
    }
}
