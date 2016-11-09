// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaTopicConfiguration.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    using System.Collections.Generic;

    /// <summary>
    /// The kafka topic operation(list,creation,update,deletion) settings.
    /// </summary>
    public class KafkaTopicConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the topic for operation.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Gets or sets the number of replications for topic.
        /// </summary>
        public int NumOfReplicas { get; set; }

        /// <summary>
        /// Gets or sets the number of partitions for topic.
        /// </summary>
        public int NumOfPartitions { get; set; }

        /// <summary>
        /// Gets or sets the customized settings for topic at topic level.
        /// </summary>
        public Dictionary<string, string> TopicLevelConfigurations { get; set; }
    }
}