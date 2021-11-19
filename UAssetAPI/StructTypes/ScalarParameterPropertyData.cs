﻿using System;

using UAssetAPI.PropertyTypes;

namespace UAssetAPI.StructTypes
{
    /// <summary>
    /// Describes ScalarParameterValue
    /// </summary>
    public class ScalarParameterPropertyData : PropertyData<FScalarParameter>
    {
        public ScalarParameterPropertyData(FName name) : base(name)
        {

        }

        public ScalarParameterPropertyData()
        {

        }

        private static readonly FName CurrentPropertyType = new FName("ScalarParameterValue");
        public override bool HasCustomStructSerialization { get { return true; } }
        public override FName PropertyType { get { return CurrentPropertyType; } }

        public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
        {
            if (includeHeader)
            {
                reader.ReadByte();
            }

            FScalarParameter data = new FScalarParameter {
                Info = new FMaterialParameterInfo {
                    Name = reader.ReadFName(),
                    Association = (EMaterialParameterAssociation)reader.ReadByte(),
                    Index = reader.ReadInt32(),
                },
                Value = reader.ReadSingle(),
                Guid = new Guid(reader.ReadBytes(16))
            };

            Value = data;
        }

        public override int Write(AssetBinaryWriter writer, bool includeHeader)
        {
            if (includeHeader)
            {
                writer.Write((byte)0);
            }

            return sizeof(short);
        }

        public override string ToString()
        {
            return Convert.ToString(Value);
        }

        public override void FromString(string[] d, UAsset asset)
        {

        }
    }
}