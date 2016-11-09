// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerMember.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The consumber member info.
    /// </summary>
    public class KafkaProtocolConsumerMember
    {
        /// <summary>
        /// Gets or sets the member id.
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// Gets or sets the member metadata.
        /// </summary>
        public byte[] MemberMetadata { get; set; }
    }
}