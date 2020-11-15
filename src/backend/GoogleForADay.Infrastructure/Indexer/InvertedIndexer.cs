using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleForADay.Core.Abstractions.Crawler;
using GoogleForADay.Core.Abstractions.Indexer;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model.Crawler;
using GoogleForADay.Core.Model.Indexer;
using GoogleForADay.Core.Model.Store;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GoogleForADay.Infrastructure.Indexer
{
    public class InvertedIndexer : IPageIndexer
    {
        public IKeyValueRepository<Keyword> Repo { get; }

        public InvertedIndexer(IKeyValueRepository<Keyword> repo)
        {
            Repo = repo;
        }

        public void Index(WebSiteInfo info, ref IndexResponse response)
        {
            var keywords = info.GetKeywords();
            var resp = response;

            Parallel.ForEach(keywords, keyword =>
            {
                var word = Repo.Get(keyword.Term);
                var exist = false;

                if (word != null)
                {
                    exist = true;
                    word.References.AddRange(keyword.References);
                    word.References = word.References.Distinct().ToList();
                }
                else
                {
                    word = keyword;
                }

                if (Repo.Upsert(keyword.Term, word) && !exist)
                    resp.IndexedWords.Add(keyword.Term);

            });

            response.IndexedWords = resp.IndexedWords;
            response.IndexedPagesCount++;

        }
    }
}
