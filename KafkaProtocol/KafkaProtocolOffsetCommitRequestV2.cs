// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetCommitRequestV2.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The version 2 of request to commit offset.
    /// supported in kafka 0.9.0 or later.
    /// </summary>
    public class KafkaProtocolOffsetCommitRequestV2 : KafkaProtocolOffsetCommitRequestV1
    {
        /// <summary>
        /// Gets or sets the offset retention time.
        /// int64
        /// Time period in ms to retain the offset.
        /// </summary>
        public int RetentionTime { get; set; }

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
            return KafkaProtocolApiVersion.V2;
        }
    }
}