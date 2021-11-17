using System;
using Xunit;
using Xunit.Abstractions;

namespace UAssetAPI.Tests
{
    [CollectionDefinition("Basic Test Harness")]
    public class BasicTestCollection : ICollectionFixture<AssetUnitTestHarness>
    {
    }

    public class AssetUnitTest
    {
        public AssetUnitTest(AssetUnitTestHarness fixture, ITestOutputHelper output)
        {
            Fixture = fixture;
            Output = output;

            /*
            if (output != null)
                Log.Logger = new LoggerConfiguration()
                    .WriteTo
                    .TestOutput(output, LogEventLevel.Verbose)
                    .CreateLogger();
            */
        }

        public AssetUnitTestHarness Fixture { get; set; }

        public ITestOutputHelper Output { get; set; }
    }
}
