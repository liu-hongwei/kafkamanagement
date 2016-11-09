// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetCommitRequestV1.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The version 1 of request to commit offset.
    /// supported in kafka 0.8.2 or later.
    /// </summary>
    public class KafkaProtocolOffsetCommitRequestV1 : KafkaProtocolOffsetCommitRequest
    {
        /// <summary>
        /// Gets or sets the group generation id.
        /// int32
        /// </summary>
        public int ConsumerGroupGenerationId { get; set; }

        /// <summary>
        /// Gets or sets the consumer id.
        /// </summary>
        public string ConsumerId { get; set; }

        /// <summary>
        /// Gets the size of the request.
        /// </summary>
        protected override int BodySize
        {
            get
            {
                // TODO : to calculate the packet size in bytes
                return 0;
            }
        }

        /// <summary>
        /// Gets the bytes of the request packet.
        /// </summary>
        protected override byte[] BodyBytes
        {
            get
            {
                // TODO : to serialize the request to bytes
                return null;
            }
        }

        /// <summary>
        /// Return the version of the request.
        /// </summary>
        /// <returns>The version of the request.</returns>
        public override KafkaProtocolApiVersion GetRequestVersion()
        {
            return KafkaProtocolApiVersion.V1;
        }
    }
}