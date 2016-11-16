// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetFetchResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The kafka response of fetching offset request.
    /// v0 and v1 supported in kafka 0.8.2 or later.
    /// </summary>
    public class KafkaProtocolOffsetFetchResponse : KafkaProtocolResponse
    {
        /*
            OffsetFetchResponse => [TopicName [Partition Offset Metadata ErrorCode]]
              TopicName => string
              Partition => int32
              Offset => int64
              Metadata => string
              ErrorCode => int16 
        */

        /// <summary>
        /// Gets or sets the topic info in the response.
        /// </summary>
        public KafkaProtocolOffsetFetchResponseTopicInfo[] TopicInfos { get; set; }

        /// <summary>
        /// Parse the response bytes.
        /// </summary>
        /// <param name="response">The bytes of the response.</param>
        public override void Parse(byte[] response)
        {
            using (var stream = new MemoryStream(response))
            {
                var reader = new BinaryReader(stream);

                var topicInfoSizeBytes = new byte[4];
                reader.Read(topicInfoSizeBytes, 0, 4);
                var numOfTopicInfo = KafkaProtocolPrimitiveType.GetInt32(topicInfoSizeBytes);
                this.TopicInfos = new KafkaProtocolOffsetFetchResponseTopicInfo[numOfTopicInfo];
                for (int i = 0; i < numOfTopicInfo; i++)
                {
                    this.TopicInfos[i] = new KafkaProtocolOffsetFetchResponseTopicInfo();

                    // topic name
                    var topicNameSizeBytes = new byte[2];
                    reader.Read(topicNameSizeBytes, 0, 2);
                    var topicNameSize = KafkaProtocolPrimitiveType.GetInt16(topicNameSizeBytes);
                    var topicNameBytes = new byte[topicNameSize];
                    reader.Read(topicNameBytes, 0, topicNameSize);
                    var topicName = Encoding.UTF8.GetString(topicNameBytes);
                    this.TopicInfos[i].TopicName = topicName;

                    // [partition info]
                    var partitionInfoSizeBytes = new byte[4];
                    reader.Read(partitionInfoSizeBytes, 0, 4);
                    var numOfPartitionInfos = KafkaProtocolPrimitiveType.GetInt32(partitionInfoSizeBytes);
                    this.TopicInfos[i].PartitionInfos = new KafkaProtocolOffsetFetchResponsePartitionInfo[numOfPartitionInfos];
                    for (int j = 0; j < numOfPartitionInfos; j++)
                    {
                        this.TopicInfos[i].PartitionInfos[j] = new KafkaProtocolOffsetFetchResponsePartitionInfo();

                        // partition id
                        var partitionBytes = new byte[4];
                        reader.Read(partitionBytes, 0, 4);
                        var partitionId = KafkaProtocolPrimitiveType.GetInt32(partitionBytes);
                        this.TopicInfos[i].PartitionInfos[j].Partition = partitionId;

                        // offset
                        var offsetBytes = new byte[8];
                        reader.Read(offsetBytes, 0, 8);
                        var offset = KafkaProtocolPrimitiveType.GetInt64(offsetBytes);
                        this.TopicInfos[i].PartitionInfos[j].Offset = offset;

                        // metadata
                        var metadataSizeBytes = new byte[2];
                        reader.Read(metadataSizeBytes, 0, 2);
                        var metadataSize = KafkaProtocolPrimitiveType.GetInt16(metadataSizeBytes);
                        var metadataBytes = new byte[metadataSize];
                        reader.Read(metadataBytes, 0, metadataSize);
                        var metadata = Encoding.UTF8.GetString(metadataBytes);
                        this.TopicInfos[i].PartitionInfos[j].Metadata = metadata;

                        // error code
                        var errorCodeBytes = new byte[2];
                        reader.Read(errorCodeBytes, 0, 2);
                        var errorCode = KafkaProtocolPrimitiveType.GetInt16(errorCodeBytes);
                        this.TopicInfos[i].PartitionInfos[j].ErrorCode = errorCode;
                    }
                }
            }
        }
    }
}