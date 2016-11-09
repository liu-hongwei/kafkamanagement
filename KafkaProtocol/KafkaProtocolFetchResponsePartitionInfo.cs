// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolFetchResponsePartitionInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The partition info in the fetch response.
    /// </summary>
    public class KafkaProtocolFetchResponsePartitionInfo
    {
        /// <summary>
        /// Gets or sets the partition id in the response.
        /// int32
        /// The id of the partition this response is for.
        /// </summary>
        public int Partition { get; set; }

        /// <summary>
        /// Gets or sets the error code. 
        /// int16
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the offset to the end of this partition.
        /// int64
        /// The offset at the end of the log for this partition. This can be used by the client to determine how many messages behind the end of the log they are.
        /// </summary>
        public int HighwaterMarkOffset { get; set; }

        /// <summary>
        /// Gets or sets the message set in bytes.
        /// int32
        /// The size in bytes of the message set for this partition
        /// </summary>
        public int MessageSetSize { get; set; }

        /// <summary>
        /// Gets or sets the data fetched from this partition.
        /// The message data fetched from this partition, in the format described above.
        /// </summary>
        public KafkaProtocolRequestMessageSet MessageSet { get; set; }
    }
}