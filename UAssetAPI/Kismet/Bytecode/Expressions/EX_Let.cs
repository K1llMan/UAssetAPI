﻿using Newtonsoft.Json;

namespace UAssetAPI.Kismet.Bytecode.Expressions
{
    /// <summary>
    /// A single Kismet bytecode instruction, corresponding to the <see cref="EExprToken.EX_Let"/> instruction.
    /// </summary>
    public class EX_Let : KismetExpression
    {
        /// <summary>
        /// The token of this expression.
        /// </summary>
        public override EExprToken Token => EExprToken.EX_Let;

        /// <summary>
        /// A pointer to the variable.
        /// </summary>
        [JsonProperty]
        public KismetPropertyPointer Value;

        public EX_Let()
        {

        }

        /// <summary>
        /// Reads out the expression from a BinaryReader.
        /// </summary>
        /// <param name="reader">The BinaryReader to read from.</param>
        public override void Read(AssetBinaryReader reader)
        {
            Value = reader.XFER_PROP_POINTER();
        }

        /// <summary>
        /// Writes the expression to a BinaryWriter.
        /// </summary>
        /// <param name="writer">The BinaryWriter to write from.</param>
        /// <returns>The iCode offset of the data that was written.</returns>
        public override int Write(AssetBinaryWriter writer)
        {
            return writer.XFER_PROP_POINTER(Value);
        }
    }
}
