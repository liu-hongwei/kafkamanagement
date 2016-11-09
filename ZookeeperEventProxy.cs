// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZookeeperEventProxy.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    using ZooKeeperNet;

    /// <summary>
    /// The bridge class between <see cref="IZookeeperEventWatcher"/> and zookeeper driver type <see cref="IWatcher"/>, for 
    /// proxy the event from <see cref="IWatcher"/> to <see cref="IZookeeperEventWatcher"/>.
    /// </summary>
    internal class ZookeeperEventProxy : IWatcher
    {
        /// <summary>
        /// The zookeeper event watcher.
        /// </summary>
        private IZookeeperEventWatcher watcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZookeeperEventProxy"/> class.
        /// </summary>
        public ZookeeperEventProxy(IZookeeperEventWatcher eventWatcher)
        {
            this.watcher = eventWatcher;
        }

        /// <summary>
        /// The zookeeper notification handler.
        /// </summary>
        /// <param name="event">The notification event.</param>
        public void Process(WatchedEvent @event)
        {
            if (this.watcher != null)
            {
                this.watcher.HandleNotification(@event);
            }
        }
    }
}