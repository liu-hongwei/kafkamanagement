// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZookeeperZnodeAcl.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    /// <summary>
    /// The zookeeper znode access control.
    /// https://zookeeper.apache.org/doc/r3.1.2/zookeeperProgrammers.html#sc_ZooKeeperAccessControl
    /// </summary>
    public class ZookeeperZnodeAcl
    {
        /// <summary>
        /// Gets or sets the access permission bits.
        /// </summary>
        public ZookeeperZnodeAclRights AclRights { get; set; }
        
        /// <summary>
        /// Gets or sets the access identity for selected schema.
        /// </summary>
        public ZookeeperZnodeAclIdentity Identity { get; set; }
    }
}
