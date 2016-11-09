// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IZookeeperEventWatcher.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    /// <summary>
    /// The zookeeper event watcher.
    /// </summary>
    public interface IZookeeperEventWatcher
    {
        /// <summary>
        /// The zookeeper notification handler.
        /// </summary>
        /// <typeparam name="T">The type of notification event.</typeparam>
        /// <param name="notification">The notification event.</param>
        void HandleNotification<T>(T notification);
    }
}