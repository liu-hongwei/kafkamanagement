// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerJoinGroupRequestV1.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The version 1 of join in consumer group request.
    /// </summary>
    public class KafkaProtocolConsumerJoinGroupRequestV1 : KafkaProtocolConsumerJoinGroupRequestV0
    {
        /// <summary>
        /// Gets or sets the maximum allowed time in milliseconds before rejoin group.
        /// int32
        /// </summary>
        public int RebalanceTimeout { get; set; }

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
        /// Gets the api version.
        /// </summary>
        public override KafkaProtocolApiVersion ApiVersion
        {
            get
            {
                return KafkaProtocolApiVersion.V1;
            }
        }
    }
}