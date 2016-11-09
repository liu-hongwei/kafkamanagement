// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IZookeeperClient.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The zookeeper access interface.
    /// </summary>
    /// <remarks>
    /// the purpose of this interface is to abstract the methods used in <see cref="IKafkaClusterManager"/> 
    /// out so the <see cref="IKafkaClusterManager"/> can be tested without real zookeeper cluster.
    /// </remarks>
    public interface IZookeeperClient : IDisposable
    {
        /// <summary>
        /// Gets or sets the zookeeper connection settings.
        /// </summary>
        ZookeeperConnectionSettings ConnectionSettings { get; set; }

        /// <summary>
        /// Connect to zookeeper cluster.
        /// </summary>
        /// <returns>A boolean value indicate whether connected to zookeeper.</returns>
        Task<bool> ConnectAsync();

        /// <summary>
        /// DisconnectAsync from zookeeper cluster.
        /// </summary>
        /// <returns>A Task holding operation.</returns>
        Task DisconnectAsync();

        /// <summary>
        /// Get the status of connection between client and zookeeper cluster.
        /// </summary>
        /// <returns>The connection status.</returns>
        Task<ZookeeperConnectionStatus> GetStatusAsync();

        /// <summary>
        /// The the children nodes under given path in zookeeper.
        /// </summary>
        /// <param name="path">The path in zookeeper.</param>
        /// <param name="watch">whether watch the changes.</param>
        /// <returns>The children nodes.</returns>
        Task<IEnumerable<string>> GetChildrenAsync(string path, bool watch = false);

        /// <summary>
        /// Get the content of zookeeper node under given path.
        /// </summary>
        /// <param name="path">The path to get content.</param>
        /// <param name="watch">whether watch the changes.</param>
        /// <returns>The content of node.</returns>
        Task<byte[]> GetDataAsync(string path, bool watch = false);

        /// <summary>
        /// Set the content of zookeeper node under givne path.
        /// </summary>
        /// <param name="path">The path to the node.</param>
        /// <param name="data">The content to set.</param>
        /// <param name="version">The version of content.</param>
        /// <returns>A task holding the operation.</returns>
        Task<ZookeeperZnodeStat> SetDataAsync(string path, byte[] data, int version);

        /// <summary>
        /// Create node under given path with given data and settings.
        /// </summary>
        /// <param name="path">The path to create node.</param>
        /// <param name="data">the content of the node.</param>
        /// <param name="acl">The access control list.</param>
        /// <param name="type">the znode type.</param>
        /// <returns>The node path if created.</returns>
        Task<string> CreateAsync(string path, byte[] data, IEnumerable<ZookeeperZnodeAcl> acl, ZookeeperZnodeType type);

        /// <summary>
        /// Delete node under given path.
        /// </summary>
        /// <param name="path">The path to the node.</param>
        /// <param name="version">The verion of node.</param>
        /// <returns>A task holding deletion operation.</returns>
        Task<bool> DeleteAsync(string path, int version);

        /// <summary>
        /// Check whether path exist in zookeeper.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <param name="watch">whether get notification.</param>
        /// <returns>The information of the path.</returns>
        Task<ZookeeperZnodeStat> ExistsAsync(string path, bool watch = false);
    }
}
