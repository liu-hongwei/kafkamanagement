// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetListRequest.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The kafka request to list offset.
    /// This API describes the valid offset range available for a set of topic-partitions. 
    /// As with the produce and fetch APIs requests must be directed to the broker that is currently the leader for the partitions in question. 
    /// This can be determined using the metadata API.
    /// </summary>
    public class KafkaProtocolOffsetListRequest : KafkaProtocolRequest
    {
        /// <summary>
        /// Gets or sets the id of broker.
        /// int32
        /// Broker id of the follower. For normal consumers, use -1.
        /// </summary>
        public int ReplicaId { get; set; }

        /// <summary>
        /// Gets or sets the topic info in the request.
        /// </summary>
        public KafkaProtocolOffsetListRequestTopicInfo[] TopicInfos { get; set; }

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
                // This API describes the valid offset range available for a set of topic-partitions. 
                // As with the produce and fetch APIs requests must be directed to the broker that is currently the leader for 
                // the partitions in question. This can be determined using the metadata API.
                //------------------------------------------------------\\
                // OffsetRequest => ReplicaId[TopicName[Partition Time MaxNumberOfOffsets]]
                //  ReplicaId => int32
                //  TopicName => string
                //  Partition => int32
                //  Time => int64
                //  MaxNumberOfOffsets => int32
                //------------------------------------------------------\\

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
                return KafkaProtocolApiKey.OffsetRequest;
            }
        }

    }
}
