// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetCommitResponsePartitionInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The partition info in the response of commit offset request.
    /// </summary>
    public class KafkaProtocolOffsetCommitResponsePartitionInfo
    {
        /// <summary>
        /// Gets or sets the id of partition.
        /// int32
        /// </summary>
        public int Partition { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// int16
        /// </summary>
        public int ErrorCode { get; set; }
    }
}