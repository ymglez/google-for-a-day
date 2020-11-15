using System;
using System.Collections.Generic;
using System.Text;
using GoogleForADay.Core.Abstractions.Indexer;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GoogleForADay.Tests
{
    public class IndexerTest : BaseTest
    {
        readonly IndexerManagerBase _indexer = ServiceProvider.GetService<IndexerManagerBase>();

        [Fact]
        public void TestIndex()
        {
 
            var response = _indexer.Index("https://github.com/ymglez/google-for-a-day", 2)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            Assert.NotNull(response);
        }
    }
}
