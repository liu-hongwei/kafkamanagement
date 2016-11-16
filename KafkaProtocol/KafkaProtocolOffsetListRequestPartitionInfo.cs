// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetListRequestPartitionInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The partition info in the request of listing offset.
    /// </summary>
    public class KafkaProtocolOffsetListRequestPartitionInfo
    {
        /// <summary>
        /// Gets or sets the id of partition for listing offset.
        /// int32
        /// </summary>
        public int Partition { get; set; }

        /// <summary>
        /// Gets or sets the timestamp in milliseconds.
        /// int64
        /// Used to ask for all messages before a certain time (ms). There are two special values. 
        /// Specify -1 to receive the latest offset (i.e. the offset of the next coming message) and -2 to receive 
        /// the earliest available offset. 
        /// Note that because offsets are pulled in descending order, asking for the earliest offset will always return you a single element.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of offsets to list.
        /// int32
        /// Maximum offsets to return
        /// </summary>
        public int MaxNumberOfOffsets { get; set; }
    }
}