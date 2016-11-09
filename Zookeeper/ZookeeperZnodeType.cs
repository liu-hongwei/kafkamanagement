// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZookeeperZnodeType.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    /// <summary>
    /// The zookeeper znode flags.
    /// see below
    /// https://zookeeper.apache.org/doc/r3.1.2/zookeeperProgrammers.html
    /// https://zookeeper.apache.org/doc/r3.1.2/api/index.html
    /// </summary>
    public enum ZookeeperZnodeType
    {
        /// <summary>
        /// znode is ephemeral node.
        /// znodes exists as long as the session that created the znode is active. When the session ends the znode is deleted. Because of this behavior ephemeral znodes are not allowed to have children.
        /// </summary>
        Ephemeral,

        /// <summary>
        /// znode is ephemral and has unique name (sequence node).
        /// append a monotonicly increasing counter to the end of path.
        /// </summary>
        EphemeralSequential,

        /// <summary>
        /// znode is not ephemeral node (will not get deleted).
        /// </summary>
        Persistent,

        /// <summary>
        /// znode is not ephemeral node (will not get deleted) and with unqiue name.
        /// append a monotonicly increasing counter to the end of path.
        /// </summary>
        PersistentSequential
    }
}
