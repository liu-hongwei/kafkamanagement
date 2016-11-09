// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetListResponsePartitionInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The partition info in the response of listing offset.
    /// </summary>
    public class KafkaProtocolOffsetListResponsePartitionInfo
    {
        /// <summary>
        /// Gets or sets the id of the partition.
        /// int32
        /// </summary>
        public int Partition { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// int16
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the offset found.
        /// [int64]
        /// </summary>
        public int[] Offset { get; set; }
    }
}