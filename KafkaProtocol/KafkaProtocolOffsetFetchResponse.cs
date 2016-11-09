// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetFetchResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The kafka response of fetching offset request.
    /// v0 and v1 supported in kafka 0.8.2 or later.
    /// </summary>
    public class KafkaProtocolOffsetFetchResponse : KafkaProtocolResponse
    {
        /// <summary>
        /// Gets or sets the topic info in the response.
        /// </summary>
        public KafkaProtocolOffsetFetchResponseTopicInfo[] TopicInfos { get; set; }

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