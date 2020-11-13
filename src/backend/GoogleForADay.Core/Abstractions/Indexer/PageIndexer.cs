using System;
using System.Collections.Generic;
using System.Text;
using GoogleForADay.Core.Abstractions.Crawler;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model.Store;

namespace GoogleForADay.Core.Abstractions.Indexer
{
    public abstract class PageIndexer
    {
        public IKeyValueRepository<Keyword> Repo { get; }
        public IWebSiteCrawler Crawler { get; }

        protected PageIndexer(IKeyValueRepository<Keyword> repo, IWebSiteCrawler crawler)
        {
            Repo = repo;
            Crawler = crawler;
        }
    }
}
