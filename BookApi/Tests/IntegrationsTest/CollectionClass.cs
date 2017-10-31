
using System.Net.Http;
using Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace BookApi.Tests
 {
    [CollectionDefinition("SystemCollection")]
    public class Collection : ICollectionFixture<TestContext>
    {

    }
 }
