using System.Collections.Generic;
using System.IO;
using System.Linq;

using FluentAssertions;

using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes;
using UAssetAPI.PropertyTypes.Simple;
using UAssetAPI.PropertyTypes.Struct;
using UAssetAPI.UnrealTypes;
using UAssetAPI.UnrealTypes.Enums;

using Xunit;
using Xunit.Abstractions;

namespace UAssetAPI.Tests.Tests
{
    [Collection("Asset Test Harness")]
    public class BasicTest : AssetUnitTest
    {
        /// <summary>
        /// MapProperties contain no easy way to determine the type of structs within them.
        /// For C++ classes, it is impossible without access to the headers, but for blueprint classes, the correct serialization is contained within the UClass.
        /// In this test, we take an asset with custom struct serialization in a map and extract data from the ClassExport in order to determine the correct serialization for the structs.
        /// Binary equality is expected.
        /// </summary>
        [Theory]
        [InlineData(@"TestAssets/TestCustomSerializationStructsInMap/wtf.uasset")]
        public void TestCustomSerializationStructsInMap(string path)
        {
            UAsset tester = new UAsset(path, UE4Version.VER_UE4_25);
            tester.VerifyBinaryEquality().Should().BeTrue();

            // Get the map property in export 2
            Export exportTwo = FPackageIndex.FromRawIndex(2).ToExport(tester);
            (exportTwo is NormalExport).Should().BeTrue();

            NormalExport exportTwoNormal = (NormalExport)exportTwo;

            FName mapPropertyName = FName.FromString("KekWait");
            MapPropertyData testMap = exportTwoNormal[mapPropertyName] as MapPropertyData;
            testMap.Should().NotBeNull();
            (testMap == exportTwoNormal[mapPropertyName.Value.Value]).Should().BeTrue();

            // Get the first entry of the map
            StructPropertyData entryKey = testMap?.Value?.Keys?.ElementAt(0) as StructPropertyData;
            StructPropertyData entryValue = testMap?.Value?[0] as StructPropertyData;
            entryKey?.Value?[0].Should().NotBeNull();
            entryValue?.Value?[0].Should().NotBeNull();

            // Check that the properties are correct
            (entryKey.Value[0] is VectorPropertyData).Should().BeTrue();
            (entryValue.Value[0] is LinearColorPropertyData).Should().BeTrue();
        }

        /// <summary>
        /// In this test, we examine a cooked asset that has been modified by an external tool.
        /// As a result of external modification, the asset now has new name map entries whose hashes were left empty.
        /// Binary equality is expected. Expected behavior is for UAssetAPI to detect this and override its normal hash algorithm.
        /// </summary>
        [Theory]
        [InlineData(@"TestAssets/TestImproperNameMapHashes/OC_Gatling_DamageB_B.uasset")]
        public void TestImproperNameMapHashes(string path)
        {
            UAsset tester = new UAsset(path, UE4Version.VER_UE4_25);
            tester.VerifyBinaryEquality().Should().BeTrue();

            Dictionary<string, bool> testingEntries = new Dictionary<string, bool> 
            {
                ["/Game/WeaponsNTools/GatlingGun/Overclocks/OC_BonusesAndPenalties/OC_Bonus_MovmentBonus_150p"] = false,
                ["/Game/WeaponsNTools/GatlingGun/Overclocks/OC_BonusesAndPenalties/OC_Bonus_MovmentBonus_150p.OC_Bonus_MovmentBonus_150p"] = false
            };

            foreach (KeyValuePair<FString, uint> overrideHashes in tester.OverrideNameMapHashes)
            {
                if (testingEntries.ContainsKey(overrideHashes.Key.Value))
                {
                    (overrideHashes.Value == 0).Should().BeTrue();
                    testingEntries[overrideHashes.Key.Value] = true;
                }
            }

            foreach (KeyValuePair<string, bool> testingEntry in testingEntries)
            {
                testingEntry.Value.Should().BeTrue();
            }
        }

