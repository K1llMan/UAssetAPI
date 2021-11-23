using System;

using Newtonsoft.Json;

namespace UAssetAPI.UnrealTypes
{
    /// <summary>
    /// Material scalar parameter
    /// </summary>
    public class FScalarParameter
    {
        [JsonProperty]
        public FMaterialParameterInfo Info { get; set; }

        [JsonProperty]
        public float Value { get; set; }

        [JsonProperty]
        public Guid Guid { get; set; }

        public FScalarParameter()
        {

        }
    }
}
