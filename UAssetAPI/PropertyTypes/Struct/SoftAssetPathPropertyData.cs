using UAssetAPI.UnrealTypes;

namespace UAssetAPI.PropertyTypes.Struct
{
    public class SoftAssetPathPropertyData : SoftObjectPathPropertyData
    {
        public SoftAssetPathPropertyData(FName name) : base(name)
        {

        }

        public SoftAssetPathPropertyData()
        {

        }

        private static readonly FName CurrentPropertyType = new("SoftAssetPath");
        public override bool HasCustomStructSerialization => true;
        public override FName PropertyType => CurrentPropertyType;
    }
}
