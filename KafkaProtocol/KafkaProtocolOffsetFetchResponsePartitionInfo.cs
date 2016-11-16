// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetFetchResponsePartitionInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The partition info in the response of fetching offset.
    /// </summary>
    public class KafkaProtocolOffsetFetchResponsePartitionInfo
    {
        /// <summary>
        /// Gets or sets the partition id.
        /// int32
        /// </summary>
        public int Partition { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// int64
        /// Last committed message offset.
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// string
        /// Any associated metadata the client wants to keep.
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// int16
        /// </summary>
        public int ErrorCode { get; set; }
    }
}