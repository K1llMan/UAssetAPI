using System.IO;

namespace UAssetAPI
{
    /// <summary>
    /// Export data for a MaterialInstanceConstant
    /// </summary>
    public class MaterialInstanceConstant : StructExport
    {
        public MaterialInstanceConstant(Export super) : base(super)
        {
            Asset = super.Asset;
            Extras = super.Extras;
        }

        public MaterialInstanceConstant(UAsset asset, byte[] extras) : base(asset, extras)
        {

        }

        public MaterialInstanceConstant()
        {

        }

        public override void Read(AssetBinaryReader reader, int nextStarting)
        {
            base.Read(reader, nextStarting);

            // TODO
        }

        public override void Write(AssetBinaryWriter writer)
        {
            base.Write(writer);
            
            // TODO
        }
    }
}