        /// <summary>
        /// In this test, we examine a cooked asset that has been modified by an external tool.
        /// As a result of external modification, two identical entries now exist in the name map, which never occurs in assets cooked by the Unreal Engine.
        /// Binary equality is not expected, but the asset must successfully parse anyways.
        /// </summary>
        [Theory]
        [InlineData(@"TestAssets/TestDuplicateNameMapEntries/BIOME_AzureWeald.uasset")]
        public void TestDuplicateNameMapEntries(string path)
        {
            UAsset tester = new UAsset(path, UE4Version.VER_UE4_25);

            // Make sure a duplicate entry actually exists
            bool duplicatesExist = false;
            Dictionary<string, bool> enumeratedEntries = new Dictionary<string, bool>();
            foreach (FString entry in tester.GetNameMapIndexList())
            {
                if (enumeratedEntries.ContainsKey(entry.Value) && enumeratedEntries[entry.Value])
                {
                    duplicatesExist = true;
                    break;
                }
                enumeratedEntries[entry.Value] = true;
            }
            duplicatesExist.Should().BeTrue();

            // Make sure all exports parsed correctly
            CheckAllExportsParsedCorrectly(tester).Should().BeTrue();
        }

        /// <summary>
        /// In this test, we have an asset with a few properties that UAssetAPI has no serialization for. (The properties do not actually exist in the engine itself, so this is expected behavior.)
        /// UAssetAPI must fallback to UnknownPropertyType to parse the asset correctly and maintain binary equality.
        /// </summary>
        [Theory]
        [InlineData(@"TestAssets/TestUnknownProperties/BP_DetPack_Charge.uasset")]
        public void TestUnknownProperties(string path)
        {
            UAsset tester = new UAsset(path, UE4Version.VER_UE4_25);
            tester.VerifyBinaryEquality().Should().BeTrue();
            CheckAllExportsParsedCorrectly(tester).Should().BeTrue();

            // Check that only the expected unknown properties are present
            Dictionary<string, bool> newUnknownProperties = new Dictionary<string, bool> 
            {
                { "GarbagePropty", false }, 
                { "EvenMoreGarbageTestingPropertyy", false }
            };

            foreach (Export testExport in tester.Exports)
            {
                if (testExport is NormalExport normalTestExport)
                {
                    foreach (PropertyData prop in normalTestExport.Data)
                    {
                        if (prop is UnknownPropertyData unknownProp)
                        {
                            string serializingType = unknownProp?.SerializingPropertyType?.Value?.Value;
                            serializingType.Should().NotBeNull();
                            newUnknownProperties.ContainsKey(serializingType).Should().BeTrue();
                            newUnknownProperties[serializingType] = true;
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, bool> entry in newUnknownProperties)
            {
                entry.Value.Should().BeTrue();
            }
        }

        private void TestManyAssetsSubsection(string path, UE4Version version)
        {
            Output.WriteLine(path);
            UAsset tester = new UAsset(path, version);
            tester.VerifyBinaryEquality().Should().BeTrue();
            CheckAllExportsParsedCorrectly(tester).Should().BeTrue();
        }

        /// <summary>
        /// In this test, we examine a variety of assets from different games and ensure that they parse correctly and maintain binary equality.
        /// </summary>
        [Theory]
        [InlineData(@"TestAssets/TestManyAssets/Astroneer/DebugMenu.uasset")]
        [InlineData(@"TestAssets/TestManyAssets/Astroneer/Staging_T2.umap")]
        [InlineData(@"TestAssets/TestManyAssets/Astroneer/Augment_BroadBrush.uasset")]
        [InlineData(@"TestAssets/TestManyAssets/Astroneer/LargeResourceCanister_IT.uasset")]
        [InlineData(@"TestAssets/TestManyAssets/Astroneer/ResourceProgressCurve.uasset")]
        public void TestManyAssetsAstroneer(string path)
        {
            TestManyAssetsSubsection(path, UE4Version.VER_UE4_23);
        }

        /// <summary>
        /// In this test, we examine a variety of assets from different games and ensure that they parse correctly and maintain binary equality.
        /// </summary>
        [Theory]
        [InlineData(@"TestAssets/TestManyAssets/Bloodstained/m01SIP_000_BG.umap")]
        [InlineData(@"TestAssets/TestManyAssets/Bloodstained/m01SIP_000_Gimmick.umap")]
        [InlineData(@"TestAssets/TestManyAssets/Bloodstained/m02VIL_004_Gimmick.umap")]
        [InlineData(@"TestAssets/TestManyAssets/Bloodstained/PB_DT_RandomizerRoomCheck.uasset")]
        [InlineData(@"TestAssets/TestManyAssets/Bloodstained/PB_DT_ItemMaster.uasset")]
        public void TestManyAssetsBloodstained(string path)
        {
            TestManyAssetsSubsection(path, UE4Version.VER_UE4_18);
        }

        /// <summary>
        /// In this test, we examine and modify a DataTable to ensure that it parses correctly and maintains binary equality.
        /// </summary>
        [Theory]
        [InlineData(@"TestAssets/TestManyAssets/Bloodstained/PB_DT_RandomizerRoomCheck.uasset")]
        public void TestDataTables(string path)
        {
            UAsset tester = new UAsset(path, UE4Version.VER_UE4_18);
            tester.VerifyBinaryEquality().Should().BeTrue();
            CheckAllExportsParsedCorrectly(tester).Should().BeTrue();
            (tester.Exports.Count == 1).Should().BeTrue();

            DataTableExport ourDataTableExport = tester.Exports[0] as DataTableExport;
            UDataTable ourTable = ourDataTableExport?.Table;
            ourTable.Should().NotBeNull();

            // Check out the first entry to make sure it's parsing alright, and flip all the flags for later testing
            StructPropertyData firstEntry = ourTable.Data[0];

            bool didFindTestName = false;
            for (int i = 0; i < firstEntry.Value.Count; i++)
            {
                PropertyData propData = firstEntry.Value[i];
                Output.WriteLine(i + ": " + propData.Name + ", " + propData.PropertyType);
                if (propData.Name == new FName("AcceleratorANDDoubleJump")) didFindTestName = true;
                if (propData is BoolPropertyData boolProp) boolProp.Value = !boolProp.Value;
            }
            didFindTestName.Should().BeTrue();

            string modifiedTableFilename = Path.Combine(Path.GetDirectoryName(path), "MODIFIED.uasset");
            // Save the modified table
            tester.Write(modifiedTableFilename);

            // Load the modified table back in and make sure we're good
            UAsset tester2 = new UAsset(modifiedTableFilename, UE4Version.VER_UE4_18);
            tester2.VerifyBinaryEquality().Should().BeTrue();
            CheckAllExportsParsedCorrectly(tester2).Should().BeTrue();
            (tester2.Exports.Count == 1).Should().BeTrue();

            // Flip the flags back to what they originally were
            firstEntry = (tester2.Exports[0] as DataTableExport)?.Table?.Data?[0];
            firstEntry.Should().NotBeNull();
            for (int i = 0; i < firstEntry.Value.Count; i++)
            {
                if (firstEntry.Value[i] is BoolPropertyData boolProp) boolProp.Value = !boolProp.Value;
            }

            // Save and check that it's binary equal to what we originally had
            tester2.Write(tester2.FilePath);
            File.ReadAllBytes(path)
                .SequenceEqual(File.ReadAllBytes(modifiedTableFilename)).Should().BeTrue();
        }

        /// <summary>
        /// In this test, we examine a variety of assets from different games and ensure that they parse correctly and maintain binary equality.
        /// </summary>
        [Theory]
        [InlineData(@"TestAssets/TestCustomProperty/AlternateStartActor.uasset")]
        public void TestCustomProperty(string path)
        {
            UAsset tester = new UAsset(path, UE4Version.VER_UE4_23);
            tester.VerifyBinaryEquality().Should().BeTrue();
            CheckAllExportsParsedCorrectly(tester).Should().BeTrue();

            // Make sure that there are no unknown properties, and that there is at least one CoolProperty with a value of 72
            bool hasCoolProperty = false;
            foreach (Export testExport in tester.Exports)
            {
                if (testExport is NormalExport normalTestExport)
                {
                    foreach (PropertyData prop in normalTestExport.Data)
                    {
                        (prop is UnknownPropertyData).Should().BeFalse();
                        if (prop is CoolPropertyData coolProp)
                        {
                            hasCoolProperty = true;
                            (coolProp.Value == 72).Should().BeTrue();
                        }
                    }
                }
            }
            hasCoolProperty.Should().BeTrue();
        }

        public BasicTest(AssetUnitTestHarness fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }
    }
}
