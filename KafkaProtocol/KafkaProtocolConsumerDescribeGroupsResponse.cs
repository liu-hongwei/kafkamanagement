// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerDescribeGroupsResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The response of consumer groups describe request.
    /// </summary>
    public class KafkaProtocolConsumerDescribeGroupsResponse : KafkaProtocolResponse
    {
        /// <summary>
        /// Gets or sets the responsed consumer group info.
        /// </summary>
        public KafkaProtocolConsumerDescribeGroupsResponseGroupInfo[] GroupInfos { get; set; }

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