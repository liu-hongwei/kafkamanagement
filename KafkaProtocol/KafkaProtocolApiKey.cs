// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolApiKey.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The key to identify the kafka api.
    /// </summary>
    public enum KafkaProtocolApiKey
    {
        /*
            The Kafka protocol is fairly simple, there are only six core client requests APIs.
            1 Metadata - Describes the currently available brokers, their host and port information, and gives information about which broker hosts which partitions.
            2 Send - Send messages to a broker
            3 Fetch - Fetch messages from a broker, one which fetches data, one which gets cluster metadata, and one which gets offset information about a topic.
            4 Offsets - Get information about the available offsets for a given topic partition.
            5 Offset Commit - Commit a set of offsets for a consumer group
            6 Offset Fetch - Fetch a set of offsets for a consumer group
        
            Each of these will be described in detail below. Additionally, as of 0.9, Kafka supports general group management for consumers and Kafka Connect. 
            The client API consists of five requests:
            7 GroupCoordinator - Locate the current coordinator of a group.
            8 JoinGroup - Become a member of a group, creating it if there are no active members.
            9 SyncGroup - Synchronize state for all members of a group (e.g. distribute partition assignments to consumers).
            10 Heartbeat - Keep a member alive in the group. 
            11 LeaveGroup - Directly depart a group.
            
            Finally, there are several administrative APIs which can be used to monitor/administer the Kafka cluster (this list will grow when KIP-4 is completed).
            12 DescribeGroups - Used to inspect the current state of a set of groups (e.g. to view consumer partition assignments).  
            13 ListGroups - List the current groups managed by a broker.
        */

        /// <summary>
        /// API to request to produce data.
        /// </summary>
        ProduceRequest = 0,

        /// <summary>
        /// API to request fetch data .
        /// </summary>
        FetchRequest = 1,

        /// <summary>
        /// API to request to list offset.
        /// </summary>
        OffsetRequest = 2,

        /// <summary>
        /// API to request topic info .
        /// </summary>
        MetadataRequest = 3,

        /// <summary>
        /// API to request leader and isr (internal use only).
        /// </summary>
        LeaderAndIsrRequest = 4,

        /// <summary>
        /// API to request to stop replica (internal use only).
        /// </summary>
        StopReplicaRequest = 5,

        /// <summary>
        /// API to request update metadata (internal use only).
        /// </summary>
        UpdateMetadataRequest = 6,

        /// <summary>
        /// API to request to shut down (internal use only).
        /// </summary>
        ControlledShutdownRequest = 7,

        /// <summary>
        /// API to request to commit offset.
        /// </summary>
        OffsetCommitRequest = 8,

        /// <summary>
        /// API to request to fetch offset.
        /// </summary>
        OffsetFetchRequest = 9,

        /// <summary>
        /// API to request to discover group coordinator.
        /// </summary>
        GroupCoordinatorRequest = 10,

        /// <summary>
        /// API to request to join consumer group.
        /// </summary>
        JoinGroupRequest = 11,

        /// <summary>
        /// API to request to send heart beat.
        /// </summary>
        HeartbeatRequest = 12,

        /// <summary>
        /// API to request to leave consumer group.
        /// </summary>
        LeaveGroupRequest = 13,

        /// <summary>
        /// API to request to sync between consumer group.
        /// </summary>
        SyncGroupRequest = 14,

        /// <summary>
        /// API to request to describe consumer group.
        /// </summary>
        DescribeGroupsRequest = 15,

        /// <summary>
        /// API to request to list consumer group.
        /// </summary>
        ListGroupsRequest = 16,

        /// <summary>
        /// API to request the SASL authentication handshake.
        /// </summary>
        SaslHandshakeRequest = 17,

        /// <summary>
        /// API to request the api version.
        /// </summary>
        ApiVersionsRequest = 18,

        /// <summary>
        /// API to request to create topic.
        /// </summary>
        CreateTopicsRequest = 19,

        /// <summary>
        /// API to request to delete topic.
        /// </summary>
        DeleteTopicsRequest = 20
    }
}