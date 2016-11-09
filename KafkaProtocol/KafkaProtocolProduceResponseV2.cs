// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolProduceResponseV2.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The version 2 of the response to the produce request.
    /// supported in kafka 0.10.0 or later.
    /// </summary>
    public class KafkaProtocolProduceResponseV2 : KafkaProtocolProduceResponseV1
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