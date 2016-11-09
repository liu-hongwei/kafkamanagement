// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerSyncGroupRequest.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The request to assign partition assignments to all members by group leader.
    /// </summary>
    public class KafkaProtocolConsumerSyncGroupRequest : KafkaProtocolRequest
    {
        /// <summary>
        /// Gets or sets the gorup id.
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the group generation id.
        /// int32
        /// </summary>
        public int GenerationId { get; set; }

        /// <summary>
        /// Gets or sets the member id.
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// Gets or sets the group assignments.
        /// </summary>
        public KafkaProtocolConsumerSyncGroupAssignment[] GroupAssignments { get; set; }

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
                return KafkaProtocolApiKey.SyncGroupRequest;
            }
        }
    }
}