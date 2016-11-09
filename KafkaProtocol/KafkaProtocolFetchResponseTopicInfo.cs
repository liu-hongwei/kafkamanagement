// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolFetchResponseTopicInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The topic info in the fetch response .
    /// </summary>
    public class KafkaProtocolFetchResponseTopicInfo
    {
        /// <summary>
        /// Gets or sets the topic name of this response.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Gets or sets the partition info for this topic in response.
        /// </summary>
        public KafkaProtocolFetchResponsePartitionInfo[] PartitionInfos { get; set; }
    }
}