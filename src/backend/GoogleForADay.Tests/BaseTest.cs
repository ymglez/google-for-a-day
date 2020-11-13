using System;
using System.Collections.Generic;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model;
using GoogleForADay.Core.Model.Store;
using GoogleForADay.Infrastructure.Store.FASTER;
using GoogleForADay.Infrastructure.Store.LightningDB;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GoogleForADay.Tests
{
    public class BaseTest
    {
        protected IServiceProvider ServiceProvider;

        public BaseTest()
        {
            ServiceProvider = new ServiceCollection()
                .AddSingleton<IKeyValueRepository<Keyword>, LightningRepository<Keyword>>()
                .BuildServiceProvider();
        }

        
    }
}
