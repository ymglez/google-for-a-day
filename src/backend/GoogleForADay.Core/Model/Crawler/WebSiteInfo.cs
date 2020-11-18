using System.Collections.Generic;
using System.Linq;
using GoogleForADay.Core.Model.Store;

namespace GoogleForADay.Core.Model.Crawler
{
    public class WebSiteInfo
    {
        public string Url { get; set; }

        public string Tittle { get; set; }

        public IDictionary<string, int> Words { get; set; }

        public List<Keyword> GetKeywords()
        {
            return Words
                .Select(item => new Keyword
                {
                    Term = item.Key,
                    References = new List<Reference>
                    {
                        new Reference
                        {
                            Url = Url,
                            Tittle = Tittle,
                            Occurrences = item.Value
                        }
                    }
                })
                .ToList();
        }

    }
}
