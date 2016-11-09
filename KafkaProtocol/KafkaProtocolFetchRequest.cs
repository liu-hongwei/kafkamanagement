// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolFetchRequest.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The request to get one or more data from topic , partitions.
    /// </summary>
    public class KafkaProtocolFetchRequest : KafkaProtocolRequest
    {
        /// <summary>
        /// Gets or sets the replica id.
        /// int32
        /// The replica id indicates the node id of the replica initiating this request. 
        /// Normal client consumers should always specify this as -1 as they have no node id. Other brokers set this to be their own node id. 
        /// The value -2 is accepted to allow a non-broker to issue fetch requests as if it were a replica broker for debugging purposes.
        /// </summary>
        public int ReplicaId { get; set; }

        /// <summary>
        /// Gets or sets the maximum waiting time in milliseconds.
        /// int32
        /// The max wait time is the maximum amount of time in milliseconds to block waiting if insufficient data is 
        /// available at the time the request is issued.
        /// </summary>
        public int MaxWaitTime { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of bytes must available in response.
        /// This is the minimum number of bytes of messages that must be available to give a response. 
        /// If the client sets this to 0 the server will always respond immediately, however if there is no new data 
        /// since their last request they will just get back empty message sets. If this is set to 1, the server will 
        /// respond as soon as at least one partition has at least 1 byte of data or the specified timeout occurs. 
        /// By setting higher values in combination with the timeout the consumer can tune for throughput and 
        /// trade a little additional latency for reading only large chunks of data (e.g. setting MaxWaitTime to 
        /// 100 ms and setting MinBytes to 64k would allow the server to wait up to 100ms to try to accumulate 64k 
        /// of data before responding).
        /// </summary>
        public int MinBytes { get; set; }

        /// <summary>
        /// Gets or sets the topic info for reading data.
        /// </summary>
        public KafkaProtocolFetchRequestTopicInfo[] TopicInfos { get; set; }

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
                return KafkaProtocolApiKey.FetchRequest;
            }
        }

    }
}
