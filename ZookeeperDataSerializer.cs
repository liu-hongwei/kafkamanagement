// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZookeeperDataSerializer.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The kafka data serializer.
    /// </summary>
    public class ZookeeperDataSerializer
    {
        /// <summary>
        /// Deserialize kafka topic infomation.
        /// </summary>
        /// <param name="topic">The topic name.</param>
        /// <param name="topicData">The topic infomation data.</param>
        /// <returns>The kafka topic info.</returns>
        public KafkaTopicInfo DeserializeKafkaTopicInfo(string topic, byte[] topicData)
        {
            if (string.IsNullOrWhiteSpace(topic) || topicData == null)
            {
                return null;
            }

            int version = 1;
            var partitions = new List<KafkaPartitionInfo>();

            var json = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(topicData)) as JToken;
            if (json != null)
            {
                var fields = json.Children().ToList();
                foreach (var field in fields)
                {
                    var property = field as JProperty;
                    if (property != null)
                    {
                        if (property.Name == "version")
                        {
                            version = int.Parse(property.Value.ToString(), CultureInfo.InvariantCulture);
                        }

                        if (property.Name == "partitions")
                        {
                            if (property.Value != null && property.Value.HasValues)
                            {
                                var children = property.Value.Children();
                                foreach (var pp in children)
                                {
                                    var partitionproperty = pp as JProperty;
                                    if (partitionproperty != null)
                                    {
                                        var partitionId = partitionproperty.Name;
                                        var replicaIds = partitionproperty.Value.ToObject<int[]>();
                                        partitions.Add(new KafkaPartitionInfo()
                                        {
                                            PartitionId = partitionId,
                                            ReplicaOwners = replicaIds
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var topicInfo = new KafkaTopicInfo() { Name = topic, Version = version, Partitions = partitions.ToArray() };
            return topicInfo;
        }

        /// <summary>
        /// Serialize the kafka topic info into bytes.
        /// </summary>
        /// <param name="topicInfo">The kafka topic info.</param>
        /// <returns>json bytes of the topic info used in zookeeper node.</returns>
        public byte[] SerializeKafkaTopicInfo(KafkaTopicInfo topicInfo)
        {
            if (topicInfo == null)
            {
                return null;
            }

            var json = new JObject();
            json.Add("version", 1);
            var partitionsObj = new JObject();
            json.Add("partitions", partitionsObj);

            // if partitionId is the same, then combine replicas
            var partitionGroups = topicInfo.Partitions.GroupBy(p => p.PartitionId);
            foreach (var pg in partitionGroups)
            {
                var id = pg.Key;
                var partitions = new List<int>();
                foreach (var p in pg)
                {
                    partitions.AddRange(p.ReplicaOwners);
                }

                var partitionList = new List<JToken>();
                var jtoken = new JProperty(id, partitions);
                partitionList.Add(jtoken);
                partitionsObj.Add(partitionList);
            }

            var content = JsonConvert.SerializeObject(json);

            return Encoding.UTF8.GetBytes(content);
        }

        /// <summary>
        /// Deserialize the content of broker node in zookeeper.
        /// </summary>
        /// <param name="id">the id of the broker in zookeeper.</param>
        /// <param name="brokerContent">the content of broker node.</param>
        /// <returns>The broker info.</returns>
        public KafkaBrokerInfo DeserializeKafkaBrokerInfo(string id, byte[] brokerContent)
        {
            var brokerInfo = JsonConvert.DeserializeObject<KafkaBrokerInfo>(Encoding.UTF8.GetString(brokerContent));
            brokerInfo.Id = int.Parse(id, CultureInfo.InvariantCulture);
            return brokerInfo;
        }

        /// <summary>
        /// Deserialize the content of partition state in zookeeper.
        /// </summary>
        /// <param name="stateData">The content of partition state.</param>
        /// <returns>The partition state.</returns>
        public KafkaPartitionState DeserializeKafkaPartitionState(byte[] stateData)
        {
            return JsonConvert.DeserializeObject<KafkaPartitionState>(Encoding.UTF8.GetString(stateData));
        }
    }
}
