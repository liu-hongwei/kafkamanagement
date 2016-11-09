// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetFetchRequest.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The kafka request to fetch offset.
    /// v0 and v1 supported in kafka 0.8.2 or later. v0 read offsets from zookeeper, v1 read offsets from kafka.
    /// </summary>
    public class KafkaProtocolOffsetFetchRequest : KafkaProtocolRequest
    {
        /// <summary>
        /// Gets or sets the consumer group.
        /// </summary>
        public string ConsumerGroup { get; set; }

        /// <summary>
        /// Gets or sets the topic info for fetching.
        /// </summary>
        public KafkaProtocolOffsetFetchRequestTopicInfo[] TopicInfos { get; set; }

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
                return KafkaProtocolApiKey.OffsetFetchRequest;
            }
        }
    }
}