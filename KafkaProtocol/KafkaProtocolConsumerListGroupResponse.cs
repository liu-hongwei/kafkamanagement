// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerListGroupResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// The response to the list group request.
    /// </summary>
    public class KafkaProtocolConsumerListGroupResponse : KafkaProtocolResponse
    {
        /// <summary>
        /// Gets or sets the error code.
        /// int16
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the consumer group info.
        /// </summary>
        public KafkaProtocolConsumerListGroupResponseGroupInfo[] Groups { get; set; }

        /// <summary>
        /// Parse the response bytes.
        /// </summary>
        /// <param name="response">The bytes of the response.</param>
        public override void Parse(byte[] response)
        {
            // ----------------------------------------------\\
            // ListGroupsResponse => ErrorCode Groups
            //  ErrorCode => int16
            //  Groups => [GroupId ProtocolType]
            //      GroupId => string
            //      ProtocolType => string
            // ----------------------------------------------\\

            using (var stream = new MemoryStream(response))
            {
                var reader = new BinaryReader(stream);

                // errorCode
                var errorCodeBytes = new byte[2];
                reader.Read(errorCodeBytes, 0, 2);
                this.ErrorCode = KafkaProtocolPrimitiveType.GetInt16(errorCodeBytes);

                // [groups]
                var groupSizeBytes = new byte[4];
                reader.Read(groupSizeBytes, 0, 4);
                var numOfGroups = KafkaProtocolPrimitiveType.GetInt32(groupSizeBytes);
                this.Groups = new KafkaProtocolConsumerListGroupResponseGroupInfo[numOfGroups];

                for (int i = 0; i < numOfGroups; i++)
                {
                    this.Groups[i] = new KafkaProtocolConsumerListGroupResponseGroupInfo();

                    // groupId : 2bytes , string
                    var groupIdSizeBytes = new byte[2];
                    reader.Read(groupIdSizeBytes, 0, 2);
                    var groupIdSize = KafkaProtocolPrimitiveType.GetInt16(groupIdSizeBytes);
                    var groupIdBytes = new byte[groupIdSize];
                    reader.Read(groupIdBytes, 0, groupIdSize);
                    this.Groups[i].GroupId = Encoding.UTF8.GetString(groupIdBytes);

                    // protocolType : 2bytes , string
                    var protocolSizeBytes = new byte[2];
                    reader.Read(protocolSizeBytes, 0, 2);
                    var protocolSize = KafkaProtocolPrimitiveType.GetInt16(protocolSizeBytes);
                    var protocolBytes = new byte[protocolSize];
                    reader.Read(protocolBytes, 0, protocolSize);
                    this.Groups[i].ProtocolType = Encoding.UTF8.GetString(protocolBytes);
                }
            }
        }
    }
}