using System.Linq;

using UAssetAPI.ExportTypes;

using Xunit;
using Xunit.Abstractions;

namespace UAssetAPI.Tests
{
    [CollectionDefinition("Asset Test Harness")]
    public class AssetTestCollection : ICollectionFixture<AssetUnitTestHarness>
    {
    }

    public class AssetUnitTest
    {
        #region Common functions

        /// <summary>
        /// Determines whether or not all exports in an asset have parsed correctly.
        /// </summary>
        /// <param name="tester">The asset to test.</param>
        /// <returns>true if all the exports in the asset have parsed correctly, otherwise false.</returns>
        public bool CheckAllExportsParsedCorrectly(UAsset tester)
        {
            return !tester.Exports.Any(t => t is RawExport);
        }

        #endregion Common functions

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
