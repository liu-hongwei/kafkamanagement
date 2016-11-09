// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolProduceResponseV1.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The version 1 of response to produce request.
    /// supported in kafka 0.9.0 or later.
    /// </summary>
    public class KafkaProtocolProduceResponseV1 : KafkaProtocolProduceResponse
    {
        /// <summary>
        /// Gets or sets the milliseconds for throttling.
        /// int32
        /// Duration in milliseconds for which the request was throttled due to quota violation. (Zero if the request did not violate any quota).
        /// </summary>
        public int ThrottleTime { get; set; }

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