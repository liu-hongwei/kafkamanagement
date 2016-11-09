// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetCommitResponseTopicInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The topic info in the response of offset commit.
    /// </summary>
    public class KafkaProtocolOffsetCommitResponseTopicInfo
    {
        /// <summary>
        /// Gets or sets the topic name.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Gets or sets the partition info in the response of offset commit.
        /// </summary>
        public KafkaProtocolOffsetCommitResponsePartitionInfo[] PartitionInfos { get; set; }
    }
}