﻿using Newtonsoft.Json;

namespace UAssetAPI.Kismet.Bytecode.Expressions
{
    /// <summary>
    /// A single Kismet bytecode instruction, corresponding to the <see cref="EExprToken.EX_InterfaceContext"/> instruction.
    /// </summary>
    public class EX_InterfaceContext : KismetExpression
    {
        /// <summary>
        /// The token of this expression.
        /// </summary>
        public override EExprToken Token { get { return EExprToken.EX_InterfaceContext; } }

        /// <summary>
        /// Interface value.
        /// </summary>
        [JsonProperty]
        public KismetExpression InterfaceValue;

        public EX_InterfaceContext()
        {

        }

        /// <summary>
        /// Reads out the expression from a BinaryReader.
        /// </summary>
        /// <param name="reader">The BinaryReader to read from.</param>
        public override void Read(AssetBinaryReader reader)
        {
            InterfaceValue = ExpressionSerializer.ReadExpression(reader);
        }

        /// <summary>
        /// Writes the expression to a BinaryWriter.
        /// </summary>
        /// <param name="writer">The BinaryWriter to write from.</param>
        /// <returns>The iCode offset of the data that was written.</returns>
        public override int Write(AssetBinaryWriter writer)
        {
            return ExpressionSerializer.WriteExpression(InterfaceValue, writer);
        }
    }
}
