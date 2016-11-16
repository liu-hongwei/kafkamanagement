// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerGroupCoordinatorRequest.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The request of getting consumer group coordinator.
    /// </summary>
    public class KafkaProtocolConsumerGroupCoordinatorRequest : KafkaProtocolRequest
    {
        /*
            GroupCoordinatorRequest => GroupId
              GroupId => string
        */

        /// <summary>
        /// Gets or sets the consumer group id.
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets the size of the request.
        /// </summary>
        protected override int BodySize
        {
            get
            {
                var size = 0;

                // group id
                size += 2 + this.GroupId.Length;

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

                    var groupIdSizeBytes = KafkaProtocolPrimitiveType.GetBytes((short)this.GroupId.Length);
                    writer.Write(groupIdSizeBytes, 0, groupIdSizeBytes.Length);
                    var groupIdBytes = Encoding.UTF8.GetBytes(this.GroupId);
                    writer.Write(groupIdBytes, 0, groupIdBytes.Length);

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
                return KafkaProtocolApiKey.GroupCoordinatorRequest;
            }
        }
    }
}