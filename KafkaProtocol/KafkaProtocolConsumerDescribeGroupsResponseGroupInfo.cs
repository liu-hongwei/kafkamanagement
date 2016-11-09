// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerDescribeGroupsResponseGroupInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The consumer group info in response to group describe request.
    /// </summary>
    public class KafkaProtocolConsumerDescribeGroupsResponseGroupInfo
    {
        /// <summary>
        /// Gets or sets the error code.
        /// int16
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the consumer group id.
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the protocol type.
        /// </summary>
        public string ProtocolType { get; set; }

        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the group members info.
        /// </summary>
        public KafkaProtocolConsumerDescribeGroupsResponseMemberInfo[] Members { get; set; }
    }
}