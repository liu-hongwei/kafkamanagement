// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaTopicInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    /// <summary>
    /// The kafka topic information.
    /// </summary>
    public class KafkaTopicInfo
    {
        /// <summary>
        /// Gets or sets the name of topic.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the version of zookeeper node for topic node.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the partition infos for topic.
        /// </summary>
        public KafkaPartitionInfo[] Partitions { get; set; }
    }
}