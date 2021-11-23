using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using UAssetAPI.Kismet.Bytecode;
using UAssetAPI.UnrealTypes;

namespace UAssetAPI.DataAccess
{
    public class AssetBinaryReader : BinaryReader
    {
        public UAsset Asset;

        public AssetBinaryReader(Stream stream, UAsset asset) : base(stream)
        {
            Asset = asset;
        }

        private byte[] ReverseIfBigEndian(byte[] data)
        {
            if (!BitConverter.IsLittleEndian) Array.Reverse(data);
            return data;
        }

        public override short ReadInt16()
        {
            return BitConverter.ToInt16(ReverseIfBigEndian(base.ReadBytes(2)), 0);
        }

        public override ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(ReverseIfBigEndian(base.ReadBytes(2)), 0);
        }

        public override int ReadInt32()
        {
            return BitConverter.ToInt32(ReverseIfBigEndian(base.ReadBytes(4)), 0);
        }

        public override uint ReadUInt32()
        {
            return BitConverter.ToUInt32(ReverseIfBigEndian(base.ReadBytes(4)), 0);
        }

        public override long ReadInt64()
        {
            return BitConverter.ToInt64(ReverseIfBigEndian(base.ReadBytes(8)), 0);
        }

        public override ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(ReverseIfBigEndian(base.ReadBytes(8)), 0);
        }

        public override float ReadSingle()
        {
            return BitConverter.ToSingle(ReverseIfBigEndian(base.ReadBytes(4)), 0);
        }

        public override double ReadDouble()
        {
            return BitConverter.ToDouble(ReverseIfBigEndian(base.ReadBytes(8)), 0);
        }

        public override string ReadString()
        {
            return ReadFString()?.Value;
        }

        public virtual FString ReadFString()
        {
            int length = ReadInt32();
            switch (length)
            {
                case 0:
                    return null;
                default:
                    if (length < 0)
                    {
                        byte[] data = ReadBytes(-length * 2);
                        return new FString(Encoding.Unicode.GetString(data, 0, data.Length - 2), Encoding.Unicode);
                    }
                    else
                    {
                        byte[] data = ReadBytes(length);
                        return new FString(Encoding.ASCII.GetString(data, 0, data.Length - 1), Encoding.ASCII);
                    }
            }
        }

        public virtual FString ReadNameMapString(out uint hashes)
        {
            FString str = ReadFString();
            if (!string.IsNullOrEmpty(str.Value))
            {
                hashes = ReadUInt32();
            }
            else
            {
                hashes = 0;
            }
            return str;
        }

        public virtual FName ReadFName()
        {
            int nameMapPointer = ReadInt32();
            int number = ReadInt32();
            return new FName(Asset.GetNameReference(nameMapPointer), number);
        }

        public string XFERSTRING()
        {
            List<byte> readData = new();
            while (true)
            {
                byte newVal = ReadByte();
                if (newVal == 0) break;
                readData.Add(newVal);
            }
            return Encoding.ASCII.GetString(readData.ToArray());
        }

        public string XFERUNICODESTRING()
        {
            List<byte> readData = new();
            while (true)
            {
                byte newVal1 = ReadByte();
                byte newVal2 = ReadByte();
                if (newVal1 == 0 && newVal2 == 0) break;
                readData.Add(newVal1);
                readData.Add(newVal2);
            }
            return Encoding.Unicode.GetString(readData.ToArray());
        }

        public void XFERTEXT()
        {

        }

        public FName XFERNAME()
        {
            return ReadFName();
        }

        public FName XFER_FUNC_NAME()
        {
            return XFERNAME();
        }

        public FPackageIndex XFERPTR()
        {
            return new(ReadInt32());
        }

        public FPackageIndex XFER_FUNC_POINTER()
        {
            return XFERPTR();
        }

        public KismetPropertyPointer XFER_PROP_POINTER()
        {
            if (Asset.EngineVersion >= KismetPropertyPointer.XFER_PROP_POINTER_SWITCH_TO_SERIALIZING_AS_FIELD_PATH_VERSION)
            {
                int numEntries = ReadInt32();
                FName[] allNames = new FName[numEntries];
                for (int i = 0; i < numEntries; i++)
                {
                    allNames[i] = ReadFName();
                }
                FPackageIndex owner = XFER_OBJECT_POINTER();
                return new KismetPropertyPointer(new FFieldPath(allNames, owner));
            }

            return new KismetPropertyPointer(XFERPTR());
        }

        public FPackageIndex XFER_OBJECT_POINTER()
        {
            return XFERPTR();
        }

        public KismetExpression[] ReadExpressionArray(EExprToken endToken)
        {
            List<KismetExpression> newData = new();
            KismetExpression currExpression = null;
            while (currExpression == null || currExpression.Token != endToken)
            {
                if (currExpression != null) newData.Add(currExpression);
                currExpression = ExpressionSerializer.ReadExpression(this);
            }
            return newData.ToArray();
        }
    }
}
