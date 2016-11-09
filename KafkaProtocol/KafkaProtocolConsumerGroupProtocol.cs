// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerGroupProtocol.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The consumer group protocol info.
    /// </summary>
    public class KafkaProtocolConsumerGroupProtocol
    {
        /// <summary>
        /// Gets or sets the protocol name.
        /// </summary>
        public string ProtocolName { get; set; }

        /// <summary>
        /// Gets or sets the protocol metadata.
        /// </summary>
        public byte[] ProtocolMetadata { get; set; }
    }
}