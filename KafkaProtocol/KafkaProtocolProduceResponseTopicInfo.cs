// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolProduceResponseTopicInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The topic info in the produce response.
    /// </summary>
    public abstract class KafkaProtocolProduceResponseTopicInfo
    {
        /// <summary>
        /// Gets or sets the name of the topic.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Gets or sets the partition info.
        /// </summary>
        public KafkaProtocolProduceResponsePartitionInfo[] PartitionInfos { get; set; }
    }
}