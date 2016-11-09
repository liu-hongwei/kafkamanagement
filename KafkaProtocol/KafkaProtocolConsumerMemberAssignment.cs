// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerMemberAssignment.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The kafka consumber member assignment info.
    /// </summary>
    public class KafkaProtocolConsumerMemberAssignment
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the partition assignments to consumers.
        /// </summary>
        public KafkaProtocolConsumerPartitionAssignment[] PartitionAssignments { get; set; }

        /// <summary>
        /// Gets or sets the user's data.
        /// </summary>
        public byte[] UserData { get; set; }
    }
}