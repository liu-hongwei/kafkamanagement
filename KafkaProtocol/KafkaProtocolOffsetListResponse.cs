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
            // OffsetResponse => [TopicName[PartitionOffsets]]
            //  PartitionOffsets => Partition ErrorCode[Offset]
            //  Partition => int32
            //  ErrorCode => int16
            //  Offset => int64
            //---------------------------------------------------\\

            // TODO : deserialize the bytes into response
        }
    }
}