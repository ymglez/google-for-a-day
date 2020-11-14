using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using GoogleForADay.Core.Abstractions.Crawler;
using GoogleForADay.Core.Extensions;
using GoogleForADay.Core.Model.Crawler;
using HtmlAgilityPack;

namespace GoogleForADay.Infrastructure.Crawler
{
    public class HtmlAgilityCrawler : IWebSiteCrawler
    {
        public IDictionary<string,int> ExternalLinks { get; set; } = 
            new ConcurrentDictionary<string, int>();

        public int Depth { get; set; }

        public const string CachePath  = "web/cache/";

        public CrawlResponse Crawl(string url, int depth = 2)
        {
            Depth = depth;
            var response = new CrawlResponse
            {
                StatusCode = HttpStatusCode.OK,
                WebInfos = new List<WebSiteInfo>()
            };

            ExternalLinks.Add(url, 1);

            var stopwatch = new Stopwatch();


            stopwatch.Start();
            var web = new HtmlWeb
            {
                CachePath = CachePath,
                UsingCache = true,
            };
            
            foreach (var (key, value) in ExternalLinks)
            {
                var document = web.Load(key);
                response.WebInfos.Add(Parse(document, key, value));
            }
            

            stopwatch.Stop();
            response.ComplexionTime = stopwatch.Elapsed.TotalSeconds;

            return response;
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

           
            foreach (var node in doc?.DocumentNode.DescendantsAndSelf())
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
