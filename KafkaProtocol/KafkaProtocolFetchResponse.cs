// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolFetchResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The response to the fetch request.
    /// </summary>
    public class KafkaProtocolFetchResponse : KafkaProtocolResponse
    {
        /// <summary>
        /// Gets or sets the topic info in response.
        /// </summary>
        public KafkaProtocolFetchResponseTopicInfo[] TopicInfos { get; set; }

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