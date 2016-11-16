// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolConsumerDescribeGroupsResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// The response of consumer groups describe request.
    /// </summary>
    public class KafkaProtocolConsumerDescribeGroupsResponse : KafkaProtocolResponse
    {
        /*
         DescribeGroupsResponse => [ErrorCode GroupId State ProtocolType Protocol Members]
          ErrorCode => int16
          GroupId => string
          State => string
          ProtocolType => string
          Protocol => string
          Members => [MemberId ClientId ClientHost MemberMetadata MemberAssignment]
            MemberId => string
            ClientId => string
            ClientHost => string
            MemberMetadata => bytes
            MemberAssignment => bytes
        */

        /// <summary>
        /// Gets or sets the responsed consumer group info.
        /// </summary>
        public KafkaProtocolConsumerDescribeGroupsResponseGroupInfo[] GroupInfos { get; set; }

        /// <summary>
        /// Parse the response bytes.
        /// </summary>
        /// <param name="response">The bytes of the response.</param>
        public override void Parse(byte[] response)
        {
            using (var stream = new MemoryStream(response))
            {
                var reader = new BinaryReader(stream);

                var sizeBytes = new byte[4];
                reader.Read(sizeBytes, 0, 4);
                var numOfGroupInfos = KafkaProtocolPrimitiveType.GetInt32(sizeBytes);

                this.GroupInfos = new KafkaProtocolConsumerDescribeGroupsResponseGroupInfo[numOfGroupInfos];
                for (int i = 0; i < numOfGroupInfos; i++)
                {
                    GroupInfos[i] = new KafkaProtocolConsumerDescribeGroupsResponseGroupInfo();

                    // error code
                    var errorCodeBytes = new byte[2];
                    reader.Read(errorCodeBytes, 0, 2);
                    GroupInfos[i].ErrorCode = KafkaProtocolPrimitiveType.GetInt16(errorCodeBytes);

                    // groupId
                    var groupIdSizeBytes = new byte[2];
                    reader.Read(groupIdSizeBytes, 0, 2);
                    var groupIdSize = KafkaProtocolPrimitiveType.GetInt16(groupIdSizeBytes);
                    var groupIdBytes = new byte[groupIdSize];
                    reader.Read(groupIdBytes, 0, groupIdSize);
                    GroupInfos[i].GroupId = Encoding.UTF8.GetString(groupIdBytes);

                    // state
                    var stateSizeBytes = new byte[2];
                    reader.Read(stateSizeBytes, 0, 2);
                    var stateSize = KafkaProtocolPrimitiveType.GetInt16(stateSizeBytes);
                    var stateBytes = new byte[stateSize];
                    reader.Read(stateBytes, 0, stateSize);
                    GroupInfos[i].State = Encoding.UTF8.GetString(stateBytes);

                    // protocol type
                    var protocolTypeSizeBytes = new byte[2];
                    reader.Read(protocolTypeSizeBytes, 0, 2);
                    var protocolTypeSize = KafkaProtocolPrimitiveType.GetInt16(protocolTypeSizeBytes);
                    var protocolTypeBytes = new byte[protocolTypeSize];
                    reader.Read(protocolTypeBytes, 0, protocolTypeSize);
                    GroupInfos[i].ProtocolType = Encoding.UTF8.GetString(protocolTypeBytes);

                    // protocol
                    var protocolSizeBytes = new byte[2];
                    reader.Read(protocolSizeBytes, 0, 2);
                    var protocolSize = KafkaProtocolPrimitiveType.GetInt16(protocolSizeBytes);
                    var protocolBytes = new byte[protocolSize];
                    reader.Read(protocolBytes, 0, protocolSize);
                    GroupInfos[i].Protocol = Encoding.UTF8.GetString(protocolBytes);

                    // [Members]
                    var membersSizeBytes = new byte[4];
                    reader.Read(membersSizeBytes, 0, 4);
                    var numOfMembers = KafkaProtocolPrimitiveType.GetInt32(membersSizeBytes);
                    GroupInfos[i].Members = new KafkaProtocolConsumerDescribeGroupsResponseMemberInfo[numOfMembers];
                    for (int j = 0; j < numOfMembers; j++)
                    {
                        GroupInfos[i].Members[j] = new KafkaProtocolConsumerDescribeGroupsResponseMemberInfo();

                        // memberid
                        var memberIdSizeBytes = new byte[2];
                        reader.Read(memberIdSizeBytes, 0, 2);
                        var memberIdSize = KafkaProtocolPrimitiveType.GetInt16(memberIdSizeBytes);
                        var memberBytes = new byte[memberIdSize];
                        reader.Read(memberBytes, 0, memberIdSize);
                        GroupInfos[i].Members[j].MemberId = Encoding.UTF8.GetString(memberBytes);

                        // client id
                        var clientIdSizeBytes = new byte[2];
                        reader.Read(clientIdSizeBytes, 0, 2);
                        var clientIdSize = KafkaProtocolPrimitiveType.GetInt16(clientIdSizeBytes);
                        var clientIdBytes = new byte[clientIdSize];
                        reader.Read(clientIdBytes, 0, clientIdSize);
                        GroupInfos[i].Members[j].ClientId = Encoding.UTF8.GetString(clientIdBytes);

                        // client host
                        var clientHostSizeBytes = new byte[2];
                        reader.Read(clientHostSizeBytes, 0, 2);
                        var clientHostSize = KafkaProtocolPrimitiveType.GetInt16(clientHostSizeBytes);
                        var clientHostBytes = new byte[clientHostSize];
                        reader.Read(clientHostBytes, 0, clientHostSize);
                        GroupInfos[i].Members[j].ClientHost = Encoding.UTF8.GetString(clientHostBytes);

                        // member metadata
                        var memberMetadataSizeBytes = new byte[4];
                        reader.Read(memberMetadataSizeBytes, 0, 4);
                        var memberMetadataSize = KafkaProtocolPrimitiveType.GetInt32(memberMetadataSizeBytes);
                        var memberMetadataBytes = new byte[memberMetadataSize];
                        reader.Read(memberMetadataBytes, 0, memberMetadataSize);
                        GroupInfos[i].Members[j].MemberMetadata = memberMetadataBytes;

                        // member assignment
                        var memberAssignmentSizeBytes = new byte[4];
                        reader.Read(memberAssignmentSizeBytes, 0, 4);
                        var memberAssignmentSize = KafkaProtocolPrimitiveType.GetInt32(memberAssignmentSizeBytes);
                        var memberAssignmentBytes = new byte[memberAssignmentSize];
                        reader.Read(memberAssignmentBytes, 0, memberAssignmentSize);
                        GroupInfos[i].Members[j].MemberAssignment = memberAssignmentBytes;
                    }
                }
            }
        }
    }
}