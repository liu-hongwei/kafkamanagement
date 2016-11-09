// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolTopicMetadataRequest.cs" company="">
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
    /// The kafka request for getting topic metadata.
    /// </summary>
    public class KafkaProtocolTopicMetadataRequest : KafkaProtocolRequest
    {
        /// <summary>
        /// Gets or sets the name of the topic for getting metadata.
        /// The topics to produce metadata for. If empty the request will yield metadata for all topics.
        /// </summary>
        public string[] TopicNames { get; set; }

        /// <summary>
        /// Gets the size of the request.
        /// </summary>
        protected override int BodySize
        {
            get
            {
                // byte size of topics
                int packetByteSize = 4;
                if (this.TopicNames != null && this.TopicNames.Length > 0)
                {
                    for (int i = 0; i < this.TopicNames.Length; i++)
                    {
                        var topicNameBytes = Encoding.UTF8.GetBytes(this.TopicNames[i]);
                        packetByteSize += (short)(2 + topicNameBytes.Length);
                    }
                }
                else
                {
                    packetByteSize += 2;
                }

                return packetByteSize;
            }
        }

        /// <summary>
        /// Gets the api id of current request.
        /// </summary>
        public override KafkaProtocolApiKey ApiKey
        {
            get
            {
                return KafkaProtocolApiKey.MetadataRequest;
            }
        }

        /// <summary>
        /// Gets the bytes of the request.
        /// </summary>
        protected override byte[] BodyBytes
        {
            get
            {
                //-----------------------------------------------------------------------\\
                // +-------------+
                // | size        |  4bytes
                // +-------------+
                // | api key     |  2bytes
                // +-------------+
                // | api version |  2bytes
                // +-------------+
                // |correlationid|  4bytes      
                // +-------------+                   +------+-----------+
                // | clientId    |  string ->        | len  | ....      |
                // +-------------+                   +------+-----------+
                // |             |                    2bytes   {len}bytes
                // | requestbody |                   +------+-----------+ 
                // |             |  array[string]->  | n    | .....     |
                // +-------------+                   +------+-----------+
                //                                    4bytes element 0..n
                //-----------------------------------------------------------------------\\

                byte[] packet = null;
                using (var stream = new MemoryStream())
                {
                    var writer = new BinaryWriter(stream);
                    if (this.TopicNames == null || !this.TopicNames.Any())
                    {
                        this.TopicNames = new string[0];
                    }

                    // write body [topicname]
                    var topicCountBytes = KafkaProtocolPrimitiveType.GetBytes(this.TopicNames.Length);
                    writer.Write(topicCountBytes, 0, topicCountBytes.Length);

                    if (this.TopicNames.Length == 0)
                    {
                        var nullNameBytes = KafkaProtocolPrimitiveType.GetBytes((short)-1);
                        writer.Write(nullNameBytes, 0, nullNameBytes.Length);
                    }
                    else
                    {
                        for (int i = 0; i < this.TopicNames.Length; i++)
                        {
                            var topicNameSizeBytes = KafkaProtocolPrimitiveType.GetBytes((short)this.TopicNames[i].Length);
                            writer.Write(topicNameSizeBytes, 0, topicNameSizeBytes.Length);
                            var topicNameBytes = Encoding.UTF8.GetBytes(this.TopicNames[i]);
                            writer.Write(topicNameBytes, 0, topicNameBytes.Length);
                        }
                    }

                    var size = stream.Length;
                    var buffer = stream.GetBuffer();
                    packet = new byte[size];
                    Array.Copy(buffer, packet, packet.Length);
                }

                return packet;
            }
        }
    }
}