using System;
using System.IO;
using System.Text;

using UAssetAPI.Kismet.Bytecode;
using UAssetAPI.UnrealTypes;

namespace UAssetAPI.DataAccess
{
    public class AssetBinaryWriter : BinaryWriter
    {
        public UAsset Asset;

        public AssetBinaryWriter(UAsset asset) : base()
        {
            Asset = asset;
        }

        public AssetBinaryWriter(Stream stream, UAsset asset) : base(stream)
        {
            Asset = asset;
        }

        public AssetBinaryWriter(Stream stream, Encoding encoding, UAsset asset) : base(stream, encoding)
        {
            Asset = asset;
        }

        public AssetBinaryWriter(Stream stream, Encoding encoding, bool leaveOpen, UAsset asset) : base(stream, encoding, leaveOpen)
        {
            Asset = asset;
        }

        private byte[] ReverseIfBigEndian(byte[] data)
        {
            if (!BitConverter.IsLittleEndian) Array.Reverse(data);
            return data;
        }

        public override void Write(short value)
        {
            Write(ReverseIfBigEndian(BitConverter.GetBytes(value)));
        }

        public override void Write(ushort value)
        {
            Write(ReverseIfBigEndian(BitConverter.GetBytes(value)));
        }

        public override void Write(int value)
        {
            Write(ReverseIfBigEndian(BitConverter.GetBytes(value)));
        }

        public override void Write(uint value)
        {
            Write(ReverseIfBigEndian(BitConverter.GetBytes(value)));
        }

        public override void Write(long value)
        {
            Write(ReverseIfBigEndian(BitConverter.GetBytes(value)));
        }

        public override void Write(ulong value)
        {
            Write(ReverseIfBigEndian(BitConverter.GetBytes(value)));
        }

        public override void Write(float value)
        {
            Write(ReverseIfBigEndian(BitConverter.GetBytes(value)));
        }

        public override void Write(double value)
        {
            Write(ReverseIfBigEndian(BitConverter.GetBytes(value)));
        }

        public override void Write(string value)
        {
            Write(new FString(value));
        }

        public virtual int Write(FString value)
        {
            switch (value?.Value)
            {
                case null:
                    Write((int)0);
                    return sizeof(int);
                default:
                    string nullTerminatedStr = value.Value + "\0";
                    Write(value.Encoding is UnicodeEncoding ? -nullTerminatedStr.Length : nullTerminatedStr.Length);
                    byte[] actualStrData = value.Encoding.GetBytes(nullTerminatedStr);
                    Write(actualStrData);
                    return actualStrData.Length + 4;
            }
        }

        public virtual void Write(FName name)
        {
            Write(Asset.SearchNameReference(name.Value));
            Write(name.Number);
        }

        public int XFERSTRING(string val)
        {
            long startMetric = BaseStream.Position;
            Write(Encoding.ASCII.GetBytes(val + "\0"));
            return (int)(BaseStream.Position - startMetric);
        }

        public int XFERUNICODESTRING(string val)
        {
            long startMetric = BaseStream.Position;
            Write(Encoding.Unicode.GetBytes(val + "\0"));
            return (int)(BaseStream.Position - startMetric);
        }

        public int XFERNAME(FName val)
        {
            Write(val);
            return 12; // FScriptName's iCode offset is 12 bytes, not 8
        }

        public int XFER_FUNC_NAME(FName val)
        {
            return XFERNAME(val);
        }

        private static readonly int PointerSize = sizeof(ulong);

        public int XFERPTR(FPackageIndex val)
        {
            Write(val.Index);
            return PointerSize; // For the iCode offset, we return the size of a pointer in memory rather than the size of an FPackageIndex on disk
        }

        public int XFER_FUNC_POINTER(FPackageIndex val)
        {
            return XFERPTR(val);
        }

        public int XFER_PROP_POINTER(KismetPropertyPointer val)
        {
            if (Asset.EngineVersion >= KismetPropertyPointer.XFER_PROP_POINTER_SWITCH_TO_SERIALIZING_AS_FIELD_PATH_VERSION)
            {
                Write(val.New.Path.Length);
                for (int i = 0; i < val.New.Path.Length; i++)
                {
                    XFERNAME(val.New.Path[i]);
                }
                XFER_OBJECT_POINTER(val.New.ResolvedOwner);
            }
            else
            {
                XFERPTR(val.Old);
            }
            return PointerSize;
        }

        public int XFER_OBJECT_POINTER(FPackageIndex val)
        {
            return XFERPTR(val);
        }
    }
}
