// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaPartitionInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    /// <summary>
    /// The kafka partition infomation.
    /// </summary>
    public class KafkaPartitionInfo
    {
        /// <summary>
        /// Gets or sets the id of the partition.
        /// </summary>
        public string PartitionId { get; set; }

        /// <summary>
        /// Gets or sets the ids of brokers who owns replication for current partition.
        /// </summary>
        public int[] ReplicaOwners { get; set; } 
    }
}