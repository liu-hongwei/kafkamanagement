// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetFetchRequestTopicInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The topic info in the request to fetch offset.
    /// </summary>
    public class KafkaProtocolOffsetFetchRequestTopicInfo
    {
        /// <summary>
        /// Gets or sets the name of topic.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Gets or sets the ids of partitions to fetch offset.
        /// [int32]
        /// </summary>
        public int[] Partitions { get; set; }
    }
}