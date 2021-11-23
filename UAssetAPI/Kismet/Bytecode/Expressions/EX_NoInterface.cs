﻿using UAssetAPI.DataAccess;

namespace UAssetAPI.Kismet.Bytecode.Expressions
{
    /// <summary>
    /// A single Kismet bytecode instruction, corresponding to the <see cref="EExprToken.EX_NoInterface"/> instruction.
    /// </summary>
    public class EX_NoInterface : KismetExpression
    {
        /// <summary>
        /// The token of this expression.
        /// </summary>
        public override EExprToken Token => EExprToken.EX_NoInterface;

        public EX_NoInterface()
        {

        }

        /// <summary>
        /// Reads out the expression from a BinaryReader.
        /// </summary>
        /// <param name="reader">The BinaryReader to read from.</param>
        public override void Read(AssetBinaryReader reader)
        {

        }

        /// <summary>
        /// Writes the expression to a BinaryWriter.
        /// </summary>
        /// <param name="writer">The BinaryWriter to write from.</param>
        /// <returns>The iCode offset of the data that was written.</returns>
        public override int Write(AssetBinaryWriter writer)
        {
            return 0;
        }
    }
}
