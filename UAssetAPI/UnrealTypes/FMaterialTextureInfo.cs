using Newtonsoft.Json;

namespace UAssetAPI.UnrealTypes
{
    public class FMaterialTextureInfo
    {
        [JsonProperty]
        public float SamplingScale { get; set; }

        [JsonProperty]
        public int UVChannelIndex { get; set; }

        [JsonProperty]
        public FName TextureName { get; set; }

        public FMaterialTextureInfo()
        {

        }
    }
}
