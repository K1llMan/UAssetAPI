using System.Collections.Generic;

using Newtonsoft.Json;

using UAssetAPI.DataAccess;
using UAssetAPI.UnrealTypes;

namespace UAssetAPI.PropertyTypes.Struct
{
    public class BoxPropertyData : PropertyData<VectorPropertyData[]> // Min, Max, IsValid
    {
        [JsonProperty]
        public bool IsValid;

        public BoxPropertyData(FName name) : base(name)
        {

        }

        public BoxPropertyData()
        {

        }

        private static readonly FName CurrentPropertyType = new("Box");
        public override bool HasCustomStructSerialization => true;
        public override FName PropertyType => CurrentPropertyType;

        public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
        {
            if (includeHeader)
            {
                reader.ReadByte();
            }

            Value = new VectorPropertyData[2];
            for (int i = 0; i < 2; i++)
            {
                VectorPropertyData next = new(Name);
                next.Read(reader, false, 0);
                Value[i] = next;
            }

            IsValid = reader.ReadBoolean();
        }

        public override int Write(AssetBinaryWriter writer, bool includeHeader)
        {
            if (includeHeader)
            {
                writer.Write((byte)0);
            }

            int totalSize = 0;
            for (int i = 0; i < 2; i++)
            {
                totalSize += Value[i].Write(writer, includeHeader);
            }
            writer.Write(IsValid);
            return totalSize + sizeof(bool);
        }

        public override void FromString(string[] d, UAsset asset)
        {
            IsValid = d[0].Equals("1") || d[0].ToLower().Equals("true");
        }

        public override string ToString()
        {
            return "(" + string.Join(", ", (IEnumerable<VectorPropertyData>)Value) + ")";
        }

        protected override void HandleCloned(PropertyData res)
        {
            BoxPropertyData cloningProperty = (BoxPropertyData)res;

            VectorPropertyData[] newData = new VectorPropertyData[Value.Length];
            for (int i = 0; i < Value.Length; i++)
            {
                newData[i] = (VectorPropertyData)Value[i].Clone();
            }
            cloningProperty.Value = newData;
        }
    }
}