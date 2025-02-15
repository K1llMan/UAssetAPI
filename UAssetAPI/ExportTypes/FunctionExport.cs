using System.IO;

namespace UAssetAPI
{
    /// <summary>
    /// Export data for a blueprint function.
    /// </summary>
    public class FunctionExport : StructExport
    {
        public FunctionExport(Export super) : base(super)
        {
            Asset = super.Asset;
            Extras = super.Extras;
        }

        public FunctionExport(UAsset asset, byte[] extras) : base(asset, extras)
        {

        }

        public FunctionExport()
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
