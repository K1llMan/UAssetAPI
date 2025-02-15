﻿using Newtonsoft.Json;
using System;
using System.IO;
using UAssetAPI.PropertyTypes;

namespace UAssetAPI.StructTypes
{
    /// <summary>
    /// A vector in 2-D space composed of components (X, Y) with floating point precision.
    /// </summary>
    public class Vector2DPropertyData : PropertyData
    {
        /// <summary>Vector's X-component.</summary>
        [JsonProperty]
        public float X;

        /// <summary>Vector's Y-component.</summary>
        [JsonProperty]
        public float Y;

        public Vector2DPropertyData(FName name) : base(name)
        {

        }

        public Vector2DPropertyData()
        {

        }

        private static readonly FName CurrentPropertyType = new FName("Vector2D");
        public override bool HasCustomStructSerialization { get { return true; } }
        public override FName PropertyType { get { return CurrentPropertyType; } }

        public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
        {
            if (includeHeader)
            {
                reader.ReadByte();
            }

            X = reader.ReadSingle();
            Y = reader.ReadSingle();
        }

        public override int Write(AssetBinaryWriter writer, bool includeHeader)
        {
            if (includeHeader)
            {
                writer.Write((byte)0);
            }

            writer.Write(X);
            writer.Write(Y);
            return sizeof(float) * 2;
        }

        public override void FromString(string[] d, UAsset asset)
        {
            if (float.TryParse(d[0], out float res1)) X = res1;
            if (float.TryParse(d[1], out float res2)) Y = res2;
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
}