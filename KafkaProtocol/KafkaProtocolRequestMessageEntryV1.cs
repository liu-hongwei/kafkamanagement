// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolRequestMessageEntryV1.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The version 1 of the message entry in the request.
    /// supported sine kafka 0.10.0
    /// </summary>
    public class KafkaProtocolRequestMessageEntryV1 : KafkaProtocolRequestMessageEntry
    {
        /// <summary>
        /// Gets or sets the timestamp of the message in milliseconds.
        /// int64
        /// This is the timestamp of the message. The timestamp type is indicated in the attributes. Unit is milliseconds since beginning of the epoch (midnight Jan 1, 1970 (UTC)).
        /// </summary>
        public int Timestamp { get; set; }
    }
}