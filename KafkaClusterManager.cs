// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaClusterManager.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// The kafka cluster manager.
    /// </summary>
    public class KafkaClusterManager : IKafkaClusterManager
    {
        /// <summary>
        /// Gets or sets the zookeeper client for cluster management.
        /// Please refer to the Kafka document here https://kafka.apache.org/documentation for how to operate kafka cluster.
        /// the internal data structure in zookeeper used by kafka cluster can be found here https://cwiki.apache.org/confluence/display/KAFKA/Kafka+data+structures+in+Zookeeper
        /// this version of implementation is based on kafka 0.10.0. most of the the idea comes from the kafk source code https://github.com/apache/kafka.
        /// it does not implement all the kafka administration features, it only implements the commonly used functions we needed to create new topic, monitoring leader changes.
        /// if you need the complete features, you can enhance or create new APIs here.
        /// </summary>
        public IZookeeperClient ZookeeperClient { get; set; }

        /// <summary>
        /// Get all active kafka broker information in cluster.
        /// </summary>
        /// <returns>Task holds the broker information.</returns>
        public async Task<IEnumerable<KafkaBrokerInfo>> ListKakfaBrokersAsync()
        {
            List<KafkaBrokerInfo> brokers = new List<KafkaBrokerInfo>();

            var idNodes = await this.ZookeeperClient.GetChildrenAsync("/brokers/ids", false).ConfigureAwait(false);

            if (idNodes != null && idNodes.Any())
            {
                foreach (var child in idNodes)
                {
                    var idData = await this.ZookeeperClient.GetDataAsync("/brokers/ids/" + child, false).ConfigureAwait(false);
                    if (idData != null && idData.Any())
                    {
                        var brokerInfo = JsonConvert.DeserializeObject<KafkaBrokerInfo>(Encoding.UTF8.GetString(idData));
                        brokerInfo.Id = int.Parse(child, CultureInfo.InvariantCulture);
                        brokers.Add(brokerInfo);
                    }
                }
            }

            return brokers.AsEnumerable();

        }

        /// <summary>
        /// List all topics in the cluster.
        /// </summary>
        /// <returns>all topic information in cluster.</returns>
        public async Task<IEnumerable<KafkaTopicInfo>> ListKafkaTopicsAsync()
        {
            List<KafkaTopicInfo> topics = new List<KafkaTopicInfo>();

            var topicNodes = await this.ZookeeperClient.GetChildrenAsync("/brokers/topics", false).ConfigureAwait(false);
            if (topicNodes != null && topicNodes.Any())
            {
                foreach (var tp in topicNodes)
                {
                    var topicInfo = await this.GetTopicInfoAsync(tp).ConfigureAwait(false);
                    if (topicInfo != null)
                    {
                        topics.Add(topicInfo);
                    }
                }
            }

            return topics.AsEnumerable();
        }

        /// <summary>
        /// Fetch the topic information from zookeeper.
        /// </summary>
        /// <param name="topic">the name of the topic.</param>
        /// <returns>The whole topic infomration.</returns>
        public async Task<KafkaTopicInfo> GetKafkaTopicAsync(string topic)
        {
            if (string.IsNullOrWhiteSpace(topic))
            {
                throw new ArgumentException("topic name is null or empty.");
            }

            return await this.GetTopicInfoAsync(topic).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a topic with creation settings.
        /// </summary>
        /// <param name="topicConfiguration">The topic creation configuration.</param>
        /// <returns>the path to the topic. null is return when unable to create it.</returns>
        public async Task<string> CreateKafkaTopicAsync(KafkaTopicConfiguration topicConfiguration)
        {
            if (topicConfiguration == null || !this.IsTopicNameValid(topicConfiguration.TopicName) || topicConfiguration.NumOfPartitions <= 0 || topicConfiguration.NumOfReplicas <= 0)
            {
                return null;
            }

            //// refer to https://github.com/apache/kafka/blob/trunk/core/src/main/scala/kafka/admin/AdminUtils.scala 
            //// the method createTopic

            // 1 figure how many brokers are available
            var brokers = await this.ListKakfaBrokersAsync().ConfigureAwait(false);
            if (brokers == null)
            {
                return null;
            }

            // note:
            // when actual number of active brokers is less than the expected number of brokers, technically you still can
            // create topic. for example: we expected 5 brokers, but actually there are only 3, when you create below topic:
            //  { "name" :"test", "partitions": "3", "replicas":"3"}
            //  as far as the number of replicas is not bigger than the number of active brokers, you can still assign replicas
            //  onto different brokers. 
            // the only problem with this scenario is the partitions will not put on the another brokers when they are alive.
            // then need manually move some partitions to the addition brokers to get better balance.
            // so to make things simple, we just don't do it when this happen.
            var expected = this.GetExpectedKafkaBrokers();
            if (expected == null || brokers.Count() != expected.Count())
            {
                return null;
            }

            var topics = await this.ListKafkaTopicsAsync().ConfigureAwait(false);
            if (topics != null)
            {
                var existing = topics.FirstOrDefault(t => t.Name == topicConfiguration.TopicName);
                if (existing != null)
                {
                    return "/brokers/topics/" + topicConfiguration.TopicName;
                }
            }

            // Note : number of replicas can not be greater than number of active brokers, 
            // because replicas of same topic can not be on the same broker

            // 2 get partitions assignment info onto those brokers with round-robin fashion.
            var assignments = this.AssignPartitionsAndReplicasToBrokers(topicConfiguration.NumOfPartitions, topicConfiguration.NumOfReplicas, brokers);

            // 3 convert partition assignment into json format for zookeeper node, and create zookeepr node
            // the format of json is : {"version":1,"partitions":{"0":[],...}}
            var serializer = new ZookeeperDataSerializer();
            var content = serializer.SerializeKafkaTopicInfo(new KafkaTopicInfo() { Name = topicConfiguration.TopicName, Version = 1, Partitions = assignments.ToArray() });

            // 4 create zookpper node
            // create node at path 'brokers/topics/{name}', with content above
            var acl = new ZookeeperZnodeAcl()
            {
                AclRights = ZookeeperZnodeAclRights.All,
                Identity = ZookeeperZnodeAclIdentity.WorldIdentity()
            };

            var path = await this.ZookeeperClient.CreateAsync("/brokers/topics/" + topicConfiguration.TopicName, content, new List<ZookeeperZnodeAcl>() { acl }, ZookeeperZnodeType.Persistent).ConfigureAwait(false);

            return path;
        }

        /// <summary>
        /// Delete a topic by topic name.
        /// </summary>
        /// <param name="topic">the name of the topic.</param>
        /// <returns>Task holds status of deletion. True indicates succeeded. False indicates failed.</returns>
        public async Task<bool> DeleteKafkaTopicAsync(string topic)
        {
            var existing = await this.GetKafkaTopicAsync(topic).ConfigureAwait(false);
            if (existing == null)
            {
                return false;
            }

            // note
            // when delete a topic from kafka, it actually create a empty node under /admin/delete_topics/{topic}
            // not deleting the node under /brokers/topics/{topic}, let the kafka controller to handle the real
            // topic removing.

            var acl = new ZookeeperZnodeAcl()
            {
                AclRights = ZookeeperZnodeAclRights.All,
                Identity = ZookeeperZnodeAclIdentity.WorldIdentity()
            };

            var path = await this.ZookeeperClient.CreateAsync("/admin/delete_topics/" + topic, Encoding.UTF8.GetBytes(string.Empty), new List<ZookeeperZnodeAcl>() { acl }, ZookeeperZnodeType.Persistent).ConfigureAwait(false);

            return !string.IsNullOrWhiteSpace(path);
        }

        /// <summary>
        /// Update partitions and others settings for a topic.
        /// </summary>
        /// <param name="topicConfiguration">The topic update information.</param>
        /// <returns>Task holds the topic info if update complete. null is return when topic does not exist or unable to update it.</returns>
        /// <remarks>this method is used to increase the number of partitions for a topic. the increasement of replica is not handled here.</remarks>
        public async Task<KafkaTopicInfo> UpdateKafkaTopicAsync(KafkaTopicConfiguration topicConfiguration)
        {
            if (topicConfiguration == null)
            {
                throw new ArgumentException("topicConfiguration is null or empty.");
            }

            // check arguments
            if (topicConfiguration == null || string.IsNullOrWhiteSpace(topicConfiguration.TopicName) ||
                topicConfiguration.NumOfPartitions <= 0 || topicConfiguration.NumOfReplicas <= 0 ||
                topicConfiguration.TopicName == "__consumer_offsets")
            {
                return null;
            }

            // 1 get topic info from zookeeper
            var topicInfo = await this.GetKafkaTopicAsync(topicConfiguration.TopicName).ConfigureAwait(false);
            if (topicInfo == null)
            {
                return null;
            }

            // 2 check partitions and replicas
            // only support add more partitions or replicas, not support reduce them
            int numOfExistingPartitions = 0;
            int numOfExistingReplicas = 0;
            var partitions = topicInfo.Partitions;
            KafkaPartitionInfo partitionInfo = null;
            if (partitions != null)
            {
                numOfExistingPartitions = partitions.Length;

                partitionInfo = partitions.FirstOrDefault(p => p.PartitionId == "0");
                if (partitionInfo != null)
                {
                    var replicas = partitionInfo.ReplicaOwners;
                    if (replicas != null)
                    {
                        numOfExistingReplicas = replicas.Length;
                    }
                }
            }

            if (numOfExistingPartitions == 0 || numOfExistingReplicas == 0)
            {
                return null;
            }

            if (numOfExistingPartitions >= topicConfiguration.NumOfPartitions)
            {
                return topicInfo;
            }

            // NOTE : if want to update replicas, need to use another call
            if (numOfExistingReplicas != topicConfiguration.NumOfReplicas)
            {
                return topicInfo;
            }


            // 3 update zookeeper node content
            var partitionsToAdd = topicConfiguration.NumOfPartitions - numOfExistingPartitions;

            // assign the brokers to brokers
            var brokers = await this.ListKakfaBrokersAsync().ConfigureAwait(false);
            var newPartitionAssignmentInfo = this.AssignPartitionsAndReplicasToBrokers(partitionsToAdd, numOfExistingReplicas, brokers, Math.Max(0, partitionInfo.ReplicaOwners[0]), numOfExistingPartitions);

            // check the newPartitios to see whether some partitions get more replicats
            if (newPartitionAssignmentInfo.Any(p => p.ReplicaOwners.Length != partitionInfo.ReplicaOwners.Length))
            {
                return null;
            }

            // merge existing and new partition info
            var finalAssignmentInfo = new List<KafkaPartitionInfo>();
            finalAssignmentInfo.AddRange(topicInfo.Partitions);
            finalAssignmentInfo.AddRange(newPartitionAssignmentInfo);
            topicInfo.Partitions = finalAssignmentInfo.ToArray();

            // 4 update the node in zookeeper
            // update node at path 'brokers/topics/{name}', with new content
            var serializer = new ZookeeperDataSerializer();
            var content = serializer.SerializeKafkaTopicInfo(topicInfo);

            await this.ZookeeperClient.SetDataAsync("/brokers/topics/" + topicConfiguration.TopicName, content, -1).ConfigureAwait(false);

            return topicInfo;
        }

        /// <summary>
        /// Get the state of a kafka partition.
        /// </summary>
        /// <param name="topic">The name of topic.</param>
        /// <param name="partitionId">The id of partition.</param>
        /// <returns>Task holds the partition state.</returns>
        public async Task<KafkaPartitionState> GetKafkaPartitionStateAsync(string topic, string partitionId)
        {
            int id;
            if (string.IsNullOrWhiteSpace(topic) || int.TryParse(partitionId, out id) == false)
            {
                return null;
            }

            var bytes = await this.ZookeeperClient.GetDataAsync("/brokers/topics/" + topic + "/partitions/" + partitionId + "/state", false).ConfigureAwait(false);
            if (bytes == null || !bytes.Any())
            {
                return null;
            }

            var serializer = new ZookeeperDataSerializer();
            var partitionState = serializer.DeserializeKafkaPartitionState(bytes);
            return partitionState;
        }

        /// <summary>
        /// Disposing unused resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing unused resources.
        /// </summary>
        /// <param name="disposing">Whether need to dispose the resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.ZookeeperClient != null)
                {
                    this.ZookeeperClient.Dispose();
                }

                this.ZookeeperClient = null;
            }
        }
        
        /// <summary>
        /// Check whether the topic name is valid.
        /// </summary>
        /// <param name="topic">The name of the topic.</param>
        /// <returns>True indicates the name is valid; False indicates the name is invalid.</returns>
        private bool IsTopicNameValid(string topic)
        {
            if (string.IsNullOrWhiteSpace(topic))
            {
                return false;
            }

            // only allow A-Z, a-z, 0-9, -
            var regex = new Regex("^[A-Za-z0-9\\-]+$");
            return regex.IsMatch(topic);
        }

        /// <summary>
        /// Get the kafka topic info.
        /// </summary>
        /// <param name="topic">The topic name.</param>
        /// <returns>The topic infomation.</returns>
        private async Task<KafkaTopicInfo> GetTopicInfoAsync(string topic)
        {
            var path = "/brokers/topics/" + topic;
            var data = await this.ZookeeperClient.GetDataAsync(path, false).ConfigureAwait(false);
            if (data == null || data.Length == 0)
            {
                return null;
            }

            var serializer = new ZookeeperDataSerializer();
            return serializer.DeserializeKafkaTopicInfo(topic, data);
        }

        /// <summary>
        /// Assign partition and replicas to kafka brokers.
        /// the basic idea here is to evenly assign the active brokers to a partition, and assign its replicas to other active brokers.
        /// </summary>
        /// <param name="numOfPartitions">the number of partitions.</param>
        /// <param name="numOfReplicas">the number of replications.</param>
        /// <param name="brokerInfos">the active kafka brokers.</param>
        /// <param name="startBrokerId">the perfered broker id to start for assignment.</param>
        /// <param name="startPartitionId">the perfered partition id to start for assignment.</param>
        /// <returns>The kafka partition assignment info.</returns>
        public IEnumerable<KafkaPartitionInfo> AssignPartitionsAndReplicasToBrokers(int numOfPartitions, int numOfReplicas, IEnumerable<KafkaBrokerInfo> brokerInfos, int startBrokerId = -1, int startPartitionId = -1)
        {

            if (numOfPartitions <= 0)
            {
                return null;
            }

            if (numOfReplicas <= 0)
            {
                return null;
            }

            if (brokerInfos == null)
            {
                return null;
            }

            var brokersArray = brokerInfos.ToArray();
            int numOfBrokers = brokersArray.Count();
            if (numOfBrokers == 0)
            {
                return null;
            }

            if (numOfReplicas > numOfBrokers)
            {
                return null;
            }

            var result = new List<KafkaPartitionInfo>();

            Random random = new Random();

            // if caller did not specific a broker for the starting partition 0, then random choose a broker for the partition 0
            var brokerIds = brokersArray.Select(e => e.Id);
            var minId = brokerIds.Min();
            var maxId = brokerIds.Max();
            if (startBrokerId > maxId)
            {
                startBrokerId = minId;
            }

            if (startPartitionId > numOfPartitions)
            {
                startPartitionId = 0;
            }

            var startIndex = startBrokerId >= 0 ? startBrokerId : random.Next(numOfBrokers);

            // partition always start from 0, unless caller specific a number to start with.
            var currentPartitionId = Math.Max(0, startPartitionId);
            var nextReplicaShift = startBrokerId >= 0 ? startBrokerId : random.Next(numOfBrokers);
            
            // for each partition , find a broker id for this partition, and also find broker ids for all replicas
            for (int i = 0; i < numOfPartitions; i++)
            {
                if (currentPartitionId > 0 && (currentPartitionId % numOfBrokers) == 0)
                {
                    nextReplicaShift += 1;
                }

                var replicaIndex = (currentPartitionId + startIndex) % numOfBrokers;

                var assignmentInfo = new KafkaPartitionInfo();

                assignmentInfo.PartitionId = currentPartitionId.ToString(CultureInfo.InvariantCulture);
                assignmentInfo.ReplicaOwners = new int[numOfReplicas];

                // assign the broker id to first replica.
                assignmentInfo.ReplicaOwners[0] = brokersArray[replicaIndex].Id;

                // find borker ids for other replicas
                for (int j = 1; j < numOfReplicas; j++)
                {
                    var factor = j % numOfBrokers;
                    replicaIndex = (replicaIndex + factor) % numOfBrokers;

                    assignmentInfo.ReplicaOwners[j] = brokersArray[replicaIndex].Id;
                }

                result.Add(assignmentInfo);

                currentPartitionId += 1;
            }

            return result;
        }

        /// <summary>
        /// Get the kafka consumer group ids.
        /// </summary>
        /// <returns>Task holds the consumer group ids.</returns>
        public async Task<IEnumerable<string>> ListKafkaConsumerGroupsAsync()
        {
            var path = "/consumers";
            var groups = await this.ZookeeperClient.GetChildrenAsync(path).ConfigureAwait(false);
            return groups;
        }

        /// <summary>
        /// Get the kafka consumer ids in given group.
        /// </summary>
        /// <param name="groupId">The id of the consumer group.</param>
        /// <returns>Task holds the consumer ids in the given group.</returns>
        public async Task<IEnumerable<string>> ListKafkaConsumerIdsAsync(string groupId)
        {
            if (string.IsNullOrWhiteSpace(groupId))
            {
                return null;
            }

            var path = "/consumers/" + groupId + "/ids";
            var consumerIds = await this.ZookeeperClient.GetChildrenAsync(path).ConfigureAwait(false);
            return consumerIds;
        }

        /// <summary>
        /// Get the kafka consumer group owned topics.
        /// </summary>
        /// <returns>Task holds the consumer group onwed topics.</returns>
        public async Task<IEnumerable<string>> ListKafkaConsumerOwnedTopicsAsync(string groupId)
        {
            if (string.IsNullOrWhiteSpace(groupId))
            {
                return null;
            }

            var path = "/consumers/" + groupId + "/owners";
            var topics = await this.ZookeeperClient.GetChildrenAsync(path).ConfigureAwait(false);
            return topics;
        }

        /// <summary>
        /// Get the kafka consumer owned partitions.
        /// </summary>
        /// <returns>Task holds the consumer owned partitions.</returns>
        public async Task<IEnumerable<string>> ListKafkaConsumerOwnedPartitionsAsync(string groupId, string topic)
        {
            if (string.IsNullOrWhiteSpace(groupId) || string.IsNullOrWhiteSpace(topic))
            {
                return null;
            }

            var path = "/consumers/" + groupId + "/owners/" + topic;
            var partitions = await this.ZookeeperClient.GetChildrenAsync(path).ConfigureAwait(false);
            return partitions;
        }

        /// <summary>
        /// Get the kafka consumer owner for specific partition.
        /// </summary>
        /// <returns>Task holds the consumer owner for specific partition.</returns>
        public async Task<string> ListKafkaConsumerOwnerAsync(string groupId, string topic, string partition)
        {
            if (string.IsNullOrWhiteSpace(groupId) || string.IsNullOrWhiteSpace(topic) || string.IsNullOrWhiteSpace(partition))
            {
                return null;
            }

            var path = "/consumers/" + groupId + "/owners/" + topic + "/" + partition;
            var content = await this.ZookeeperClient.GetDataAsync(path).ConfigureAwait(false);
            var owner = Encoding.UTF8.GetString(content);
            return owner;
        }

        /// <summary>
        /// Get the kafka consumer offset.
        /// </summary>
        /// <returns>Task holds the consumer offset.</returns>
        public async Task<long> ListKafkaConsumerOffsetAsync(string groupId, string topic, string partition)
        {
            if (string.IsNullOrWhiteSpace(groupId) || string.IsNullOrWhiteSpace(topic) || string.IsNullOrWhiteSpace(partition))
            {
                return 0;
            }

            var path = "/consumers/" + groupId + "/offsets/" + topic + "/" + partition;
            var content = await this.ZookeeperClient.GetDataAsync(path).ConfigureAwait(false);
            var txt = Encoding.UTF8.GetString(content);
            long offset;
            if (!long.TryParse(txt, out offset))
            {
                offset = 0;
            }

            return offset;
        }
        
        /// <summary>
        /// Get the configured kafka brokers from RCS.
        /// </summary>
        /// <returns>The list for brokers configured in RCS.</returns>
        private IEnumerable<KafkaBrokerInfo> GetExpectedKafkaBrokers()
        {
            var brokers = new List<KafkaBrokerInfo>();

            // read from RCS to get the pre-configured kafka brokers
            var configMgr = ComponentFactory.Current.Resolve<IConfigurationManager>();
            var brokersCfg = configMgr.GetSetting("KafkaClusterServers", string.Empty);
            var brokerPort = configMgr.GetSetting("KafkaClusterDefaultPort", 29001);

            if (string.IsNullOrWhiteSpace(brokersCfg))
            {
                return null;
            }

            var brokersPairs = brokersCfg.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (brokersPairs == null || !brokersPairs.Any())
            {
                return null;
            }

            foreach (var pairs in brokersPairs)
            {
                string host = string.Empty;
                int port = brokerPort;

                var parts = pairs.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts != null && parts.Any())
                {
                    // it can be DNS name or ip
                    try
                    {
                        var ips = Dns.GetHostAddresses(parts[0]);
                        if (ips != null)
                        {
                            var ipv4 = ips.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
                            if (ipv4 != null)
                            {
                                host = ipv4.ToString();
                            }
                        }
                    }
                    catch (SocketException)
                    {
                        // if unable to get ip, then just ignore it
                    }

                    if (parts.Length < 2 || !int.TryParse(parts[1], out port))
                    {
                        port = brokerPort;
                    }
                }

                if (!string.IsNullOrWhiteSpace(host))
                {
                    brokers.Add(new KafkaBrokerInfo() { Host = host, Port = port });
                }
            }

            return brokers;
        }
    }
}
