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
 
            var response = _indexer.Index("http://www.cubadebate.cu/", 2)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            System.IO.File.AppendAllText("index_results.log" , JsonConvert.SerializeObject(response) + '\n');
            Assert.NotNull(response);
        }
    }
}
