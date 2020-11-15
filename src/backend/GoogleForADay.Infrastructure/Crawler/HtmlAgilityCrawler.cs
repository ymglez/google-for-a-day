using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GoogleForADay.Core.Abstractions.Crawler;
using GoogleForADay.Core.Extensions;
using GoogleForADay.Core.Model.Crawler;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace GoogleForADay.Infrastructure.Crawler
{
    public class HtmlAgilityCrawler : IWebSiteCrawler
    {
        public IDictionary<string,int> ExternalLinks { get; set; } = 
            new ConcurrentDictionary<string, int>();

        public int Depth { get; set; }
        public bool Started { get; set; }
        public int Index { get; set; }

        public const string CachePath  = "web/cache/";

        private readonly HtmlWeb _web = new HtmlWeb
        {
            CachePath = CachePath,
            UsingCache = true,
        };

        public async Task<Tuple<bool, WebSiteInfo>> Crawl(string url, int depth)
        {
            Depth = depth;
            Started = true;

            ExternalLinks.Add(url, 1);

            return await Next();
        }

        public async Task<Tuple<bool, WebSiteInfo>> Next()
        {
            if(!Started)
                throw new Exception("Crawler not started yet");

            if (Index >= ExternalLinks.Keys.Count)
                return new Tuple<bool, WebSiteInfo>(false, null);

            var currentUrl = ExternalLinks.Keys.ElementAt(Index);
            var level = ExternalLinks[currentUrl];
            try
            {
                var document = await _web.LoadFromWebAsync(currentUrl);
                var info = Parse(document, currentUrl, level);
                Index++;
                return new Tuple<bool, WebSiteInfo>(true, info);
            }
            catch
            {
                return new Tuple<bool, WebSiteInfo>(true, null);
            }
        }


        #region helpers

        private WebSiteInfo Parse(HtmlDocument doc, string url, int level)
        {
            if (doc?.DocumentNode == null) return null;
            var tt = doc.DocumentNode.SelectSingleNode("//head/title");
            var response = new WebSiteInfo
            {
                Url = url,
                Tittle = tt?.InnerText,
                Words = new Dictionary<string, int>()
            };

            foreach (var node in doc.DocumentNode.DescendantsAndSelf())
            {
                
                if (node.NodeType == HtmlNodeType.Text && node.ParentNode.Name != "script")
                {
                    ExtractWords(ref response, node);
                }
                else if(node.Name == "a" && node.Attributes.Contains("href") )
                {
                    var att = node.Attributes["href"];

                    if (level < Depth && att.Value.Contains("http") && 
                        !ExternalLinks.ContainsKey(att.Value))
                    {
                        ExternalLinks.Add(att.Value, level + 1);
                    }
                    
                }
            }

            return response;
        }

        private static void ExtractWords(ref WebSiteInfo response, HtmlNode item)
        {
            var text = HtmlEntity.DeEntitize(item.InnerText.Trim()) ;

            if (string.IsNullOrEmpty(text)) return;

            var words = text.SplitIntoWords();

            foreach (var word in words)
            {
                if (response.Words.ContainsKey(word))
                    response.Words[word]++;
                else
                    response.Words.Add(word, 1);
            }
        }

        #endregion


    }
}
