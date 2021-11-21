namespace UAssetAPI.StructTypes
{
    /// <summary>
    /// A struct that contains a string reference to a class. Can be used to make soft references to classes.
    /// </summary>
    public class SoftClassPathPropertyData : SoftObjectPathPropertyData
    {
        public SoftClassPathPropertyData(FName name) : base(name)
        {

        }

        public SoftClassPathPropertyData()
        {

        }

        private static readonly FName CurrentPropertyType = new("SoftClassPath");
        public override bool HasCustomStructSerialization => true;
        public override FName PropertyType => CurrentPropertyType;
    }
}
