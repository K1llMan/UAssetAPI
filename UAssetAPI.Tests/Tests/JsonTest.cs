using System.IO;
using System.Linq;

using FluentAssertions;

using UAssetAPI.Extensions;

using Xunit;
using Xunit.Abstractions;

namespace UAssetAPI.Tests.Tests
{
    [Collection("Asset Test Harness")]
    public class JsonTest : AssetUnitTest
    {
        private void TestJsonOnFile(string file, UE4Version version)
        {
            Output.WriteLine(file);
            UAsset tester = new UAsset(file, version);
            tester.VerifyBinaryEquality().Should().BeTrue();
            CheckAllExportsParsedCorrectly(tester).Should().BeTrue();

            string jsonSerializedAsset = tester.SerializeJson();
            string jsonFilename = Path.Combine(Path.GetDirectoryName(file), "raw.json");
            File.WriteAllText(jsonFilename, jsonSerializedAsset);

            UAsset tester2 = UAsset.DeserializeJson(jsonFilename);
            string modifiedFilename = Path.Combine(Path.GetDirectoryName(file), "MODIFIED.uasset");
            tester2.Write(modifiedFilename);

            // For the assets we're testing binary equality is maintained and can be used as a metric of success, but binary equality is not guaranteed for most assets
            File.ReadAllBytes(file)
                .SequenceEqual(File.ReadAllBytes(modifiedFilename))
                .Should().BeTrue();
        }

        /// <summary>
        /// In this test, we serialize some assets to JSON and back to test if the JSON serialization system is functional.
        /// </summary>
        [Theory]
        [InlineData(@"TestAssets/TestManyAssets/Bloodstained/PB_DT_RandomizerRoomCheck.uasset", UE4Version.VER_UE4_18)]
        [InlineData(@"TestAssets/TestManyAssets/Bloodstained/m02VIL_004_Gimmick.umap", UE4Version.VER_UE4_18)]
        [InlineData(@"TestAssets/TestManyAssets/Astroneer/Staging_T2.umap", UE4Version.VER_UE4_23)]
        [InlineData(@"TestAssets/TestJson/ABP_SMG_A.uasset", UE4Version.VER_UE4_25)]
        public void TestJson(string path, UE4Version version)
        {
            TestJsonOnFile(path, version);
        }

        /// <summary>
        /// In this test, we serialize some assets to JSON and back to test if the JSON serialization system is functional.
        /// </summary>
        [Theory]
        [InlineData(@"TestAssets/TestJson/ABP_SMG_A.uasset", @"TestAssets/TestJson/AssetInfo.json", UE4Version.VER_UE4_26)]
        public void TestFModelJson(string file, string json, UE4Version version)
        {
            UAsset asset = new UAsset(file, version);
            asset.UpdateFromFModelJson(json);
        }


        public JsonTest(AssetUnitTestHarness fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }
    }
}
