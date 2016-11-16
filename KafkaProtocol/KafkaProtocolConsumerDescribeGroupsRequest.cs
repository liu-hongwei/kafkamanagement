// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerDescribeGroupsRequest.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The request for describing consumer groups.
    /// </summary>
    public class KafkaProtocolConsumerDescribeGroupsRequest : KafkaProtocolRequest
    {
        /*
            DescribeGroupsRequest => [GroupId]
              GroupId => string 
        */

        /// <summary>
        /// Gets or sets the ids of groups.
        /// </summary>
        public string[] GroupIds { get; set; }

        /// <summary>
        /// Gets the size of the request.
        /// </summary>
        protected override int BodySize
        {
            get
            {
                int size = 4;
                if (this.GroupIds != null && this.GroupIds.Any())
                {
                    for (int i = 0; i < this.GroupIds.Length; i++)
                    {
                        size += 2 + this.GroupIds[i].Length;
                    }
                }

                return size;
            }
        }

        /// <summary>
        /// Gets the bytes of the request packet.
        /// </summary>
        protected override byte[] BodyBytes
        {
            get
            {
                byte[] body;
                using (var stream = new MemoryStream())
                {
                    var writer = new BinaryWriter(stream);

                    if (this.GroupIds == null)
                    {
                        var groupIdSizeBytes = KafkaProtocolPrimitiveType.GetBytes(0);
                        writer.Write(groupIdSizeBytes, 0, groupIdSizeBytes.Length);
                    }
                    else
                    {
                        var groupIdSizeBytes = KafkaProtocolPrimitiveType.GetBytes(this.GroupIds.Length);
                        writer.Write(groupIdSizeBytes, 0, groupIdSizeBytes.Length);

                        foreach (var groupId in this.GroupIds)
                        {
                            var idSizeBytes = KafkaProtocolPrimitiveType.GetBytes((short)groupId.Length);
                            writer.Write(idSizeBytes, 0, idSizeBytes.Length);
                            var idBytes = Encoding.UTF8.GetBytes(groupId);
                            writer.Write(idBytes, 0, idBytes.Length);
                        }
                    }

                    var size = stream.Length;
                    var buffer = stream.GetBuffer();
                    body = new byte[size];
                    Array.Copy(buffer, body, body.Length);
                }

                return body;
            }
        }

        /// <summary>
        /// Gets the api id of current request.
        /// </summary>
        public override KafkaProtocolApiKey ApiKey
        {
            get
            {
                return KafkaProtocolApiKey.DescribeGroupsRequest;
            }
        }
    }
}