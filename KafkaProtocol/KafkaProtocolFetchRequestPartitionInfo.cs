// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolFetchRequestPartitionInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The partition info in the fetch request.
    /// </summary>
    public class KafkaProtocolFetchRequestPartitionInfo
    {
        /// <summary>
        /// Gets or sets the partition id.
        /// int32
        /// The id of the partition the fetch is for.
        /// </summary>
        public int Partition { get; set; }

        /// <summary>
        /// Gets or sets the offset to begin.
        /// int64
        /// The offset to begin this fetch from.
        /// </summary>
        public int FetchOffset { get; set; }

        /// <summary>
        /// Gets or sets the maximum bytes to read from this partition.
        /// int32
        /// The maximum bytes to include in the message set for this partition. This helps bound the size of the response.
        /// </summary>
        public int MaxBytes { get; set; }
    }
}