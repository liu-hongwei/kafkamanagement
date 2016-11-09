// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerJoinGroupRequestV0.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The version 0 request of join in consumer group.
    /// </summary>
    public class KafkaProtocolConsumerJoinGroupRequestV0 : KafkaProtocolConsumerJoinGroupRequest
    {
        /// <summary>
        /// Gets the size of the request.
        /// </summary>
        protected override int BodySize
        {
            get
            {
                return base.BodySize;
            }
        }

        /// <summary>
        /// Gets the api version.
        /// </summary>
        public override KafkaProtocolApiVersion ApiVersion
        {
            get
            {
                return KafkaProtocolApiVersion.V0;
            }
        }
    }
}