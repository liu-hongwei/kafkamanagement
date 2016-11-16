// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetListRequest.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The kafka request to list offset.
    /// This API describes the valid offset range available for a set of topic-partitions. 
    /// As with the produce and fetch APIs requests must be directed to the broker that is currently the leader for the partitions in question. 
    /// This can be determined using the metadata API.
    /// </summary>
    public class KafkaProtocolOffsetListRequest : KafkaProtocolRequest
    {
        /*
        OffsetRequest => ReplicaId [TopicName [Partition Time MaxNumberOfOffsets]]
          ReplicaId => int32
          TopicName => string
          Partition => int32
          Time => int64
          MaxNumberOfOffsets => int32 
        */

        /// <summary>
        /// Gets or sets the id of broker.
        /// int32
        /// Broker id of the follower. For normal consumers, use -1.
        /// </summary>
        public int ReplicaId { get; set; }

        /// <summary>
        /// Gets or sets the topic info in the request.
        /// </summary>
        public KafkaProtocolOffsetListRequestTopicInfo[] TopicInfos { get; set; }

        /// <summary>
        /// Gets the size of the request.
        /// </summary>
        protected override int BodySize
        {
            get
            {
                // size of replicaId : int32 , 4bytes
                int size = 4;

                // size of the array : int32, 4bytes
                size += 4;
                if (this.TopicInfos != null && this.TopicInfos.Any())
                {
                    foreach (var topic in this.TopicInfos)
                    {
                        // topic name, string
                        size += 2 + topic.TopicName.Length;

                        // size of array
                        size += 4;
                        if (topic.PartitionInfos != null && topic.PartitionInfos.Any())
                        {
                            for (var i = 0; i < topic.PartitionInfos.Length; i++)
                            {
                                // size of partition id
                                size += 4;

                                // size of timestamp
                                size += 8;

                                // size of maxnumberofoffsets
                                size += 4;
                            }
                        }
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
                // This API describes the valid offset range available for a set of topic-partitions. 
                // As with the produce and fetch APIs requests must be directed to the broker that is currently the leader for 
                // the partitions in question. This can be determined using the metadata API.
                //------------------------------------------------------\\
                // OffsetRequest => ReplicaId[TopicName[Partition Time MaxNumberOfOffsets]]
                //  ReplicaId => int32
                //  TopicName => string
                //  Partition => int32
                //  Time => int64
                //  MaxNumberOfOffsets => int32
                //------------------------------------------------------\\
                byte[] body;
                using (var stream = new MemoryStream())
                {
                    var writer = new BinaryWriter(stream);

                    // replica id
                    var replicaIdBytes = KafkaProtocolPrimitiveType.GetBytes(this.ReplicaId);
                    writer.Write(replicaIdBytes, 0, replicaIdBytes.Length);

                    // array of topic info
                    if (this.TopicInfos == null || this.TopicInfos.Length == 0)
                    {
                        var topicInfosSizeBytes = KafkaProtocolPrimitiveType.GetBytes(0);
                        writer.Write(topicInfosSizeBytes, 0, topicInfosSizeBytes.Length);
                    }
                    else
                    {
                        var topicInfosSizeBytes = KafkaProtocolPrimitiveType.GetBytes(this.TopicInfos.Length);
                        writer.Write(topicInfosSizeBytes, 0, topicInfosSizeBytes.Length);

                        for (int i = 0; i < this.TopicInfos.Length; i++)
                        {
                            // topic name
                            var topicNameSizeBytes = KafkaProtocolPrimitiveType.GetBytes((short)this.TopicInfos[i].TopicName.Length);
                            writer.Write(topicNameSizeBytes, 0, topicNameSizeBytes.Length);
                            var topicNameBytes = Encoding.UTF8.GetBytes(this.TopicInfos[i].TopicName);
                            writer.Write(topicNameBytes, 0, topicNameBytes.Length);

                            // array of partition info
                            if (this.TopicInfos[i].PartitionInfos == null ||
                                this.TopicInfos[i].PartitionInfos.Length == 0)
                            {
                                var partitionInfosSizeBytes = KafkaProtocolPrimitiveType.GetBytes(0);
                                writer.Write(partitionInfosSizeBytes, 0, partitionInfosSizeBytes.Length);
                            }
                            else
                            {
                                var partitionInfosSizeBytes = KafkaProtocolPrimitiveType.GetBytes(this.TopicInfos[i].PartitionInfos.Length);
                                writer.Write(partitionInfosSizeBytes, 0, partitionInfosSizeBytes.Length);

                                for (int j = 0; j < this.TopicInfos[i].PartitionInfos.Length; j++)
                                {
                                    // partition id
                                    var partitionIdBytes = KafkaProtocolPrimitiveType.GetBytes(this.TopicInfos[i].PartitionInfos[j].Partition);
                                    writer.Write(partitionIdBytes, 0, partitionIdBytes.Length);

                                    // time
                                    var timeBytes = KafkaProtocolPrimitiveType.GetBytes(this.TopicInfos[i].PartitionInfos[j].Timestamp);
                                    writer.Write(timeBytes, 0, timeBytes.Length);

                                    // MaxNumberOfOffsets
                                    var maxNumberOfOffsetsBytes = KafkaProtocolPrimitiveType.GetBytes(this.TopicInfos[i].PartitionInfos[j].MaxNumberOfOffsets);
                                    writer.Write(maxNumberOfOffsetsBytes, 0, maxNumberOfOffsetsBytes.Length);
                                }
                            }
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
                return KafkaProtocolApiKey.OffsetRequest;
            }
        }

    }
}
