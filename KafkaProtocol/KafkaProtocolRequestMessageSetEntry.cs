// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolRequestMessageSetEntry.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The common fields in a message set entry.
    /// </summary>
    public class KafkaProtocolRequestMessageSetEntry
    {
        /// <summary>
        /// Gets or sets the offset of a message.
        /// int64
        /// This is the offset used in kafka as the log sequence number. When the producer is sending non compressed messages, 
        /// it can set the offsets to anything. When the producer is sending compressed messages, to avoid server side recompression, 
        /// each compressed message should have offset starting from 0 and increasing by one for each inner message in the compressed message. 
        /// (see more details about compressed messages in Kafka below)
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the size of message.
        /// int32
        /// </summary>
        public int MessageSize { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public KafkaProtocolRequestMessageEntry Message { get; set; }
    }
}