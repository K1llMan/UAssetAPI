namespace UAssetAPI.StructTypes
{
    public class SkeletalMeshAreaWeightedTriangleSamplerPropertyData : WeightedRandomSamplerPropertyData
    {
        public SkeletalMeshAreaWeightedTriangleSamplerPropertyData(FName name) : base(name)
        {

        }

        public SkeletalMeshAreaWeightedTriangleSamplerPropertyData()
        {

        }

        private static readonly FName CurrentPropertyType = new("SkeletalMeshAreaWeightedTriangleSampler");
        public override bool HasCustomStructSerialization => true;
        public override FName PropertyType => CurrentPropertyType;
    }
}