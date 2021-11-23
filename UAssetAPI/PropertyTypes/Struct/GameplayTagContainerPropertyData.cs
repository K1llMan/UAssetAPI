using System;

using UAssetAPI.DataAccess;
using UAssetAPI.UnrealTypes;

namespace UAssetAPI.PropertyTypes.Struct
{
    public class GameplayTagContainerPropertyData : PropertyData<FName[]>
    {
        public GameplayTagContainerPropertyData(FName name) : base(name)
        {
            Value = new FName[0];
        }

        public GameplayTagContainerPropertyData()
        {
            Value = new FName[0];
        }

        private static readonly FName CurrentPropertyType = new("GameplayTagContainer");
        public override bool HasCustomStructSerialization => true;
        public override FName PropertyType => CurrentPropertyType;

        public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
        {
            if (includeHeader)
            {
                reader.ReadByte();
            }

            int numEntries = reader.ReadInt32();
            Value = new FName[numEntries];
            for (int i = 0; i < numEntries; i++)
            {
                Value[i] = reader.ReadFName();
            }
        }

        public override int Write(AssetBinaryWriter writer, bool includeHeader)
        {
            if (includeHeader)
            {
                writer.Write((byte)0);
            }

            writer.Write(Value.Length);
            int totalSize = sizeof(int);
            for (int i = 0; i < Value.Length; i++)
            {
                writer.Write(Value[i]);
                totalSize += sizeof(int) * 2;
            }
            return totalSize;
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

        protected override void HandleCloned(PropertyData res)
        {
            GameplayTagContainerPropertyData cloningProperty = (GameplayTagContainerPropertyData)res;

            FName[] newData = new FName[Value.Length];
            for (int i = 0; i < Value.Length; i++)
            {
                newData[i] = (FName)Value[i].Clone();
            }
            cloningProperty.Value = newData;
        }
    }
}