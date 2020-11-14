using System.Linq;
using System.Threading.Tasks;
using GoogleForADay.Core.Abstractions.Crawler;
using GoogleForADay.Core.Abstractions.Indexer;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model.Store;

namespace GoogleForADay.Infrastructure.Indexer
{
    public class InvertedIndexer : PageIndexer
    {
        public InvertedIndexer(IKeyValueRepository<Keyword> repo, IWebSiteCrawler crawler) 
            : base(repo, crawler)
        {
        }

        public override async Task<object> Index(string url, int depth = 2)
        {
            var response = await Crawler.Crawl(url, depth);
            var keywordsGroup = response.WebInfos
                .Select(siteInfo => siteInfo.GetKeywords())
                .SelectMany(keys => keys)
                .GroupBy(k => k.Term)
                .ToList();

            
            foreach (var grp in keywordsGroup)
            {
                var key = grp.Key;

                var allReferences = grp
                    .Select(k => k.References)
                    .SelectMany(r => r)
                    .ToList();

                var word = Repo.Get(key) ?? grp.FirstOrDefault();
                word?.References.AddRange(allReferences);

                if(word == null) continue;

                word.References = word?.References.Distinct().ToList();

                Repo.Upsert(key, word);
            }

            Repo.SaveChanges();

            return new
            {
                PagesCount = response.WebInfos.Count,
                WordsCount = keywordsGroup.Distinct().Count()
            };
        }
    }
}
