// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerGroupCoordinatorResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The response of getting consumer group coordinator request.
    /// </summary>
    public class KafkaProtocolConsumerGroupCoordinatorResponse : KafkaProtocolResponse
    {
        /// <summary>
        /// Gets or sets the error code.
        /// int16
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the coordinator id.
        /// int32
        /// </summary>
        public int CoordinatorId { get; set; }

        /// <summary>
        /// Gets or sets the coordinator host info.
        /// </summary>
        public string CoordinatorHost { get; set; }

        /// <summary>
        /// Gets or sets the coordinator port.
        /// int32
        /// </summary>
        public int CoordinatorPort { get; set; }

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