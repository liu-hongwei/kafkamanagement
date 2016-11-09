// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolRequestMessageEntry.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// the base model of the message in request. 
    /// </summary>
    public abstract class KafkaProtocolRequestMessageEntry
    {
        /// <summary>
        /// Gets or sets the CRC for the message.
        /// int32
        /// The CRC is the CRC32 of the remainder of the message bytes. This is used to check the integrity of the message on the broker and consumer.
        /// </summary>
        public int Crc { get; set; }

        /// <summary>
        /// Gets or sets the version id.
        /// int8
        /// This is a version id used to allow backwards compatible evolution of the message binary format. The current value is 1.
        /// </summary>
        public int MagicByte { get; set; }

        /// <summary>
        /// Gets or sets the metadata attributes.
        /// int8
        /// This byte holds metadata attributes about the message.
        /// The lowest 3 bits contain the compression codec used for the message.
        /// The fourth lowest bit represents the timestamp type. 0 stands for CreateTime and 1 stands for 
        /// LogAppendTime.The producer should always set this bit to 0. (since 0.10.0)
        /// All other bits should be set to 0.
        /// </summary>
        public int Attributes { get; set; }

        /// <summary>
        /// Gets or sets key for this message .
        /// The key is an optional message key that was used for partition assignment. The key can be null.
        /// </summary>
        public byte[] Key { get; set; }

        /// <summary>
        /// Gets or sets the actual content of message.
        /// The value is the actual message contents as an opaque byte array. Kafka supports recursive messages 
        /// in which case this may itself contain a message set. The message can be null.
        /// </summary>
        public byte[] Value { get; set; }
    }
}