// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerGroupCoordinatorResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The response of getting consumer group coordinator request.
    /// </summary>
    public class KafkaProtocolConsumerGroupCoordinatorResponse : KafkaProtocolResponse
    {
        /*
        GroupCoordinatorResponse => ErrorCode CoordinatorId CoordinatorHost CoordinatorPort
          ErrorCode => int16
          CoordinatorId => int32
          CoordinatorHost => string
          CoordinatorPort => int32
         */

        /// <summary>
        /// Gets or sets the error code.
        /// int16
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the coordinator id.
        /// int32
        /// </summary>
        public int CoordinatorId { get; set; }

        /// <summary>
        /// Gets or sets the coordinator host info.
        /// </summary>
        public string CoordinatorHost { get; set; }

        /// <summary>
        /// Gets or sets the coordinator port.
        /// int32
        /// </summary>
        public int CoordinatorPort { get; set; }

        /// <summary>
        /// Parse the response bytes.
        /// </summary>
        /// <param name="response">The bytes of the response.</param>
        public override void Parse(byte[] response)
        {
            using (var stream = new MemoryStream(response))
            {
                var reader = new BinaryReader(stream);

                var errorCodeBytes = new byte[2];
                reader.Read(errorCodeBytes, 0, 2);
                this.ErrorCode = KafkaProtocolPrimitiveType.GetInt16(errorCodeBytes);

                var coordinatorIdBytes = new byte[4];
                reader.Read(coordinatorIdBytes, 0, 4);
                this.CoordinatorId = KafkaProtocolPrimitiveType.GetInt32(coordinatorIdBytes);

                var coordinatorHostSizeBytes = new byte[2];
                reader.Read(coordinatorHostSizeBytes, 0, 2);
                var coordinatorHostSize = KafkaProtocolPrimitiveType.GetInt16(coordinatorHostSizeBytes);
                var coordinatorHostBytes = new byte[coordinatorHostSize];
                reader.Read(coordinatorHostBytes, 0, coordinatorHostSize);
                this.CoordinatorHost = Encoding.UTF8.GetString(coordinatorHostBytes);

                var coordinatorPortBytes = new byte[4];
                reader.Read(coordinatorPortBytes, 0, 4);
                this.CoordinatorPort = KafkaProtocolPrimitiveType.GetInt32(coordinatorPortBytes);

            }
        }
    }
}