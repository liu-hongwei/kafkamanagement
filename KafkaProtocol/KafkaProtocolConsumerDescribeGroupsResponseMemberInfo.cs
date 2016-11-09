// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerDescribeGroupsResponseMemberInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The consumer group member info in response to the describe group request.
    /// </summary>
    public class KafkaProtocolConsumerDescribeGroupsResponseMemberInfo
    {
        /// <summary>
        /// Gets or sets the member id.
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client host info.
        /// </summary>
        public string ClientHost { get; set; }

        /// <summary>
        /// Gets or sets the member metadata.
        /// </summary>
        public byte[] MemberMetadata { get; set; }

        /// <summary>
        /// Gets or sets the member assignment.
        /// </summary>
        public byte[] MemberAssignment { get; set; }
    }
}