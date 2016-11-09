// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaPartitionState.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    using Newtonsoft.Json;

    /// <summary>
    /// The kafka partition state.
    /// </summary>
    public class KafkaPartitionState
    {
        /// <summary>
        /// Gets or sets the epoch of controller.
        /// </summary>
        [JsonProperty("controller_epoch")]
        public int ControllerEpoch { get; set; }


        /// <summary>
        /// Gets or sets leader broker id.
        /// </summary>
        [JsonProperty("leader")]
        public int LeaderId { get; set; }

        /// <summary>
        /// Gets or sets version of state.
        /// </summary>
        [JsonProperty("version")]
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets epoch of leader.
        /// </summary>
        [JsonProperty("leader_epoch")]
        public int LeaderEpoch { get; set; }

        /// <summary>
        /// Gets or sets the isr list;
        /// </summary>
        [JsonProperty("isr")]
        public int[] Isr { get; set; }
    }
}