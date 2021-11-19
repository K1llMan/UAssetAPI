using System;

using UAssetAPI.PropertyTypes;

namespace UAssetAPI.StructTypes
{
    /// <summary>
    /// Describes MaterialTextureInfo
    /// </summary>
    public class MaterialTextureInfoPropertyData : PropertyData<FMaterialTextureInfo>
    {
        public MaterialTextureInfoPropertyData(FName name) : base(name)
        {

        }

        public MaterialTextureInfoPropertyData()
        {

        }

        private static readonly FName CurrentPropertyType = new FName("MaterialTextureInfo");
        public override bool HasCustomStructSerialization { get { return true; } }
        public override FName PropertyType { get { return CurrentPropertyType; } }

        public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
        {
            if (includeHeader)
            {
                reader.ReadByte();
            }

            FMaterialTextureInfo data = new FMaterialTextureInfo {
                SamplingScale = reader.ReadSingle(),
                UVChannelIndex = reader.ReadInt32(),
                TextureName = reader.ReadFName()
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