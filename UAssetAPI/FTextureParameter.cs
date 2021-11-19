﻿using System;

using Newtonsoft.Json;

namespace UAssetAPI
{
    public class FTextureParameter
    {
        [JsonProperty]
        public FMaterialParameterInfo Info { get; set; }

        [JsonProperty]
        public FPackageIndex Value { get; set; }

        [JsonProperty]
        public Guid Guid { get; set; }

        public FTextureParameter()
        {

        }
    }
}
