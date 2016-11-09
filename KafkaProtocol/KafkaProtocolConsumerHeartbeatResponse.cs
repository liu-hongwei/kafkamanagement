// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerHeartbeatResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The response to the consumer hearbeat request.
    /// </summary>
    public class KafkaProtocolConsumerHeartbeatResponse : KafkaProtocolResponse
    {
        /// <summary>
        /// Gets or sets the error code.
        /// int16
        /// </summary>
        public int ErrorCode { get; set; }

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