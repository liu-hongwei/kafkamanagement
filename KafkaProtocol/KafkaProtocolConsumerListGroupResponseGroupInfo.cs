// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerListGroupResponseGroupInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The consumer group info in list group response.
    /// </summary>
    public class KafkaProtocolConsumerListGroupResponseGroupInfo
    {
        /// <summary>
        /// Gets or sets the gorup id.
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the protocol type.
        /// </summary>
        public string ProtocolType { get; set; }
    }
}