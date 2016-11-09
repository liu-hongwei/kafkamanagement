// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetFetchResponseTopicInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The topic info in the response of fetching offset.
    /// </summary>
    public class KafkaProtocolOffsetFetchResponseTopicInfo
    {
        /// <summary>
        /// Gets or sets the name of topic in the response.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Gets or sets the partition info in the response.
        /// </summary>
        public KafkaProtocolOffsetFetchResponsePartitionInfo[] PartitionInfos { get; set; }
    }
}