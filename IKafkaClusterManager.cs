// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKafkaClusterManager.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface for managing kafka cluster.
    /// </summary>
    public interface IKafkaClusterManager : IDisposable
    {
        /// <summary>
        /// Gets or sets the zookeeper client for cluster management.
        /// </summary>
        IZookeeperClient ZookeeperClient { get; set; }

        /// <summary>
        /// Fetch the topic information from zookeeper.
        /// </summary>
        /// <param name="topic">the name of the topic.</param>
        /// <returns>The whole topic infomration.</returns>
        Task<KafkaTopicInfo> GetKafkaTopicAsync(string topic);

        /// <summary>
        /// Create a topic with creation settings.
        /// </summary>
        /// <param name="topicConfiguration">The topic creation configuration.</param>
        /// <returns>the path to the topic. null is return when unable to create it.</returns>
        Task<string> CreateKafkaTopicAsync(KafkaTopicConfiguration topicConfiguration);

        /// <summary>
        /// List all topics in the cluster.
        /// </summary>
        /// <returns>all topic information in cluster.</returns>
        Task<IEnumerable<KafkaTopicInfo>> ListKafkaTopicsAsync();

        /// <summary>
        /// Delete a topic by topic name.
        /// </summary>
        /// <param name="topic">the name of the topic.</param>
        /// <returns>Task holds status of deletion. True indicates succeeded. False indicates failed.</returns>
        Task<bool> DeleteKafkaTopicAsync(string topic);

        /// <summary>
        /// Update partitions and others settings for a topic.
        /// </summary>
        /// <param name="topicConfiguration">The topic update information.</param>
        /// <returns>Task holds the topic info if update complete. null is return when topic does not exist or unable to update it.</returns>
        Task<KafkaTopicInfo> UpdateKafkaTopicAsync(KafkaTopicConfiguration topicConfiguration);
        
        /// <summary>
        /// List all brokers in cluster.
        /// </summary>
        /// <returns>Task holds the broker info. null is returned if no brokers.</returns>
        Task<IEnumerable<KafkaBrokerInfo>> ListKakfaBrokersAsync();

        /// <summary>
        /// Get the state of a kafka partition.
        /// </summary>
        /// <param name="topic">The name of topic.</param>
        /// <param name="partitionId">The id of partition.</param>
        /// <returns>Task holds the partition state.</returns>
        Task<KafkaPartitionState> GetKafkaPartitionStateAsync(string topic, string partitionId);

        /// <summary>
        /// Get the kafka consumer group ids.
        /// </summary>
        /// <returns>Task holds the consumer group ids.</returns>
        Task<IEnumerable<string>> ListKafkaConsumerGroupsAsync();

        /// <summary>
        /// Get the kafka consumer ids in given group.
        /// </summary>
        /// <param name="groupId">The id of the consumer group.</param>
        /// <returns>Task holds the consumer ids in the given group.</returns>
        Task<IEnumerable<string>> ListKafkaConsumerIdsAsync(string groupId);

        /// <summary>
        /// Get the kafka consumer group owned topics.
        /// </summary>
        /// <returns>Task holds the consumer group onwed topics.</returns>
        Task<IEnumerable<string>> ListKafkaConsumerOwnedTopicsAsync(string groupId);

        /// <summary>
        /// Get the kafka consumer owned partitions.
        /// </summary>
        /// <returns>Task holds the consumer owned partitions.</returns>
        Task<IEnumerable<string>> ListKafkaConsumerOwnedPartitionsAsync(string groupId, string topic);

        /// <summary>
        /// Get the kafka consumer owner for specific partition.
        /// </summary>
        /// <returns>Task holds the consumer owner for specific partition.</returns>
        Task<string> ListKafkaConsumerOwnerAsync(string groupId, string topic, string partition);

        /// <summary>
        /// Get the kafka consumer offset.
        /// </summary>
        /// <returns>Task holds the consumer offset.</returns>
        Task<long> ListKafkaConsumerOffsetAsync(string groupId, string topic, string partition);
    }
}
