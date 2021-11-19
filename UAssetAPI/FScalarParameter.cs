﻿using System;

using Newtonsoft.Json;

namespace UAssetAPI
{
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
