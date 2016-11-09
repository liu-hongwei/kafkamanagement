// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetCommitRequestTopicInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The topic info in the request to commit offset.
    /// </summary>
    public class KafkaProtocolOffsetCommitRequestTopicInfo
    {
        /// <summary>
        /// Gets or sets the name of topic.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Gets or sets the partition info in the request.
        /// </summary>
        public KafkaProtocolOffsetCommitRequestPartitionInfo[] PartitionInfos { get; set; }
    }
}