// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolProduceResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The kafka response to the produce request.
    /// </summary>
    public abstract class KafkaProtocolProduceResponse : KafkaProtocolResponse
    {
        /// <summary>
        /// Gets or sets the topic infos in the response.
        /// </summary>
        public KafkaProtocolProduceResponseTopicInfo[] TopicInfos { get; set; }
    }
}