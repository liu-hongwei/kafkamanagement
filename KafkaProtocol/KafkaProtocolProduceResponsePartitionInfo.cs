// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolProduceResponsePartitionInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The base model of partition info in the produce response.
    /// </summary>
    public abstract class KafkaProtocolProduceResponsePartitionInfo
    {
        /// <summary>
        /// Gets or sets the id of the partition for the response.
        /// int32
        /// The partition this response entry corresponds to.
        /// </summary>
        public int Partition { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// int16
        /// The error from this partition, if any. Errors are given on a per-partition basis because a given partition may be unavailable or 
        /// maintained on a different host, while others may have successfully accepted the produce request.
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the offset of first message.
        /// int64
        /// The offset assigned to the first message in the message set appended to this partition.
        /// </summary>
        public int Offset { get; set; }
    }
}