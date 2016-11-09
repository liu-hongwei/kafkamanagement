// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolProduceRequestTopicInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The topic info in the produce request.
    /// </summary>
    public class KafkaProtocolProduceRequestTopicInfo
    {
        /// <summary>
        /// Gets or sets the name of topic.
        /// The topic that data is being published to.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Gets or sets the partitions which data will be written.
        /// </summary>
        public KafkaProtocolProduceRequestPartitionInfo[] PartitonInfos { get; set; }
    }
}