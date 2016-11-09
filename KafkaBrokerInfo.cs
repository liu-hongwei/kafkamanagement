// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaBrokerInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    using Newtonsoft.Json;

    /// <summary>
    /// The kafka broker information.
    /// </summary>
    public class KafkaBrokerInfo
    {
        /// <summary>
        /// Gets or sets the id of the broker.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the JMX port opened for current broker.
        /// </summary>
        [JsonProperty("jmx_port")]
        public int JmxPort { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of broker info in zookeeper.
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the broker's data endpoint info.
        /// </summary>
        [JsonProperty("endpoints")]
        public string[] Endpoints { get; set; }

        /// <summary>
        /// Gets or sets the broker ip or host name.
        /// </summary>
        [JsonProperty("host")]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the version of zookeeper nodes for broker.
        /// </summary>
        [JsonProperty("version")]
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the broker endpoint port.
        /// </summary>
        [JsonProperty("port")]
        public int Port { get; set; }
    }
}