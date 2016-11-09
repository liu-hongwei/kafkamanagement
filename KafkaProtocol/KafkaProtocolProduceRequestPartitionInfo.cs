// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolProduceRequestPartitionInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The partition info in the produce request.
    /// </summary>
    public class KafkaProtocolProduceRequestPartitionInfo
    {
        /// <summary>
        /// Gets or sets the id of partition which data will be written.
        /// int32
        /// The partition that data is being published to.
        /// </summary>
        public int Partition { get; set; }

        /// <summary>
        /// Gets or sets the size of message in bytes.
        /// int32
        /// The size, in bytes, of the message set that follows.
        /// </summary>
        public int MessageSetSize { get; set; }

        /// <summary>
        /// Gets or sets the set of the message.
        /// A set of messages in the standard format described above.
        /// </summary>
        public KafkaProtocolRequestMessageSet MessageSet { get; set; }
    }
}