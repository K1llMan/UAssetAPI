namespace UAssetAPI
{
    public class FMaterialParameterInfo
    {
        public FName Name { get; set; }

        public EMaterialParameterAssociation Association { get; set; }

        public int Index { get; set; }


        public FMaterialParameterInfo()
        {
            Name = new FName();
            Association = EMaterialParameterAssociation.LayerParameter;
            Index = 0;
        }
    }
}
