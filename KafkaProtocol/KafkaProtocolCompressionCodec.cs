// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolCompressionCodec.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The kafka message compression codecs.
    /// </summary>
    public enum KafkaProtocolCompressionCodec
    {
        /// <summary>
        /// message is not compressed.
        /// </summary>
        None = 0,

        /// <summary>
        /// message is compressed with gzip standard.
        /// </summary>
        Gzip = 1,

        /// <summary>
        /// message is compressed with snappy standard.
        /// </summary>
        Snappy = 2
    }
}