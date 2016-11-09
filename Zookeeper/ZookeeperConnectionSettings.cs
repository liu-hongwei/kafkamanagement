// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZookeeperConnectionSettings.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    /// <summary>
    /// The access settings for zookeeper.
    /// </summary>
    public class ZookeeperConnectionSettings
    {
        /// <summary>
        /// Gets or sets the zookeeper connection strings.
        /// </summary>
        public string ConnectionStrings { get; set; }

        /// <summary>
        /// Gets or sets the zookeeper connection session timeout milliseconds.
        /// </summary>
        public int SessionTimeoutMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets the maximum timeout in minutes for retrying connection.
        /// </summary>
        public int MaxRetryConnectMinutes { get; set; }

        /// <summary>
        /// Gets or sets the zookeeper event watcher.
        /// </summary>
        public IZookeeperEventWatcher EventWatcher { get; set; }
    }
}