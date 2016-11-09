// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The base model of the response.
    /// </summary>
    public abstract class KafkaProtocolResponse
    {
        /// <summary>
        /// Parse the response bytes.
        /// </summary>
        /// <param name="response">The bytes of the response.</param>
        public abstract void Parse(byte[] response);
    }
}