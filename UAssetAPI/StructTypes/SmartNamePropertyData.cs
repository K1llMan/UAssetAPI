﻿using System;
using UAssetAPI.PropertyTypes;

namespace UAssetAPI.StructTypes
{
    /// <summary>
    /// Special FName struct used within animations.
    /// </summary>
    public class SmartNamePropertyData : PropertyData
    {
        /// <summary>
        /// The display name of this FSmartName.
        /// </summary>
        public FName DisplayName;

        /// <summary>
        /// SmartName::UID_Type - for faster access
        /// </summary>
        public ushort SmartNameID;

        /// <summary>
        /// Uncertain
        /// </summary>
        public Guid TempGUID;

        public SmartNamePropertyData(FName name) : base(name)
        {

        }

        public SmartNamePropertyData()
        {

        }

        private static readonly FName CurrentPropertyType = new("SmartName");
        public override bool HasCustomStructSerialization => true;
        public override FName PropertyType => CurrentPropertyType;

        public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
        {
            if (includeHeader)
            {
                reader.ReadByte();
            }

            DisplayName = reader.ReadFName();
            if (reader.Asset.GetCustomVersion<FAnimPhysObjectVersion>() < FAnimPhysObjectVersion.RemoveUIDFromSmartNameSerialize)
            {
                SmartNameID = reader.ReadUInt16();
            }
            if (reader.Asset.GetCustomVersion<FAnimPhysObjectVersion>() < FAnimPhysObjectVersion.SmartNameRefactorForDeterministicCooking)
            {
                TempGUID = new Guid(reader.ReadBytes(16));
            }
        }

        public override int Write(AssetBinaryWriter writer, bool includeHeader)
        {
            if (includeHeader)
            {
                writer.Write((byte)0);
            }

            long here = writer.BaseStream.Position;

            writer.Write(DisplayName);
            if (writer.Asset.GetCustomVersion<FAnimPhysObjectVersion>() < FAnimPhysObjectVersion.RemoveUIDFromSmartNameSerialize)
            {
                writer.Write(SmartNameID);
            }
            if (writer.Asset.GetCustomVersion<FAnimPhysObjectVersion>() < FAnimPhysObjectVersion.SmartNameRefactorForDeterministicCooking)
            {
                writer.Write(TempGUID.ToByteArray());
            }

            return (int)(writer.BaseStream.Position - here);
        }

        public override void FromString(string[] d, UAsset asset)
        {
            DisplayName = FName.FromString(d[0]);
            if (ushort.TryParse(d[1], out ushort rawSmartNameID)) SmartNameID = rawSmartNameID;
            if (Guid.TryParse(d[2], out Guid rawTempGUID)) TempGUID = rawTempGUID;
        }

        public override string ToString()
        {
            return "(" + ")";
        }
    }
}
