// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetCommitResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The kafka response to commit offset request.
    /// suppport request in version 0, 1 , 2.
    /// </summary>
    public class KafkaProtocolOffsetCommitResponse : KafkaProtocolResponse
    {
        /// <summary>
        /// Gets or sets the topic info in the response of offset commit.
        /// </summary>
        public KafkaProtocolOffsetCommitResponseTopicInfo[] TopicInfos { get; set; }

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