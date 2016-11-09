// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaProtocolProduceResponsePartitionInfoV2.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management.KafkaProtocol
{
    /// <summary>
    /// The version 2 of partition info in the response to produce request.
    /// supported in kafka 0.10.0 or later.
    /// </summary>
    public class KafkaProtocolProduceResponsePartitionInfoV2 : KafkaProtocolProduceResponsePartitionInfo
    {
        /// <summary>
        /// Gets or sets the timestamp in milliseconds.
        /// int64
        /// If LogAppendTime is used for the topic, this is the timestamp assigned by the broker to the message set. All the messages in the message set have the same timestamp.
        /// If CreateTime is used, this field is always -1. The producer can assume the timestamp of the messages in the produce request has been accepted by the 
        /// broker if there is no error code returned.
        /// Unit is milliseconds since beginning of the epoch (midnight Jan 1, 1970 (UTC)).
        /// </summary>
        public int Timestamp { get; set; }
    }
}