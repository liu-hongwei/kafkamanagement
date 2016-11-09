// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetCommitRequestPartitionInfoV1.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The version 1 of partition info in the request to commit offset.
    /// </summary>
    public class KafkaProtocolOffsetCommitRequestPartitionInfoV1 : KafkaProtocolOffsetCommitRequestPartitionInfo
    {
        /// <summary>
        /// Gets or sets the timestamp.
        /// int64
        /// Timestamp of the commit
        /// </summary>
        public int TimeStamp { get; set; }
    }
}