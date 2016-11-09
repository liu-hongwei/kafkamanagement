// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetCommitRequest.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The request to commit offset to kafka.
    /// </summary>
    public abstract class KafkaProtocolOffsetCommitRequest : KafkaProtocolRequest
    {
        /// <summary>
        /// Gets or sets the consumer group id.
        /// </summary>
        public string ConsumerGroupId { get; set; }

        /// <summary>
        /// Gets or sets the topic info.
        /// </summary>
        public KafkaProtocolOffsetCommitRequestTopicInfo[] TopicInfos { get; set; }

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
        /// Gets the api version.
        /// </summary>
        public override KafkaProtocolApiVersion ApiVersion
        {
            get
            {
                return this.GetRequestVersion();
            }
        }
        
        /// <summary>
        /// Gets the api id of current request.
        /// </summary>
        public override KafkaProtocolApiKey ApiKey
        {
            get
            {
                return KafkaProtocolApiKey.OffsetCommitRequest;
            }
        }

        /// <summary>
        /// Return the version of the request.
        /// </summary>
        /// <returns>The version of the request.</returns>
        public abstract KafkaProtocolApiVersion GetRequestVersion();
    }
}