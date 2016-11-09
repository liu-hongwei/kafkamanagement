// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerJoinGroupRequest.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The request of join into consumer group.
    /// </summary>
    public class KafkaProtocolConsumerJoinGroupRequest : KafkaProtocolRequest
    {
        /// <summary>
        /// Gets or sets the group id.
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the session timeout milliseconds.
        /// int32
        /// </summary>
        public int SessionTimeout { get; set; }

        /// <summary>
        /// Gets or sets the member id.
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// Gets or sets the protocol type.
        /// </summary>
        public string ProtocolType { get; set; }

        /// <summary>
        /// Gets or sets the group protocols.
        /// </summary>
        public KafkaProtocolConsumerGroupProtocol[] GroupProtocols { get; set; }

        /// <summary>
        /// Gets the size of the request.
        /// </summary>
        protected override int BodySize
        {
            get
            {
                // TODO : to calculate the packet size in bytes
                return 0;
            }
        }

        /// <summary>
        /// Gets the bytes of the request packet.
        /// </summary>
        protected override byte[] BodyBytes
        {
            get
            {
                // TODO : to serialize the request to bytes
                return null;
            }
        }

        /// <summary>
        /// Gets the api id of current request.
        /// </summary>
        public override KafkaProtocolApiKey ApiKey
        {
            get
            {
                return KafkaProtocolApiKey.JoinGroupRequest;
            }
        }
    }
}