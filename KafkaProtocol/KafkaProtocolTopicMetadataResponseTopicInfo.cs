// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolTopicMetadataResponseTopicInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The topic metadata in topic metadata response.
    /// </summary>
    public class KafkaProtocolTopicMetadataResponseTopicInfo
    {
        /// <summary>
        /// Gets or sets the error code.
        /// int16
        /// </summary>
        public int TopicErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the name of topic.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Gets or sets the partition metadatas of current topic.
        /// </summary>
        public KafkaProtocolTopicMetadataResponsePartitionInfo[] PartitionInfos { get; set; }
    }
}