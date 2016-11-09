// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolTopicMetadataResponsePartitionInfo.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The partition info in the topic metadata response.
    /// </summary>
    public class KafkaProtocolTopicMetadataResponsePartitionInfo
    {
        /// <summary>
        /// Gets or sets the error code.
        /// int16
        /// </summary>
        public int PartitionErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the partition id.
        /// int32
        /// </summary>
        public int PartitionId { get; set; }

        /// <summary>
        /// Gets or sets the id of broker who is leader of current partition.
        /// int32
        /// The node id for the kafka broker currently acting as leader for this partition. If no leader exists because we are in the middle of a leader election this id will be -1
        /// </summary>
        public int Leader { get; set; }

        /// <summary>
        /// Gets or sets the ids of brokers who holds replicas for current partition.
        /// [int32]
        /// The set of alive nodes that currently acts as slaves for the leader for this partition.
        /// </summary>
        public int[] Replicas { get; set; }

        /// <summary>
        /// Gets or sets the ids of replics who catchs up with leader.
        /// [int32]
        /// The set subset of the replicas that are "caught up" to the leader
        /// </summary>
        public int[] Isr { get; set; }
    }
}