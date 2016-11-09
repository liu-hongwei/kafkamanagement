// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZookeeperConnectionStatus.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    /// <summary>
    /// The zookeeper connection status.
    /// </summary>
    public enum ZookeeperConnectionStatus
    {
        /// <summary>
        /// authentication failed.
        /// </summary>
        AuthFailed,

        /// <summary>
        /// connection colosed.
        /// </summary>
        Closed,

        /// <summary>
        /// connected to cluster.
        /// </summary>
        Connected,

        /// <summary>
        /// connecting to cluster.
        /// </summary>
        Connecting,

        /// <summary>
        /// connection disconnected.
        /// </summary>
        NotConnected
    }
}
