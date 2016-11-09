// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolRequestMessageSet.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The message set in kafka request.
    /// </summary>
    public class KafkaProtocolRequestMessageSet
    {
        /// <summary>
        /// Gets or sets the message sets.
        /// </summary>
        public KafkaProtocolRequestMessageSetEntry[] MessageSet { get; set; }
    }
}