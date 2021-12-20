using System;
using System.Text;

using Newtonsoft.Json;

using UAssetAPI.JSON;

namespace UAssetAPI.UnrealTypes
{
    /// <summary>
    /// Unreal string - consists of a string and an encoding
    /// </summary>
    [JsonConverter(typeof(FStringJsonConverter))]
    public class FString : ICloneable
    {
        public string Value;
        public Encoding Encoding;
        public static readonly string NullCase = "null";

        public override string ToString()
        {
            return Value ?? NullCase;
        }

        public override bool Equals(object obj)
        {
            if (obj is FString fStr)
            {
                if (fStr == null)
                    return false;
                return Value == fStr.Value && Encoding == fStr.Encoding;
            }

            if (obj is string str)
            {
                return Value == str;
            }

            return false;
        }

        public static bool operator ==(FString one, FString two)
        {
            if (one is null || two is null) return one is null && two is null;
            return one.Equals(two);
        }

        public static bool operator !=(FString one, FString two)
        {
            if (one is null || two is null) return !(one is null && two is null);
            return !one.Equals(two);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public object Clone()
        {
            return new FString(Value, null, Encoding);
        }

        public static FString FromString(string value, UAsset asset = null, Encoding encoding = null)
        {
            return value == NullCase
                ? null
                : new FString(value, asset, encoding);
        }

        public FString(string value, UAsset asset = null, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8.GetByteCount(value) == value.Length
                ? Encoding.ASCII
                : Encoding.Unicode;

            Value = value;
            Encoding = encoding;

            asset?.AddNameReference(this);
        }
    }
}
