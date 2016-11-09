// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZookeeperOperationException.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The zookeeper operation related exception.
    /// </summary>
    [Serializable]
    public class ZookeeperOperationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZookeeperOperationException" /> class.
        /// </summary>
        public ZookeeperOperationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZookeeperOperationException" /> class.
        /// </summary>
        /// <param name="message">the exception message.</param>
        public ZookeeperOperationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZookeeperOperationException" /> class.
        /// </summary>
        /// <param name="message">the exception message.</param>
        /// <param name="exception">the inner exception.</param>
        public ZookeeperOperationException(string message, Exception exception) : base(message, exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZookeeperOperationException" /> class.
        /// </summary>
        /// <param name="info">the serialization info.</param>
        /// <param name="context">the streaming context.</param>
        protected ZookeeperOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
