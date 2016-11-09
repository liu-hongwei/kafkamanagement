// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolProduceRequest.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The kafka request to send message sets to kafka broker.
    /// message sets of v0, v1 supported in kafka 0.9.0 or later.
    /// v2 supported in kafka 0.10.0 or later.
    /// </summary>
    public class KafkaProtocolProduceRequest : KafkaProtocolRequest
    {
        /// <summary>
        /// Gets or sets value for whether broker need acknowledgement from other brokers.
        /// int16
        /// This field indicates how many acknowledgements the servers should receive before responding to the request. 
        /// If it is 0 the server will not send any response (this is the only case where the server will not reply to a request). 
        /// If it is 1, the server will wait the data is written to the local log before sending a response. 
        /// If it is -1 the server will block until the message is committed by all in sync replicas before sending a response.
        /// </summary>
        public int RequiredAcks { get; set; }

        /// <summary>
        /// Gets or sets the maximum milliseconds the broker wait for ACKs.
        /// int32
        /// This provides a maximum time in milliseconds the server can await the receipt of the number of acknowledgements in RequiredAcks. 
        /// The timeout is not an exact limit on the request time for a few reasons: 
        /// (1) it does not include network latency, 
        /// (2) the timer begins at the beginning of the processing of this request so if many requests are queued due to server 
        /// overload that wait time will not be included, (3) we will not terminate a local write so if the local write time exceeds this timeout 
        /// it will not be respected. To get a hard timeout of this type the client should use the socket timeout.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the topic infos which data will write to.
        /// </summary>
        public KafkaProtocolProduceRequestTopicInfo[] TopicInfos { get; set; }

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
                return KafkaProtocolApiKey.ProduceRequest;
            }
        }

    }
}