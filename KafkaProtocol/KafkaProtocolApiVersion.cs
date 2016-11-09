// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolApiVersion.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The api versions supported by kafka brokers.
    /// </summary>
    public enum KafkaProtocolApiVersion
    {
        /// <summary>
        /// API is supported in kafka 0.8.1 or later.
        /// </summary>
        V0 = 0,

        /// <summary>
        /// API is supported in kafka 0.8.2 or later.
        /// </summary>
        V1 = 1,

        /// <summary>
        /// API is supported in kafka 0.9.0 or later.
        /// </summary>
        V2 = 2
    }
}