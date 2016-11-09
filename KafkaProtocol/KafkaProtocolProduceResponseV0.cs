// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolProduceResponseV0.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The version 0 of the response to produce request.
    /// supported in kafka version older than 0.9.0
    /// </summary>
    public class KafkaProtocolProduceResponseV0 : KafkaProtocolProduceResponse
    {
        /// <summary>
        /// Parse the response bytes.
        /// </summary>
        /// <param name="response">The bytes of the response.</param>
        public override void Parse(byte[] response)
        {
            // TODO : deserialize the bytes into response
        }

    }
}