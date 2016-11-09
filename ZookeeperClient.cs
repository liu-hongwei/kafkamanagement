// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZookeeperClient.cs" company="">
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
    using Org.Apache.Zookeeper.Data;
    using ZooKeeperNet;

    /// <summary>
    /// The zookeeper access client.
    /// </summary>
    /// <remarks>
    /// the purpose of this interface is to abstract the methods used in <see cref="KafkaClusterManager"/> 
    /// out so the <see cref="KafkaClusterManager"/> can be tested without real zookeeper cluster.
    /// </remarks>
    public class ZookeeperClient : IZookeeperClient
    {
        /// <summary>
        /// The default connection retry timeout is 5 minutes.
        /// </summary>
        private const int DefaultZookeeperRetryConnectionMinutes = 5;

        /// <summary>
        /// The default connection session timeout is 10 seconds.
        /// </summary>
        private const int DefaultZookeeperSessionTimeoutSeconds = 10;

        /// <summary>
        /// The lock object to prevent create multi connections to zookeeper.
        /// </summary>
        private object zookeeperLock = new object();

        /// <summary>
        /// The zookeeper client to access zookeeper.
        /// </summary>
        private ZooKeeper zookeeper;

        /// <summary>
        /// Requestor for getting node children.
        /// </summary>
        private Requestor<IEnumerable<string>> getChildrenRequestor;

        /// <summary>
        /// Requestor for creating or deleting node.
        /// </summary>
        private Requestor<string> createOrDeleteRequestor;

        /// <summary>
        /// Requestor for getting node data.
        /// </summary>
        private Requestor<byte[]> getDataRequestor;

        /// <summary>
        /// Requestor for setting or detecting node.
        /// </summary>
        private Requestor<Stat> setOrExistsRequestor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZookeeperClient"/> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "It is disposed in the Dispose method.")]
        public ZookeeperClient()
        {
            this.getChildrenRequestor = new Requestor<IEnumerable<string>>(new ExceptionOnlyRetryPolicy<IEnumerable<string>>(this.ShouldRetry), new UnbreakableCircuitBreaker<IEnumerable<string>>("zookeeper.GetChildren"), true);
            this.createOrDeleteRequestor = new Requestor<string>(new ExceptionOnlyRetryPolicy<string>(this.ShouldRetry), new UnbreakableCircuitBreaker<string>("zookeeper.CreateOrDelete"), true);
            this.getDataRequestor = new Requestor<byte[]>(new ExceptionOnlyRetryPolicy<byte[]>(this.ShouldRetry), new UnbreakableCircuitBreaker<byte[]>("zookeeper.GetData"), true);
            this.setOrExistsRequestor = new Requestor<Stat>(new ExceptionOnlyRetryPolicy<Stat>(this.ShouldRetry), new UnbreakableCircuitBreaker<Stat>("zookeeper.SetOrExists"), true);
        }

        /// <summary>
        /// Gets or sets the zookeeper connection settings.
        /// </summary>
        public ZookeeperConnectionSettings ConnectionSettings { get; set; }

        /// <summary>
        /// Connect to zookeeper cluster.
        /// </summary>
        /// <returns>A boolean value indicate whether connected to zookeeper.</returns>
        public Task<bool> ConnectAsync()
        {
            if (this.ConnectionSettings == null)
            {
                throw new ZookeeperOperationException("ConnectionSettings property is null.");
            }

            if (string.IsNullOrWhiteSpace(this.ConnectionSettings.ConnectionStrings))
            {
                throw new ZookeeperOperationException("ConnectionSettings.ConnectionStrings is null or empty.");
            }

            if (this.ConnectionSettings.SessionTimeoutMilliseconds <= 0)
            {
                this.ConnectionSettings.SessionTimeoutMilliseconds = DefaultZookeeperSessionTimeoutSeconds * 1000;
            }
            
            if (this.ConnectionSettings.MaxRetryConnectMinutes <= 0)
            {
                this.ConnectionSettings.MaxRetryConnectMinutes = DefaultZookeeperRetryConnectionMinutes;
            }

            this.CreateClient(this.zookeeper == null);

            return Task.Run(() =>
            {
                if (this.zookeeper.State == ZooKeeper.States.CONNECTED)
                {
                    return true;
                }

                DateTime time = DateTime.UtcNow;
                DateTime timeout = time.AddMinutes(this.ConnectionSettings.MaxRetryConnectMinutes);

                while (this.zookeeper.State != ZooKeeper.States.CONNECTED && timeout > time)
                {
                    // for each session, if state is closed , try to create again
                    this.CreateClient(this.zookeeper.State == ZooKeeper.States.CLOSED);

                    System.Threading.Thread.Sleep(500);
                    time = DateTime.UtcNow;
                }

                if (this.zookeeper.State != ZooKeeper.States.CONNECTED)
                {
                    return false;
                }

                return true;
            });
        }

        /// <summary>
        /// DisconnectAsync from zookeeper cluster.
        /// </summary>
        /// <returns>A Task holding operation.</returns>
        public Task DisconnectAsync()
        {
            return Task.Run(() =>
            {
                if (this.zookeeper == null)
                {
                    return;
                }

                this.zookeeper.Dispose();
                this.zookeeper = null;
            });
        }

        /// <summary>
        /// Get the status of connection between client and zookeeper cluster.
        /// </summary>
        /// <returns>The connection status.</returns>
        public Task<ZookeeperConnectionStatus> GetStatusAsync()
        {
            ZookeeperConnectionStatus status = ZookeeperConnectionStatus.NotConnected;

            if (this.zookeeper != null)
            {
                if (this.zookeeper.State == ZooKeeper.States.CONNECTED)
                {
                    status = ZookeeperConnectionStatus.Connected;
                }

                if (this.zookeeper.State == ZooKeeper.States.ASSOCIATING ||
                    this.zookeeper.State == ZooKeeper.States.CONNECTING)
                {
                    status = ZookeeperConnectionStatus.Connecting;
                }

                if (this.zookeeper.State == ZooKeeper.States.CLOSED)
                {
                    status = ZookeeperConnectionStatus.Closed;
                }
            }

            return Task.FromResult(status);
        }

        /// <summary>
        /// The the children nodes under given path in zookeeper.
        /// </summary>
        /// <param name="path">The path in zookeeper.</param>
        /// <param name="watch">whether watch the changes.</param>
        /// <returns>The children nodes.</returns>
        public async Task<IEnumerable<string>> GetChildrenAsync(string path, bool watch = false)
        {
            await this.EnsureConnectedToZookeeperClusterAsync().ConfigureAwait(false);
            IEnumerable<string> result = null;
            try
            {
                result = await this.getChildrenRequestor.ExecuteAsync(() => Task.Run(() => this.zookeeper.GetChildren(path, watch))).ConfigureAwait(false);
            }
            catch (KeeperException)
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Get the content of zookeeper node under given path.
        /// </summary>
        /// <param name="path">The path to get content.</param>
        /// <param name="watch">whether watch the changes.</param>
        /// <returns>The content of node.</returns>
        public async Task<byte[]> GetDataAsync(string path, bool watch = false)
        {
            await this.EnsureConnectedToZookeeperClusterAsync().ConfigureAwait(false);
            byte[] result;
            try
            {
                result = await this.getDataRequestor.ExecuteAsync(() => Task.Run(() => this.zookeeper.GetData(path, watch, null))).ConfigureAwait(false);
            }
            catch (KeeperException.NoNodeException)
            {
                result = null;
            }
            catch (KeeperException.SessionExpiredException)
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Set the content of zookeeper node under givne path.
        /// </summary>
        /// <param name="path">The path to the node.</param>
        /// <param name="data">The content to set.</param>
        /// <param name="version">The version of content.</param>
        /// <returns>A task holding the operation.</returns>
        public async Task<ZookeeperZnodeStat> SetDataAsync(string path, byte[] data, int version)
        {
            await this.EnsureConnectedToZookeeperClusterAsync().ConfigureAwait(false);

            ZookeeperZnodeStat result = null;
            var stat = await this.setOrExistsRequestor.ExecuteAsync(() => Task.Run(() => this.zookeeper.SetData(path, data, version))).ConfigureAwait(false);
            if (stat != null)
            {
                result = new ZookeeperZnodeStat()
                {
                    Aversion = stat.Aversion,
                    Ctime = stat.Ctime,
                    Cversion = stat.Cversion,
                    Czxid = stat.Czxid,
                    DataLength = stat.DataLength,
                    EphemeralOwner = stat.EphemeralOwner,
                    Mtime = stat.Mtime,
                    Mzxid = stat.Mzxid,
                    NumChildren = stat.NumChildren,
                    Version = stat.Version
                };
            }

            return result;
        }

        /// <summary>
        /// Create node under given path with given data and settings.
        /// </summary>
        /// <param name="path">The path to create node.</param>
        /// <param name="data">the content of the node.</param>
        /// <param name="acl">The access control list.</param>
        /// <param name="type">the znode type.</param>
        /// <returns>The node path if created.</returns>
        public async Task<string> CreateAsync(string path, byte[] data, IEnumerable<ZookeeperZnodeAcl> acl, ZookeeperZnodeType type)
        {
            await this.EnsureConnectedToZookeeperClusterAsync().ConfigureAwait(false);

            string result;
            try
            {
                var zacls = new List<ACL>();
                if (acl == null || !acl.Any())
                {
                    var ids = ZookeeperZnodeAclIdentity.WorldIdentity();
                    zacls.Add(new ACL((int)ZookeeperZnodeAclRights.All, new ZKId(ids.Schema, ids.Identity)));
                }
                else
                {
                    foreach (var ac in acl)
                    {
                        if (ac.Identity == null)
                        {
                            ac.Identity = ZookeeperZnodeAclIdentity.WorldIdentity();
                        }

                        zacls.Add(new ACL((int)ac.AclRights, new ZKId(ac.Identity.Schema, ac.Identity.Identity)));
                    }
                }

                var zmode = CreateMode.Persistent;
                if (type == ZookeeperZnodeType.Persistent)
                {
                    zmode = CreateMode.Persistent;
                }
                else if (type == ZookeeperZnodeType.PersistentSequential)
                {
                    zmode = CreateMode.PersistentSequential;
                }
                else if (type == ZookeeperZnodeType.Ephemeral)
                {
                    zmode = CreateMode.Ephemeral;
                }
                else if (type == ZookeeperZnodeType.EphemeralSequential)
                {
                    zmode = CreateMode.EphemeralSequential;
                }

                result = await this.createOrDeleteRequestor.ExecuteAsync(() => Task.Run(() => this.zookeeper.Create(path, data, zacls, zmode))).ConfigureAwait(false);
            }
            catch (KeeperException.NodeExistsException)
            {
                result = path;
            }
            catch (KeeperException.SessionExpiredException)
            {
                result = null;
            }

            return result;

        }

        /// <summary>
        /// Delete node under given path.
        /// </summary>
        /// <param name="path">The path to the node.</param>
        /// <param name="version">The verion of node.</param>
        /// <returns>A task holding deletion operation.</returns>
        public async Task<bool> DeleteAsync(string path, int version)
        {
            await this.EnsureConnectedToZookeeperClusterAsync().ConfigureAwait(false);

            try
            {
               await this.createOrDeleteRequestor.ExecuteWithoutResultAsync(() => Task.Run(() => this.zookeeper.Delete(path, version))).ConfigureAwait(false);
            }
            catch (KeeperException.NoNodeException)
            {
                return false;
            }
            catch (KeeperException.BadVersionException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check whether path exists in zookeeper.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <param name="watch">whether get notification.</param>
        /// <returns>The information of the path.</returns>
        public async Task<ZookeeperZnodeStat> ExistsAsync(string path, bool watch = false)
        {
            await this.EnsureConnectedToZookeeperClusterAsync().ConfigureAwait(false);
            ZookeeperZnodeStat result = null;
            try
            {
                var stat = await this.setOrExistsRequestor.ExecuteAsync(() => Task.Run(() => this.zookeeper.Exists(path, watch))).ConfigureAwait(false);
                if (stat != null)
                {
                    result = new ZookeeperZnodeStat()
                    {
                        Aversion = stat.Aversion,
                        Ctime = stat.Ctime,
                        Cversion = stat.Cversion,
                        Czxid = stat.Czxid,
                        DataLength = stat.DataLength,
                        EphemeralOwner = stat.EphemeralOwner,
                        Mtime = stat.Mtime,
                        Mzxid = stat.Mzxid,
                        NumChildren = stat.NumChildren,
                        Version = stat.Version
                    };
                }
            }
            catch (KeeperException)
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Disposing unused resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing unused resources.
        /// </summary>
        /// <param name="disposing">Whether need to dispose the resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.zookeeper != null)
                {
                    this.zookeeper.Dispose();
                    this.zookeeper = null;
                }

                if (this.getChildrenRequestor != null)
                {
                    this.getChildrenRequestor.Dispose();
                    this.getChildrenRequestor = null;
                }

                if (this.createOrDeleteRequestor != null)
                {
                    this.createOrDeleteRequestor.Dispose();
                    this.createOrDeleteRequestor = null;
                }

                if (this.getDataRequestor != null)
                {
                    this.getDataRequestor.Dispose();
                    this.getDataRequestor = null;
                }

                if (this.setOrExistsRequestor != null)
                {
                    this.setOrExistsRequestor.Dispose();
                    this.setOrExistsRequestor = null;
                }
            }
        }

        /// <summary>
        /// Determine whether Zookeeper operation should be retried.
        /// </summary>
        /// <param name="currentAttempt">Attempt number.</param>
        /// <param name="exception">Exception that occurred.</param>
        /// <returns>Result indicating if operation should be retries and when.</returns>
        private RetryPolicyResult ShouldRetry(int currentAttempt, Exception exception)
        {
            if (exception is KeeperException.ConnectionLossException || exception is KeeperException.SessionExpiredException || exception is TimeoutException)
            {
                lock (this.zookeeperLock)
                {
                    this.DisconnectAsync().Wait();
                    this.ConnectAsync().Wait();
                }

                return new RetryPolicyResult(TimeSpan.FromSeconds(2));
            }

            return RetryPolicyResult.NoRetry;
        }

        /// <summary>
        /// Ensure connected to zookeeper cluster.
        /// </summary>
        private async Task EnsureConnectedToZookeeperClusterAsync()
        {
            var connected = false;
            if (this.zookeeper == null || this.zookeeper.State != ZooKeeper.States.CONNECTED)
            {
                connected = await this.ConnectAsync().ConfigureAwait(false);
            }
            else if (this.zookeeper.State == ZooKeeper.States.CONNECTED)
            {
                connected = true;
            }

            if (!connected)
            {
                throw new ZookeeperOperationException("failed to connect to zookeeper cluster.");
            }
        }

        /// <summary>
        /// create zookeeper client.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        private void CreateClient(bool condition)
        {
            if (condition)
            {
                lock (this.zookeeperLock)
                {
                    if (condition)
                    {
                        this.zookeeper = new ZooKeeper(this.ConnectionSettings.ConnectionStrings, TimeSpan.FromMilliseconds(this.ConnectionSettings.SessionTimeoutMilliseconds), this.ConnectionSettings.EventWatcher == null ? null : new ZookeeperEventProxy(this.ConnectionSettings.EventWatcher));
                    }
                }
            }
        }
    }
}
