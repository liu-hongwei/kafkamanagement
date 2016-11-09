// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerListGroupRequest.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// The request to list consumer groups.
    /// </summary>
    public class KafkaProtocolConsumerListGroupRequest : KafkaProtocolRequest
    {
        /// <summary>
        /// Gets the size of the request.
        /// </summary>
        protected override int BodySize
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
                int packetByteSize = 12;

                // byte size of clientId (string type has 2 bytes to indicates the length)
                var clientIdBytes = Encoding.UTF8.GetBytes(this.ClientId);
                packetByteSize += (short)(2 + clientIdBytes.Length);

                // request body is empty ,so no size

                return packetByteSize;
            }
        }

        /// <summary>
        /// Gets the bytes of the request packet.
        /// </summary>
        protected override byte[] BodyBytes
        {
            get
            {
                byte[] packet = null;
                using (var stream = new MemoryStream())
                {
                    var writer = new BinaryWriter(stream);

                    // write size
                    var sizeBytes = KafkaProtocolPrimitiveType.GetBytes((int)(this.BodySize - 4));
                    writer.Write(sizeBytes, 0, sizeBytes.Length);

                    // write apikey
                    var apiKeyBytes = KafkaProtocolPrimitiveType.GetBytes((short)KafkaProtocolApiKey.ListGroupsRequest);
                    writer.Write(apiKeyBytes, 0, apiKeyBytes.Length);

                    // write apiversion
                    var apiVersionBytes = KafkaProtocolPrimitiveType.GetBytes((short)KafkaProtocolApiVersion.V0);
                    writer.Write(apiVersionBytes, 0, apiVersionBytes.Length);

                    // write correlationId
                    var correlationIdBytes = KafkaProtocolPrimitiveType.GetBytes(this.CorrelationId);
                    writer.Write(correlationIdBytes, 0, correlationIdBytes.Length);

                    // write clientId
                    var clientIdBytes = Encoding.UTF8.GetBytes(this.ClientId);
                    var clientIdSizeBytes = KafkaProtocolPrimitiveType.GetBytes((short)clientIdBytes.Length);
                    writer.Write(clientIdSizeBytes, 0, clientIdSizeBytes.Length);
                    writer.Write(clientIdBytes);

                    // request body is empty.

                    var size = stream.Length;
                    var buffer = stream.GetBuffer();
                    packet = new byte[size];
                    Array.Copy(buffer, packet, packet.Length);
                }

                return packet;
            }
        }

        /// <summary>
        /// Gets the api id of current request.
        /// </summary>
        public override KafkaProtocolApiKey ApiKey
        {
            get
            {
                return KafkaProtocolApiKey.ListGroupsRequest;
            }
        }
    }
}