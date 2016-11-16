// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetListResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The kafka response of listing offset.
    /// The response contains the starting offset of each segment for the requested partition as well as the "log end offset" 
    /// i.e. the offset of the next message that would be appended to the given partition.
    /// </summary>
    public class KafkaProtocolOffsetListResponse : KafkaProtocolResponse
    {
        /*
         OffsetResponse => [TopicName [PartitionOffsets]]
          PartitionOffsets => Partition ErrorCode [Offset]
          Partition => int32
          ErrorCode => int16
          Offset => int64
         */

        /// <summary>
        /// Gets or sets the topic info in the response.
        /// </summary>
        public KafkaProtocolOffsetListResponseTopicInfo[] TopicInfos { get; set; }

        /// <summary>
        /// Parse the response bytes.
        /// </summary>
        /// <param name="response">The bytes of the response.</param>
        public override void Parse(byte[] response)
        {
            //---------------------------------------------------\\
            // OffsetResponse => ReplicaId [TopicName[PartitionOffsets]]
            //  PartitionOffsets => Partition ErrorCode[Offset]
            //  Partition => int32
            //  ErrorCode => int16
            //  Offset => int64
            //---------------------------------------------------\\

            using (var stream = new MemoryStream(response))
            {
                var reader = new BinaryReader(stream);

                // size of topic array
                var topicInfoSizeBytes = new byte[4];
                reader.Read(topicInfoSizeBytes, 0, 4);
                var numberOfTopicInfos = KafkaProtocolPrimitiveType.GetInt32(topicInfoSizeBytes);

                this.TopicInfos = new KafkaProtocolOffsetListResponseTopicInfo[numberOfTopicInfos];
                for (int i = 0; i < numberOfTopicInfos; i++)
                {
                    this.TopicInfos[i] = new KafkaProtocolOffsetListResponseTopicInfo();

                    // topic name
                    var topicNameSizeBytes = new byte[2];
                    reader.Read(topicNameSizeBytes, 0, 2);
                    var topicNameSize = KafkaProtocolPrimitiveType.GetInt16(topicNameSizeBytes);
                    var topicNameBytes = new byte[topicNameSize];
                    reader.Read(topicNameBytes, 0, topicNameSize);
                    this.TopicInfos[i].TopicName = Encoding.UTF8.GetString(topicNameBytes);

                    // size of partition offset array
                    var partitionInfoSizeBytes = new byte[4];
                    reader.Read(partitionInfoSizeBytes, 0, 4);
                    var partitionInfoSize = KafkaProtocolPrimitiveType.GetInt32(partitionInfoSizeBytes);
                    this.TopicInfos[i].PartitionInfos = new KafkaProtocolOffsetListResponsePartitionInfo[partitionInfoSize];
                    for (int j = 0; j < partitionInfoSize; j++)
                    {
                        this.TopicInfos[i].PartitionInfos[j] = new KafkaProtocolOffsetListResponsePartitionInfo();

                        // partition id
                        var partitionIdBytes = new byte[4];
                        reader.Read(partitionIdBytes, 0, 4);
                        this.TopicInfos[i].PartitionInfos[j].Partition = KafkaProtocolPrimitiveType.GetInt32(partitionIdBytes);
                        
                        // error code
                        var errorCodeBytes = new byte[2];
                        reader.Read(errorCodeBytes, 0, 2);
                        this.TopicInfos[i].PartitionInfos[j].ErrorCode = KafkaProtocolPrimitiveType.GetInt16(errorCodeBytes);

                        // size of offset array
                        var offsetSizeBytes = new byte[4];
                        reader.Read(offsetSizeBytes, 0, 4);
                        var offsetSize = KafkaProtocolPrimitiveType.GetInt32(offsetSizeBytes);
                        this.TopicInfos[i].PartitionInfos[j].Offset = new long[offsetSize];
                        for (int k = 0; k < offsetSize; k++)
                        {
                            var offsetBytes = new byte[8];
                            reader.Read(offsetBytes, 0, 8);
                            this.TopicInfos[i].PartitionInfos[j].Offset[k] = KafkaProtocolPrimitiveType.GetInt64(offsetBytes);
                        }
                    }
                }
            }
        }
    }
}