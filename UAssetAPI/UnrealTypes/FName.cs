﻿using System;

using Newtonsoft.Json;

using UAssetAPI.JSON;

namespace UAssetAPI.UnrealTypes
{
    /// <summary>
    /// Unreal name - consists of an FString (which is serialized as an index in the name map) and an instance number
    /// </summary>
    [JsonConverter(typeof(FNameJsonConverter))]
    public class FName : ICloneable
    {
        public FString Value;
        /// <summary>Instance number.</summary>
        public int Number;

        /// <summary>
        /// Converts this FName instance into a human-readable string. This is the inverse of <see cref="FromString(string)"/>.
        /// </summary>
        /// <returns>The human-readable string that represents this FName.</returns>
        /// <remarks>
        /// The human-readable string is formatted as the string representation of the <see cref="Value"/> followed by the instance number <see cref="Number"/> in parentheses. In some special cases, the number in parentheses will be omitted, in which case it is safe to assume that it is zero.
        /// </remarks>
        public override string ToString()
        {
            if (Number == int.MinValue) 
                return Value.ToString();
            return Value + "(" + Number + ")";
        }

        /// <summary>
        /// Converts a human-readable string into an FName instance. This is the inverse of <see cref="ToString"/>.
        /// </summary>
        /// <param name="val">The human-readable string to convert into an FName instance.</param>
        /// <param name="asset">Asset to store string</param>
        /// <returns>An FName instance that this string represents.</returns>
        /// <remarks>
        /// If the string ends in a decimal number surrounded by parentheses, such as in alphabet(2), the number inside parentheses (2) will be used as the instance number and the rest of the string will be used as the value (alphabet).
        /// Otherwise, the string itself will become the value of the new instance, and the instance number will default to zero.
        /// </remarks>
        public static FName FromString(string val, UAsset asset = null)
        {
            if (val == null || val == "null") 
                return null;
            if (val.Length == 0 || val[val.Length - 1] != ')') 
                return new FName(val, 0, asset);

            int locLastLeftBracket = val.LastIndexOf('(');
            if (locLastLeftBracket < 0) 
                return new FName(val, 0, asset);

            string discriminatorRaw = val.Substring(locLastLeftBracket + 1, val.Length - locLastLeftBracket - 2);
            if (!int.TryParse(discriminatorRaw, out int discriminator)) 
                return new FName(val, 0, asset);

            string realStr = val.Substring(0, locLastLeftBracket);
            return new FName(realStr, discriminator, asset);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FName name)) 
                return false;
            return (Value == name.Value || Value.Value == name.Value.Value) && Number == name.Number;
        }

        public static bool operator ==(FName one, FName two)
        {
            if (one is null || two is null) 
                return one is null && two is null;
            return one.Equals(two);
        }

        public static bool operator !=(FName one, FName two)
        {
            if (one is null || two is null) 
                return !(one is null && two is null);
            return !one.Equals(two);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ Number.GetHashCode();
        }

        public object Clone()
        {
            return new FName((FString)Value.Clone(), Number);
        }

        /// <summary>
        /// Creates a new FName instance.
        /// </summary>
        /// <param name="value">The string literal that the new FName's value will be, verbatim.</param>
        /// <param name="number">The instance number of the new FName.</param>
        /// <param name="asset">Asset to store string</param>
        public FName(string value, int number = 0, UAsset asset = null)
        {
            Value = value == null 
                ? new FString(null) 
                : new FString(value, asset);
            Number = number;
        }

        /// <summary>
        /// Creates a new FName instance.
        /// </summary>
        /// <param name="value">The FString that the FName's value will be, verbatim.</param>
        /// <param name="number">The instance number of the new FName.</param>
        public FName(FString value, int number = 0)
        {
            Value = value;
            Number = number;
        }

        /// <summary>
        /// Creates a new blank FName instance.
        /// </summary>
        public FName()
        {
            Value = new FString(string.Empty);
            Number = 0;
        }
    }
}
