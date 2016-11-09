// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZookeeperZnodeAclRights.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Kafka.Management
{
    using System;

   /// <summary>
    /// zookeeper znode ACL permissions.
    /// https://zookeeper.apache.org/doc/r3.1.2/zookeeperProgrammers.html#sc_ACLPermissions
    /// </summary>
    [Flags]
    public enum ZookeeperZnodeAclRights
    {
        /// <summary>
        /// you can do nothing.
        /// </summary>
        None = 0,

        /// <summary>
        /// you can get data from a node and list its children.
        /// </summary>
        Read = 1 << 0,

        /// <summary>
        /// you can set data for a node.
        /// </summary>
        Write = 1 << 1,

        /// <summary>
        /// you can create a child node.
        /// </summary>
        Create = 1 << 2,

        /// <summary>
        /// you can delete a child node.
        /// </summary>
        Delete = 1 << 3,

        /// <summary>
        /// you can set permissions.
        /// </summary>
        Admin = 1 << 4,

        /// <summary>
        /// you can do anything.
        /// </summary>
        All = 1 << 0 | 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4
    }
}