// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolTopicMetadataResponseBrokerInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The broker info in topic metadata response.
    /// </summary>
    public class KafkaProtocolTopicMetadataResponseBrokerInfo
    {
        /// <summary>
        /// Gets or sets the broker id.
        /// int32
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the broker ip.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port opened by broker.
        /// int32
        /// </summary>
        public int Port { get; set; }
    }
}