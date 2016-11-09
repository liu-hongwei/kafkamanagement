// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerPartitionAssignment.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The kafka consumer partition assignment info.
    /// </summary>
    public class KafkaProtocolConsumerPartitionAssignment
    {
        /// <summary>
        /// Gets or sets the topic name.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Gets or sets the id of partition.
        /// int32
        /// </summary>
        public int Partition { get; set; }
    }
}