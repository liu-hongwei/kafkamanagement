// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolOffsetFetchRequest.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The kafka request to fetch offset.
    /// v0 and v1 supported in kafka 0.8.2 or later. v0 read offsets from zookeeper, v1 read offsets from kafka.
    /// </summary>
    public class KafkaProtocolOffsetFetchRequest : KafkaProtocolRequest
    {
        /*
            OffsetFetchRequest => ConsumerGroup [TopicName [Partition]]
              ConsumerGroup => string
              TopicName => string
              Partition => int32
        */

        /// <summary>
        /// Gets or sets the consumer group.
        /// </summary>
        public string ConsumerGroup { get; set; }

        /// <summary>
        /// Gets or sets the topic info for fetching.
        /// </summary>
        public KafkaProtocolOffsetFetchRequestTopicInfo[] TopicInfos { get; set; }

        /// <summary>
        /// Gets the api version.
        /// </summary>
        public override KafkaProtocolApiVersion ApiVersion
        {
            // There is no format difference between Offset Fetch Request v0 and v1. 
            // Functionality wise, Offset Fetch Request v0 will fetch offset from zookeeper, 
            // Offset Fetch Request v1 will fetch offset from Kafka.
            get
            {
                return KafkaProtocolApiVersion.V1;
            }
        }

        /// <summary>
        /// Gets the size of the request.
        /// </summary>
        protected override int BodySize
        {
            get
            {
                var size = 0;

                // consumer group
                size += 2 + this.ConsumerGroup.Length;

                // [topicinfo]
                size += 4;
                if (this.TopicInfos != null && this.TopicInfos.Any())
                {
                    for (int i = 0; i < this.TopicInfos.Length; i++)
                    {
                        // topic name
                        size += 2 + this.TopicInfos[i].TopicName.Length;

                        // [partitioninfo]
                        size += 4;

                        if (this.TopicInfos[i].Partitions != null && this.TopicInfos[i].Partitions.Any())
                        {
                            for (int j = 0; j < this.TopicInfos[i].Partitions.Length; j++)
                            {
                                // partition
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
                byte[] body;
                using (var stream = new MemoryStream())
                {
                    var writer = new BinaryWriter(stream);

                    // consumer group
                    var consumerGroupSizeBytes = KafkaProtocolPrimitiveType.GetBytes((short)this.ConsumerGroup.Length);
                    writer.Write(consumerGroupSizeBytes, 0, consumerGroupSizeBytes.Length);
                    var consumerGroupBytes = Encoding.UTF8.GetBytes(this.ConsumerGroup);
                    writer.Write(consumerGroupBytes, 0, consumerGroupBytes.Length);

                    if (this.TopicInfos == null)
                    {
                        var topicInfoSizeBytes = KafkaProtocolPrimitiveType.GetBytes(0);
                        writer.Write(topicInfoSizeBytes, 0, topicInfoSizeBytes.Length);
                    }
                    else
                    {
                        // [topicinfo]
                        var topicInfoSizeBytes = KafkaProtocolPrimitiveType.GetBytes(this.TopicInfos.Length);
                        writer.Write(topicInfoSizeBytes, 0, topicInfoSizeBytes.Length);
                        foreach (var topicInfo in this.TopicInfos)
                        {
                            // topic name
                            var topicNameSizeBytes = KafkaProtocolPrimitiveType.GetBytes((short)topicInfo.TopicName.Length);
                            writer.Write(topicNameSizeBytes, 0, topicNameSizeBytes.Length);
                            var topicNameBytes = Encoding.UTF8.GetBytes(topicInfo.TopicName);
                            writer.Write(topicNameBytes, 0, topicNameBytes.Length);

                            if (topicInfo.Partitions == null)
                            {
                                var partitionSizeBytes = KafkaProtocolPrimitiveType.GetBytes(0);
                                writer.Write(partitionSizeBytes, 0, partitionSizeBytes.Length);
                            }
                            else
                            {
                                // partition
                                var partitionSizeBytes = KafkaProtocolPrimitiveType.GetBytes(topicInfo.Partitions.Length);
                                writer.Write(partitionSizeBytes, 0, partitionSizeBytes.Length);
                                foreach (var partition in topicInfo.Partitions)
                                {
                                    var partitionBytes = KafkaProtocolPrimitiveType.GetBytes(partition);
                                    writer.Write(partitionBytes, 0, partitionBytes.Length);
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
                return KafkaProtocolApiKey.OffsetFetchRequest;
            }
        }
    }
}