// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZookeeperZnodeAclIdentity.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    /// <summary>
    /// supported znode ACL schemas.
    /// https://zookeeper.apache.org/doc/r3.1.2/zookeeperProgrammers.html#sc_ZooKeeperAccessControl
    /// </summary>
    public class ZookeeperZnodeAclIdentity
    {
        /// <summary>
        /// Gets or sets the ACL schema.
        /// </summary>
        public string Schema { get; protected set; }

        /// <summary>
        /// Gets or sets the identity.
        /// </summary>
        public string Identity { get; protected set; }

        /// <summary>
        /// schema of auth doesn't use any id, represents any authenticated user.
        /// </summary>
        public static ZookeeperZnodeAclIdentity AuthIdentity()
        {
            return new ZookeeperZnodeAclIdentity() { Schema = "auth", Identity = string.Empty };
        }

        /// <summary>
        /// schema of digest uses a username:password string to generate MD5 hash which is then used as an ACL ID identity. Authentication is done by sending the username:password in clear text. When used in the ACL the expression will be the username:base64 encoded SHA1 password digest.
        /// </summary>
        /// <param name="username">the user name.</param>
        /// <param name="password">the password.</param>
        public static ZookeeperZnodeAclIdentity DigestIdentity(string username = "", string password = "")
        {
            var identity = string.Empty;
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                identity = username + ":" + password;
            }

            return new ZookeeperZnodeAclIdentity() { Schema = "digest", Identity = identity };
        }

        /// <summary>
        /// host uses the client host name as an ACL ID identity. The ACL expression is a hostname suffix. For example, the ACL expression host:corp.com matches the ids host:host1.corp.com and host:host2.corp.com, but not host:host1.store.com.
        /// </summary>
        /// <param name="hostname">the host name.</param>
        public static ZookeeperZnodeAclIdentity HostIdentity(string hostname = "")
        {
            return new ZookeeperZnodeAclIdentity() { Schema = "host", Identity = hostname };
        }

        /// <summary>
        /// ip uses the client host IP as an ACL ID identity. The ACL expression is of the form addr/bits where the most significant bits of addr are matched against the most significant bits of the client host IP.
        /// https://zookeeper.apache.org/doc/r3.1.2/zookeeperProgrammers.html#sc_ZooKeeperAccessControl
        /// </summary>
        /// <param name="ipWithBits">expression is of the form addr/bits where the most significant bits of addr are matched against the most significant bits of the client host IP.</param>
        public static ZookeeperZnodeAclIdentity IPIdentity(string ipWithBits = "")
        {
            return new ZookeeperZnodeAclIdentity() { Schema = "ip", Identity = ipWithBits };
        }

        /// <summary>
        /// schema of world has a single id, anyone, that represents anyone.
        /// </summary>
        public static ZookeeperZnodeAclIdentity WorldIdentity()
        {
            return new ZookeeperZnodeAclIdentity() { Schema = "world", Identity = "anyone" };
        }

    }
}