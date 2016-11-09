// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerSyncGroupAssignment.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The kafka consumer group assignment info.
    /// </summary>
    public class KafkaProtocolConsumerSyncGroupAssignment
    {
        /// <summary>
        /// Gets or sets the member id.
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// Gets or sets the member assignment.
        /// </summary>
        public byte[] MemberAssignment { get; set; }
    }
}