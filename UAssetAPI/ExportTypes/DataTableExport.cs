using System.Collections.Generic;

using UAssetAPI.DataAccess;
using UAssetAPI.PropertyTypes;
using UAssetAPI.PropertyTypes.Simple;
using UAssetAPI.PropertyTypes.Struct;
using UAssetAPI.UnrealTypes;

namespace UAssetAPI.ExportTypes
{
    /// <summary>
    /// Imported spreadsheet table.
    /// </summary>
    public class UDataTable
    {
        public List<StructPropertyData> Data;

        public UDataTable()
        {
            Data = new List<StructPropertyData>();
        }

        public UDataTable(List<StructPropertyData> data)
        {
            Data = data;
        }
    }

    /// <summary>
    /// Export for an imported spreadsheet table. See <see cref="UDataTable"/>.
    /// </summary>
    public class DataTableExport : NormalExport
    {
        public UDataTable Table;

        public DataTableExport(Export super) : base(super)
        {

        }

        public DataTableExport(UDataTable uData, UAsset asset, byte[] extras) : base(asset, extras)
        {
            Table = uData;
        }

        public DataTableExport()
        {

        }

        public override void Read(AssetBinaryReader reader, int nextStarting)
        {
            base.Read(reader, nextStarting);

            // Find an ObjectProperty named RowStruct
            FName decidedStructType = new("Generic");
            foreach (PropertyData thisData in Data)
            {
                if (thisData.Name.Value.Value == "RowStruct" && thisData is ObjectPropertyData thisObjData && thisObjData.Value.IsImport())
                {
                    decidedStructType = thisObjData.ToImport(reader.Asset).ObjectName;
                    break;
                }
            }

            reader.ReadInt32();

            Table = new UDataTable();

            int numEntries = reader.ReadInt32();
            for (int i = 0; i < numEntries; i++)
            {
                FName rowName = reader.ReadFName();
                StructPropertyData nextStruct = new(rowName)
                {
                    StructType = decidedStructType
                };
                nextStruct.Read(reader, false, 1);
                Table.Data.Add(nextStruct);
            }
        }

        public override void Write(AssetBinaryWriter writer)
        {
            base.Write(writer);

            // Find an ObjectProperty named RowStruct
            FName decidedStructType = new("Generic");
            foreach (PropertyData thisData in Data)
            {
                if (thisData.Name.Value.Value == "RowStruct" && thisData is ObjectPropertyData thisObjData)
                {
                    decidedStructType = thisObjData.ToImport(writer.Asset).ObjectName;
                    break;
                }
            }

            writer.Write((int)0);

            writer.Write(Table.Data.Count);
            for (int i = 0; i < Table.Data.Count; i++)
            {
                StructPropertyData thisDataTableEntry = Table.Data[i];
                thisDataTableEntry.StructType = decidedStructType;
                writer.Write(thisDataTableEntry.Name);
                thisDataTableEntry.Write(writer, false);
            }
        }
    }
}
