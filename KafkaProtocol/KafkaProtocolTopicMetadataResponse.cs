// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolTopicMetadataResponse.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// The kafka response of getting kafka topic metadata request.
    /// </summary>
    public class KafkaProtocolTopicMetadataResponse : KafkaProtocolResponse
    {
        /// <summary>
        /// Gets or sets the brokers.
        /// </summary>
        public KafkaProtocolTopicMetadataResponseBrokerInfo[] BrokerInfos { get; set; }

        /// <summary>
        /// Gets or sets the topic metadata.
        /// </summary>
        public KafkaProtocolTopicMetadataResponseTopicInfo[] TopicInfos { get; set; }

        /// <summary>
        /// Parse the response in bytes.
        /// </summary>
        /// <param name="response">The bytes of the response.</param>
        public override void Parse(byte[] response)
        {
            //-------------------------------------------------------------------\\
            // MetadataResponse => [Broker][TopicMetadata]
            //  Broker => NodeId Host Port  (any number of brokers may be returned)
            //    NodeId => int32
            //    Host => string
            //    Port => int32
            //  TopicMetadata => TopicErrorCode TopicName [PartitionMetadata]
            //    TopicErrorCode => int16
            //  PartitionMetadata => PartitionErrorCode PartitionId Leader Replicas Isr
            //    PartitionErrorCode => int16
            //    PartitionId => int32
            //    Leader => int32
            //    Replicas => [int32]
            //    Isr => [int32]
            //-------------------------------------------------------------------\\

            using (var stream = new MemoryStream(response))
            {
                var reader = new BinaryReader(stream);

                // response body
                // [broker]
                var brokersSizeBytes = new byte[4];
                reader.Read(brokersSizeBytes, 0, 4);
                var numOfBrokers = KafkaProtocolPrimitiveType.GetInt32(brokersSizeBytes);
                this.BrokerInfos = new KafkaProtocolTopicMetadataResponseBrokerInfo[numOfBrokers];
                for (int i = 0; i < numOfBrokers; i++)
                {
                    // brokerId , 4bytes , Int32
                    var brokerIdBytes = new byte[4];
                    reader.Read(brokerIdBytes, 0, 4);
                    var brokerId = KafkaProtocolPrimitiveType.GetInt32(brokerIdBytes);

                    // host , string
                    var hostSizeBytes = new byte[2];
                    reader.Read(hostSizeBytes, 0, 2);
                    var hostSize = KafkaProtocolPrimitiveType.GetInt16(hostSizeBytes);
                    var hostBytes = new byte[hostSize];
                    reader.Read(hostBytes, 0, hostSize);
                    var hostName = Encoding.UTF8.GetString(hostBytes, 0, hostSize);

                    // port, 4bytes, int32
                    var portBytes = new byte[4];
                    reader.Read(portBytes, 0, 4);
                    var portNum = KafkaProtocolPrimitiveType.GetInt32(portBytes);

                    this.BrokerInfos[i] = new KafkaProtocolTopicMetadataResponseBrokerInfo()
                    {
                        Host = hostName,
                        NodeId = brokerId,
                        Port = portNum
                    };
                }

                // [topicmetadata]
                var topicMetadataSizeBytes = new byte[4];
                reader.Read(topicMetadataSizeBytes, 0, 4);
                var numOfTopicMetadatas = KafkaProtocolPrimitiveType.GetInt32(topicMetadataSizeBytes);
                this.TopicInfos = new KafkaProtocolTopicMetadataResponseTopicInfo[numOfTopicMetadatas];
                for (int i = 0; i < numOfTopicMetadatas; i++)
                {
                    this.TopicInfos[i] = new KafkaProtocolTopicMetadataResponseTopicInfo();

                    // errorCode, 2bytes, int16
                    var topicErrorCodeBytes = new byte[2];
                    reader.Read(topicErrorCodeBytes, 0, 2);
                    var errorCode = KafkaProtocolPrimitiveType.GetInt16(topicErrorCodeBytes);
                    this.TopicInfos[i].TopicErrorCode = errorCode;

                    // topicname, string
                    var topicNameSizeBytes = new byte[2];
                    reader.Read(topicNameSizeBytes, 0, 2);
                    var topicNameSize = KafkaProtocolPrimitiveType.GetInt16(topicNameSizeBytes);
                    var topicNameBytes = new byte[topicNameSize];
                    reader.Read(topicNameBytes, 0, topicNameSize);
                    var topicName = Encoding.UTF8.GetString(topicNameBytes);
                    this.TopicInfos[i].TopicName = topicName;

                    // [partitionmetadata]
                    var partitionMetadataSizeBytes = new byte[4];
                    reader.Read(partitionMetadataSizeBytes, 0, 4);
                    var numOfPartitionMetadatas = KafkaProtocolPrimitiveType.GetInt32(partitionMetadataSizeBytes);
                    this.TopicInfos[i].PartitionInfos = new KafkaProtocolTopicMetadataResponsePartitionInfo[numOfPartitionMetadatas];
                    for (int j = 0; j < numOfPartitionMetadatas; j++)
                    {
                        this.TopicInfos[i].PartitionInfos[j] = new KafkaProtocolTopicMetadataResponsePartitionInfo();

                        // errorCode
                        var partitionErrorCodeBytes = new byte[2];
                        reader.Read(partitionErrorCodeBytes, 0, 2);
                        var partitionErrorCode = KafkaProtocolPrimitiveType.GetInt16(partitionErrorCodeBytes);
                        this.TopicInfos[i].PartitionInfos[j].PartitionErrorCode = partitionErrorCode;

                        // partitionId
                        var partitionIdBytes = new byte[4];
                        reader.Read(partitionIdBytes, 0, 4);
                        var partitionId = KafkaProtocolPrimitiveType.GetInt32(partitionIdBytes);
                        this.TopicInfos[i].PartitionInfos[j].PartitionId = partitionId;

                        // leader
                        var leaderBytes = new byte[4];
                        reader.Read(leaderBytes, 0, 4);
                        var leaderId = KafkaProtocolPrimitiveType.GetInt32(leaderBytes);
                        this.TopicInfos[i].PartitionInfos[j].Leader = leaderId;

                        // [replica]
                        var replicaSizeBytes = new byte[4];
                        reader.Read(replicaSizeBytes, 0, 4);
                        var numOfReplicas = KafkaProtocolPrimitiveType.GetInt32(replicaSizeBytes);
                        this.TopicInfos[i].PartitionInfos[j].Replicas = new int[numOfReplicas];
                        for (int k = 0; k < numOfReplicas; k++)
                        {
                            var replicaIdBytes = new byte[4];
                            reader.Read(replicaIdBytes, 0, 4);
                            var replicaId = KafkaProtocolPrimitiveType.GetInt32(replicaIdBytes);
                            this.TopicInfos[i].PartitionInfos[j].Replicas[k] = replicaId;
                        }

                        // [Isr]
                        var isrSizeBytes = new byte[4];
                        reader.Read(isrSizeBytes, 0, 4);
                        var numOfIsrs = KafkaProtocolPrimitiveType.GetInt32(isrSizeBytes);
                        this.TopicInfos[i].PartitionInfos[j].Isr = new int[numOfIsrs];
                        for (int k = 0; k < numOfIsrs; k++)
                        {
                            var isrIdBytes = new byte[4];
                            reader.Read(isrIdBytes, 0, 4);
                            var isrId = KafkaProtocolPrimitiveType.GetInt32(isrIdBytes);
                            this.TopicInfos[i].PartitionInfos[j].Isr[k] = isrId;
                        }
                    }
                }
            }
        }
    }
}