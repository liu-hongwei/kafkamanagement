// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerJoinGroupResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The response to the join consumer group request.
    /// </summary>
    public class KafkaProtocolConsumerJoinGroupResponse : KafkaProtocolResponse
    {
        /// <summary>
        /// Gets or sets the error code.
        /// int16
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the consumer generation id.
        /// int32
        /// </summary>
        public int GenerationId { get; set; }

        /// <summary>
        /// Gets or sets the group protocol.
        /// </summary>
        public string GroupProtocol { get; set; }

        /// <summary>
        /// Gets or sets the leader id.
        /// </summary>
        public string LeaderId { get; set; }

        /// <summary>
        /// Gets or sets the member id.
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// Gets or sets the consumer members.
        /// </summary>
        public KafkaProtocolConsumerMember[] Members { get; set; }

        /// <summary>
        /// Parse the response bytes.
        /// </summary>
        /// <param name="response">The bytes of the response.</param>
        public override void Parse(byte[] response)
        {
            // TODO : deserialize the bytes into response
        }

    }
}