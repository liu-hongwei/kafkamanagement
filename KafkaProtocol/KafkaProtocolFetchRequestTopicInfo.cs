// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolFetchRequestTopicInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The topic info in the fetch request.
    /// </summary>
    public class KafkaProtocolFetchRequestTopicInfo
    {
        /// <summary>
        /// Gets or sets the topic name.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Gets or sets the partition info.
        /// </summary>
        public KafkaProtocolFetchRequestPartitionInfo[] PartitionInfos { get; set; }
    }
}