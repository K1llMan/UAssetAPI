using System;

using Newtonsoft.Json;

using UAssetAPI.UnrealTypes;

namespace UAssetAPI.JSON
{
    public class FNameJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FName);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            FName realVal = (FName)value;
            writer.WriteValue(realVal is null ? "null" : realVal.ToString());
        }

        public override bool CanRead => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return null;
            return FName.FromString(Convert.ToString(reader.Value));
        }
    }
}
