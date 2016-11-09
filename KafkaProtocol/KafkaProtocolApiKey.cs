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