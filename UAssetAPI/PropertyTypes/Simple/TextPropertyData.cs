using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using UAssetAPI.DataAccess;
using UAssetAPI.UnrealTypes;
using UAssetAPI.UnrealTypes.Enums;

namespace UAssetAPI.PropertyTypes.Simple
{
    /// <summary>
    /// Describes an FText.
    /// </summary>
    public class TextPropertyData : PropertyData<FString>
    {
        /// <summary>Flags with various information on what sort of FText this is</summary>
        [JsonProperty]
        public ETextFlag Flags = 0;
        /// <summary>The HistoryType of this FText.</summary>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public TextHistoryType HistoryType = TextHistoryType.Base;
        /// <summary>The string table ID being referenced, if applicable</summary>
        [JsonProperty]
        public FName TableId;
        /// <summary>A namespace to use when parsing texts that use LOCTEXT</summary>
        [JsonProperty]
        public FString Namespace;
        /// <summary>The source string for this FText. In the Unreal Engine, this is also known as SourceString.</summary>
        [JsonProperty]
        public FString CultureInvariantString;

        public bool ShouldSerializeTableId()
        {
            return HistoryType == TextHistoryType.StringTableEntry;
        }

        public TextPropertyData(FName name) : base(name)
        {

        }

        public TextPropertyData()
        {

        }

        private static readonly FName CurrentPropertyType = new("TextProperty");
        public override FName PropertyType => CurrentPropertyType;

        public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
        {
            if (includeHeader)
            {
                reader.ReadByte();
            }

            if (reader.Asset.EngineVersion < UE4Version.VER_UE4_FTEXT_HISTORY)
            {
                CultureInvariantString = reader.ReadFString();
                if (reader.Asset.EngineVersion >= UE4Version.VER_UE4_ADDED_NAMESPACE_AND_KEY_DATA_TO_FTEXT)
                {
                    Namespace = reader.ReadFString();
                    Value = reader.ReadFString();
                }
                else
                {
                    Namespace = null;
                    Value = reader.ReadFString();
                }
            }

            Flags = (ETextFlag)reader.ReadUInt32();

            if (reader.Asset.EngineVersion >= UE4Version.VER_UE4_FTEXT_HISTORY)
            {
                HistoryType = (TextHistoryType)reader.ReadSByte();

                switch (HistoryType)
                {
                    case TextHistoryType.None:
                        Value = null;
                        if (reader.Asset.GetCustomVersion<FEditorObjectVersion>() >= FEditorObjectVersion.CultureInvariantTextSerializationKeyStability)
                        {
                            bool bHasCultureInvariantString = reader.ReadInt32() == 1;
                            if (bHasCultureInvariantString)
                            {
                                CultureInvariantString = reader.ReadFString();
                            }
                        }
                        break;
                    case TextHistoryType.Base:
                        Namespace = reader.ReadFString();
                        Value = reader.ReadFString();
                        CultureInvariantString = reader.ReadFString();
                        break;
                    case TextHistoryType.StringTableEntry:
                        TableId = reader.ReadFName();
                        Value = reader.ReadFString();
                        break;
                    default:
                        throw new NotImplementedException("Unimplemented reader for " + HistoryType);
                }
            }
        }

        public override int Write(AssetBinaryWriter writer, bool includeHeader)
        {
            if (includeHeader)
            {
                writer.Write((byte)0);
            }

            int here = (int)writer.BaseStream.Position;

            if (writer.Asset.EngineVersion < UE4Version.VER_UE4_FTEXT_HISTORY)
            {
                writer.Write(CultureInvariantString);
                if (writer.Asset.EngineVersion >= UE4Version.VER_UE4_ADDED_NAMESPACE_AND_KEY_DATA_TO_FTEXT)
                {
                    writer.Write(Namespace);
                    writer.Write(Value);
                }
                else
                {
                    writer.Write(Value);
                }
            }

            writer.Write((uint)Flags);

            if (writer.Asset.EngineVersion >= UE4Version.VER_UE4_FTEXT_HISTORY)
            {
                writer.Write((sbyte)HistoryType);

                switch (HistoryType)
                {
                    case TextHistoryType.None:
                        if (writer.Asset.GetCustomVersion<FEditorObjectVersion>() >= FEditorObjectVersion.CultureInvariantTextSerializationKeyStability)
                        {
                            if (CultureInvariantString == null || string.IsNullOrEmpty(CultureInvariantString.Value))
                            {
                                writer.Write(0);
                            }
                            else
                            {
                                writer.Write(1);
                                writer.Write(CultureInvariantString);
                            }
                        }
                        break;
                    case TextHistoryType.Base:
                        writer.Write(Namespace);
                        writer.Write(Value);
                        writer.Write(CultureInvariantString);
                        break;
                    case TextHistoryType.StringTableEntry:
                        writer.Write(TableId);
                        writer.Write(Value);
                        break;
                    default:
                        throw new NotImplementedException("Unimplemented writer for " + HistoryType);
                }
            }

            return (int)writer.BaseStream.Position - here;
        }

        public override string ToString()
        {
            if (Value == null) return "null";

            switch (HistoryType)
            {
                case TextHistoryType.None:
                    return "None, " + CultureInvariantString;
                case TextHistoryType.Base:
                    return "Base, " + Namespace + ", " + Value + ", " + CultureInvariantString;
                case TextHistoryType.StringTableEntry:
                    return "StringTableEntry, " + TableId + ", " + Value;
                default:
                    throw new NotImplementedException("Unimplemented display for " + HistoryType);
            }
        }

        public override void FromString(string[] d, UAsset asset)
        {
            throw new NotImplementedException("TextPropertyData.FromString is currently unimplemented");
        }

        protected override void HandleCloned(PropertyData res)
        {
            TextPropertyData cloningProperty = (TextPropertyData)res;

            cloningProperty.TableId = (FName)TableId.Clone();
            cloningProperty.Namespace = (FString)Namespace.Clone();
            cloningProperty.Namespace = (FString)CultureInvariantString.Clone();
        }
    }
}