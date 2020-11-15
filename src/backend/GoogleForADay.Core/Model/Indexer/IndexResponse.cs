using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleForADay.Core.Abstractions.Crawler;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model.Store;

namespace GoogleForADay.Core.Model.Indexer
{
    public class IndexResponse
    {
        public long ComplexionTime { get; set; }

        public int IndexedPagesCount { get; set; }
        public HashSet<string> IndexedWords { get; set; } = new HashSet<string>();
    }
}
