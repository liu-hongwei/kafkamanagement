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