﻿using System;
using System.Collections.Generic;
using System.IO;
using UAssetAPI.PropertyTypes;

namespace UAssetAPI.StructTypes
{
    /// <summary>
    /// <see cref="IntPropertyData"/> (<see cref="int"/>) property with per-platform overrides.
    /// </summary>
    public class PerPlatformIntPropertyData : PropertyData<int[]>
    {
        public PerPlatformIntPropertyData(FName name) : base(name)
        {

        }

        public PerPlatformIntPropertyData()
        {

        }

        private static readonly FName CurrentPropertyType = new FName("PerPlatformInt");
        public override bool HasCustomStructSerialization { get { return true; } }
        public override FName PropertyType { get { return CurrentPropertyType; } }

        public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
        {
            if (includeHeader)
            {
                reader.ReadByte();
            }

            int numEntries = reader.ReadInt32();
            Value = new int[numEntries];
            for (int i = 0; i < numEntries; i++)
            {
                Value[i] = reader.ReadInt32();
            }
        }

        public override int Write(AssetBinaryWriter writer, bool includeHeader)
        {
            if (includeHeader)
            {
                writer.Write((byte)0);
            }

            writer.Write(Value.Length);
            for (int i = 0; i < Value.Length; i++)
            {
                writer.Write(Value[i]);
            }
            return sizeof(int) + sizeof(int) * Value.Length;
        }

        public override void FromString(string[] d, UAsset asset)
        {
            var valueList = new List<int>();
            if (int.TryParse(d[0], out int res1)) valueList.Add(res1);
            if (int.TryParse(d[1], out int res2)) valueList.Add(res2);
            if (int.TryParse(d[2], out int res3)) valueList.Add(res3);
            if (int.TryParse(d[3], out int res4)) valueList.Add(res4);
            Value = valueList.ToArray();
        }

        public override string ToString()
        {
            string oup = "(";
            for (int i = 0; i < Value.Length; i++)
            {
                oup += Convert.ToString(Value[i]) + ", ";
            }
            return oup.Remove(oup.Length - 2) + ")";
        }
    }
}