// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZookeeperZnodeStat.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    /// <summary>
    /// The znode state in zookeeper.
    /// see https://zookeeper.apache.org/doc/r3.1.2/zookeeperProgrammers.html
    /// </summary>
    public class ZookeeperZnodeStat
    {
        /// <summary>
        /// Gets or sets ACL version.
        /// </summary>
        public int Aversion { get; set; }

        /// <summary>
        /// Gets or sets the created time (in milliseconds from epoch).
        /// </summary>
        public long Ctime { get; set; }

        /// <summary>
        /// Gets or sets the children version.
        /// </summary>
        public int Cversion { get; set; }

        /// <summary>
        /// Gets or sets the created zookeeper transaction ID).
        /// </summary>
        public long Czxid { get; set; }

        /// <summary>
        /// Gets or sets the length of data field of current znode.
        /// </summary>
        public int DataLength { get; set; }

        /// <summary>
        /// Gets or sets the session id of the owner of this znode if the znode is an ephemeral node. If it is not an ephemeral node, it will be zero.
        /// </summary>
        public long EphemeralOwner { get; set; }

        /// <summary>
        /// Gets or sets the time in milliseconds from epoch when this znode was last modified.
        /// </summary>
        public long Mtime { get; set; }

        /// <summary>
        /// Gets or sets the zxid of the change that last modified this znode.
        /// </summary>
        public long Mzxid { get; set; }

        /// <summary>
        /// Gets or sets the number of children of this znode.
        /// </summary>
        public int NumChildren { get; set; }

        /// <summary>
        /// Gets or sets the number of changes to the data of this znode.
        /// </summary>
        public int Version { get; set; }
    }
}
