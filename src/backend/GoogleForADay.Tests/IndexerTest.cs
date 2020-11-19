using System;
using System.Collections.Generic;
using System.Text;
using GoogleForADay.Core.Abstractions.Indexer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace GoogleForADay.Tests
{
    public class IndexerTest : BaseTest
    {
        readonly IndexerManagerBase _indexer = ServiceProvider.GetService<IndexerManagerBase>();

        [Fact]
        public void TestIndex()
        {
            var url = "https://dejanstojanovic.net/";
            var response = _indexer.Index(url, 2)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            response.IndexedWordsCount = response.IndexedWords.Count;
            response.IndexedWords = null;
            System.IO.File.AppendAllText("index_results.log" , 
                url + '\n' + JsonConvert.SerializeObject(response) + '\n');
            
            Assert.NotNull(response);
        }
    }
}
