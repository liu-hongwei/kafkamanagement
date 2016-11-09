// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetCommitRequestPartitionInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The partition info in the request to commit offset.
    /// </summary>
    public abstract class KafkaProtocolOffsetCommitRequestPartitionInfo
    {
        /// <summary>
        /// Gets or sets the partition id.
        /// int32
        /// </summary>
        public int Partition { get; set; }

        /// <summary>
        /// Gets or sets  the offset.
        /// int64
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the metadata client set.
        /// Any associated metadata the client wants to keep.
        /// </summary>
        public string Metadata { get; set; }
    }
}