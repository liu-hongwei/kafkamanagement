// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetListRequestTopicInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The topic info in the request to list offset.
    /// </summary>
    public class KafkaProtocolOffsetListRequestTopicInfo
    {
        /// <summary>
        /// Gets or sets the topic to list offset.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Gets or sets the partition info in the request.
        /// </summary>
        public KafkaProtocolOffsetListRequestPartitionInfo[] PartitionInfos { get; set; }
    }
}