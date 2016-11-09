// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolFetchResponseV1.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The version 1 of the fetch response to fetch request.
    /// </summary>
    public class KafkaProtocolFetchResponseV1 : KafkaProtocolFetchResponse
    {
        /// <summary>
        /// Gets or sets the duration in milliseconds.
        /// Duration in milliseconds for which the request was throttled due to quota violation. (Zero if the request did not violate any quota.)
        /// </summary>
        public int ThrottleTime { get; set; }
    }
}