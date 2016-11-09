// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZookeeperEventWatcher.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    /// <summary>
    /// The zookeeper event watcher.
    /// </summary>
    public class ZookeeperEventWatcher : IZookeeperEventWatcher
    {
        /// <summary>
        /// handle the notification from zookeeper.
        /// </summary>
        /// <typeparam name="T">type of the notification.</typeparam>
        /// <param name="notification">the notified data.</param>
        public void HandleNotification<T>(T notification)
        {
            // TODO : if need to handle events from zookeeper then process it here, or create your own implementation of IZookeeperEventWatcher
        }
    }
}