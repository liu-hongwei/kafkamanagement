// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolRequest.cs" company="">
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
    /// The base model of a kafka request.
    /// </summary>
    public abstract class KafkaProtocolRequest
    {
        /// <summary>
        /// Gets the size of the request.
        /// </summary>
        public int Size
        {
            get
            {
                return this.HeaderSize + this.BodySize;
            }
        }

        /// <summary>
        /// Gets the bytes of the request packet.
        /// </summary>
        public byte[] Packet
        {
            get
            {
                var header = this.HeaderBytes;
                var body = this.BodyBytes;
                if (header == null || !header.Any())
                {
                    // this should never happen
                    return null;
                }

                var size = header.Length;
                var packet = new byte[size];
                Array.Copy(header, 0, packet, 0, header.Length);
                if (body != null && body.Any())
                {
                    size += body.Length;
                    var buffer = new byte[size];
                    Array.Copy(packet, 0, buffer, 0, packet.Length);
                    Array.Copy(body, 0, buffer, header.Length, body.Length);
                    packet = buffer;
                }

                return packet;
            }
        }

        /// <summary>
        /// Gets the api id of current request.
        /// </summary>
        public abstract KafkaProtocolApiKey ApiKey { get; }

        /// <summary>
        /// Gets the api version.
        /// </summary>
        public virtual KafkaProtocolApiVersion ApiVersion
        {
            get
            {
                return KafkaProtocolApiVersion.V0;
            }
        }

        /// <summary>
        /// Gets the identifier who makes the request.
        /// </summary>
        public virtual string ClientId
        {
            get
            {
                return Guid.NewGuid().ToString("N");
            }
        }

        /// <summary>
        /// Gets the correlation id identify this request.
        /// </summary>
        public virtual int CorrelationId
        {
            get
            {
                return (int)(DateTime.UtcNow.Ticks / int.MaxValue);
            }
        }

        /// <summary>
        /// Gets the size of request header.
        /// </summary>
        protected int HeaderSize
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
                // |  body       |                    2bytes   {len}bytes
                // +-------------+
                //-----------------------------------------------------------------------\\

                // the fixed length of 'size','apikey','apiversion,'correlationId'
                int headerByteSize = 12;

                // byte size of clientId (string type has 2 bytes to indicates the length)
                var clientIdBytes = Encoding.UTF8.GetBytes(this.ClientId);
                headerByteSize += (short)(2 + clientIdBytes.Length);

                return headerByteSize;
            }
        }

        /// <summary>
        /// Gets the bytes of header.
        /// </summary>
        protected byte[] HeaderBytes
        {
            get
            {
                byte[] header = null;
                using (var stream = new MemoryStream())
                {
                    var writer = new BinaryWriter(stream);

                    // write size
                    var sizeBytes = KafkaProtocolPrimitiveType.GetBytes((int)(this.Size - 4));
                    writer.Write(sizeBytes, 0, sizeBytes.Length);

                    // write apikey
                    var apiKeyBytes = KafkaProtocolPrimitiveType.GetBytes((short)KafkaProtocolApiKey.MetadataRequest);
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

                    var size = stream.Length;
                    var buffer = stream.GetBuffer();
                    header = new byte[size];
                    Array.Copy(buffer, header, header.Length);
                }

                return header;
            }
        }

        /// <summary>
        /// Gets the size of the request body.
        /// </summary>
        protected abstract int BodySize { get; }

        /// <summary>
        /// Gets the bytes of the request packet.
        /// </summary>
        protected abstract byte[] BodyBytes { get; }
    }
}