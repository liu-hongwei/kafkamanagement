October, 2016 by Hongwei Liu, hongwei_liu@outlook.com

this readme document contains the text version of the kafka protocl, which is copied from https://kafka.apache.org/protocol.html
The guide of kafka protocol can be found here https://cwiki.apache.org/confluence/display/KAFKA/A+Guide+To+The+Kafka+Protocol

Kafka protocol guide

This document covers the wire protocol implemented in Kafka. It is meant to give a readable guide to the protocol that covers the available requests, their binary format, and the proper way to make use of them to implement a client. This document assumes you understand the basic design and terminology described here

Preliminaries
Network
Partitioning and bootstrapping
Partitioning Strategies
Batching
Versioning and Compatibility
The Protocol
Protocol Primitive Types
Notes on reading the request format grammars
Common Request and Response Structure
Message Sets
Constants
Error Codes
Api Keys
The Messages
Some Common Philosophical Questions
Preliminaries

Network

Kafka uses a binary protocol over TCP. The protocol defines all apis as request response message pairs. All messages are size delimited and are made up of the following primitive types.

The client initiates a socket connection and then writes a sequence of request messages and reads back the corresponding response message. No handshake is required on connection or disconnection. TCP is happier if you maintain persistent connections used for many requests to amortize the cost of the TCP handshake, but beyond this penalty connecting is pretty cheap.

The client will likely need to maintain a connection to multiple brokers, as data is partitioned and the clients will need to talk to the server that has their data. However it should not generally be necessary to maintain multiple connections to a single broker from a single client instance (i.e. connection pooling).

The server guarantees that on a single TCP connection, requests will be processed in the order they are sent and responses will return in that order as well. The broker's request processing allows only a single in-flight request per connection in order to guarantee this ordering. Note that clients can (and ideally should) use non-blocking IO to implement request pipelining and achieve higher throughput. i.e., clients can send requests even while awaiting responses for preceding requests since the outstanding requests will be buffered in the underlying OS socket buffer. All requests are initiated by the client, and result in a corresponding response message from the server except where noted.

The server has a configurable maximum limit on request size and any request that exceeds this limit will result in the socket being disconnected.

Partitioning and bootstrapping

Kafka is a partitioned system so not all servers have the complete data set. Instead recall that topics are split into a pre-defined number of partitions, P, and each partition is replicated with some replication factor, N. Topic partitions themselves are just ordered "commit logs" numbered 0, 1, ..., P.

All systems of this nature have the question of how a particular piece of data is assigned to a particular partition. Kafka clients directly control this assignment, the brokers themselves enforce no particular semantics of which messages should be published to a particular partition. Rather, to publish messages the client directly addresses messages to a particular partition, and when fetching messages, fetches from a particular partition. If two clients want to use the same partitioning scheme they must use the same method to compute the mapping of key to partition.

These requests to publish or fetch data must be sent to the broker that is currently acting as the leader for a given partition. This condition is enforced by the broker, so a request for a particular partition to the wrong broker will result in an the NotLeaderForPartition error code (described below).

How can the client find out which topics exist, what partitions they have, and which brokers currently host those partitions so that it can direct its requests to the right hosts? This information is dynamic, so you can't just configure each client with some static mapping file. Instead all Kafka brokers can answer a metadata request that describes the current state of the cluster: what topics there are, which partitions those topics have, which broker is the leader for those partitions, and the host and port information for these brokers.

In other words, the client needs to somehow find one broker and that broker will tell the client about all the other brokers that exist and what partitions they host. This first broker may itself go down so the best practice for a client implementation is to take a list of two or three urls to bootstrap from. The user can then choose to use a load balancer or just statically configure two or three of their kafka hosts in the clients.

The client does not need to keep polling to see if the cluster has changed; it can fetch metadata once when it is instantiated cache that metadata until it receives an error indicating that the metadata is out of date. This error can come in two forms: (1) a socket error indicating the client cannot communicate with a particular broker, (2) an error code in the response to a request indicating that this broker no longer hosts the partition for which data was requested.

Cycle through a list of "bootstrap" kafka urls until we find one we can connect to. Fetch cluster metadata.
Process fetch or produce requests, directing them to the appropriate broker based on the topic/partitions they send to or fetch from.
If we get an appropriate error, refresh the metadata and try again.
Partitioning Strategies

As mentioned above the assignment of messages to partitions is something the producing client controls. That said, how should this functionality be exposed to the end-user?

Partitioning really serves two purposes in Kafka:

It balances data and request load over brokers
It serves as a way to divvy up processing among consumer processes while allowing local state and preserving order within the partition. We call this semantic partitioning.
For a given use case you may care about only one of these or both.

To accomplish simple load balancing a simple approach would be for the client to just round robin requests over all brokers. Another alternative, in an environment where there are many more producers than brokers, would be to have each client chose a single partition at random and publish to that. This later strategy will result in far fewer TCP connections.

Semantic partitioning means using some key in the message to assign messages to partitions. For example if you were processing a click message stream you might want to partition the stream by the user id so that all data for a particular user would go to a single consumer. To accomplish this the client can take a key associated with the message and use some hash of this key to choose the partition to which to deliver the message.

Batching

Our apis encourage batching small things together for efficiency. We have found this is a very significant performance win. Both our API to send messages and our API to fetch messages always work with a sequence of messages not a single message to encourage this. A clever client can make use of this and support an "asynchronous" mode in which it batches together messages sent individually and sends them in larger clumps. We go even further with this and allow the batching across multiple topics and partitions, so a produce request may contain data to append to many partitions and a fetch request may pull data from many partitions all at once.

The client implementer can choose to ignore this and send everything one at a time if they like.

Versioning and Compatibility

The protocol is designed to enable incremental evolution in a backward compatible fashion. Our versioning is on a per-api basis, each version consisting of a request and response pair. Each request contains an API key that identifies the API being invoked and a version number that indicates the format of the request and the expected format of the response.

The intention is that clients would implement a particular version of the protocol, and indicate this version in their requests. Our goal is primarily to allow API evolution in an environment where downtime is not allowed and clients and servers cannot all be changed at once.

The server will reject requests with a version it does not support, and will always respond to the client with exactly the protocol format it expects based on the version it included in its request. The intended upgrade path is that new features would first be rolled out on the server (with the older clients not making use of them) and then as newer clients are deployed these new features would gradually be taken advantage of.

Currently all versions are baselined at 0, as we evolve these APIs we will indicate the format for each version individually.

Retrieving Supported API versions

In order for a client to successfully talk to a broker, it must use request versions supported by the broker. Clients may work against multiple broker versions, however to do so the clients need to know what versions of various APIs a broker supports. Starting from 0.10.0.0, brokers provide information on various versions of APIs they support. Details of this new capability can be found here. Clients may use the supported API versions information to take appropriate actions such as propagating an unsupported API version error to application or choose an API request/response version supported by both the client and broker. The following sequence maybe used by a client to obtain supported API versions from a broker.

Client sends ApiVersionsRequest to a broker after connection has been established with the broker. If SSL is enabled, this happens after SSL connection has been established.
On receiving ApiVersionsRequest, a broker returns its full list of supported ApiKeys and versions regardless of current authentication state (e.g., before SASL authentication on an SASL listener, do note that no Kafka protocol requests may take place on a SSL listener before the SSL handshake is finished). If this is considered to leak information about the broker version a workaround is to use SSL with client authentication which is performed at an earlier stage of the connection where the ApiVersionRequest is not available. Also, note that broker versions older than 0.10.0.0 do not support this API and will either ignore the request or close connection in response to the request.
If multiple versions of an API are supported by broker and client, clients are recommended to use the latest version supported by the broker and itself.
Deprecation of a protocol version is done by marking an API version as deprecated in protocol documentation.
Supported API versions obtained from a broker, is valid only for current connection on which that information is obtained. In the event of disconnection, the client should obtain the information from broker again, as the broker might have upgraded/downgraded in the mean time.
SASL Authentication Sequence

The following sequence is used for SASL authentication:

Kafka ApiVersionsRequest may be sent by the client to obtain the version ranges of requests supported by the broker. This is optional.
Kafka SaslHandshakeRequest containing the SASL mechanism for authentication is sent by the client. If the requested mechanism is not enabled in the server, the server responds with the list of supported mechanisms and closes the client connection. If the mechanism is enabled in the server, the server sends a successful response and continues with SASL authentication.
The actual SASL authentication is now performed. A series of SASL client and server tokens corresponding to the mechanism are sent as opaque packets. These packets contain a 32-bit size followed by the token as defined by the protocol for the SASL mechanism.
If authentication succeeds, subsequent packets are handled as Kafka API requests. Otherwise, the client connection is closed.
For interoperability with 0.9.0.x clients, the first packet received by the server is handled as a SASL/GSSAPI client token if it is not a valid Kafka request. SASL/GSSAPI authentication is performed starting with this packet, skipping the first two steps above.

The Protocol

Protocol Primitive Types

The protocol is built out of the following primitive types.

Fixed Width Primitives

int8, int16, int32, int64 - Signed integers with the given precision (in bits) stored in big endian order.

Variable Length Primitives

bytes, string - These types consist of a signed integer giving a length N followed by N bytes of content. A length of -1 indicates null. string uses an int16 for its size, and bytes uses an int32.

Arrays

This is a notation for handling repeated structures. These will always be encoded as an int32 size containing the length N followed by N repetitions of the structure which can itself be made up of other primitive types. In the BNF grammars below we will show an array of a structure foo as [foo].

Notes on reading the request format grammars

The BNFs below give an exact context free grammar for the request and response binary format. The BNF is intentionally not compact in order to give human-readable name. As always in a BNF a sequence of productions indicates concatenation. When there are multiple possible productions these are separated with '|' and may be enclosed in parenthesis for grouping. The top-level definition is always given first and subsequent sub-parts are indented.

Common Request and Response Structure

All requests and responses originate from the following grammar which will be incrementally describe through the rest of this document:

RequestOrResponse => Size (RequestMessage | ResponseMessage)
Size => int32
Field	Description
message_size	The message_size field gives the size of the subsequent request or response message in bytes. The client can read requests by first reading this 4 byte size as an integer N, and then reading and parsing the subsequent N bytes of the request.
Message Sets

A description of the message set format can be found here. (KAFKA-3368)

Constants

Error Codes

We use numeric codes to indicate what problem occurred on the server. These can be translated by the client into exceptions or whatever the appropriate error handling mechanism in the client language. Here is a table of the error codes currently in use:

Error	Code	Retriable	Description
UNKNOWN	-1	False	The server experienced an unexpected error when processing the request
NONE	0	False	
OFFSET_OUT_OF_RANGE	1	False	The requested offset is not within the range of offsets maintained by the server.
CORRUPT_MESSAGE	2	True	This message has failed its CRC checksum, exceeds the valid size, or is otherwise corrupt.
UNKNOWN_TOPIC_OR_PARTITION	3	True	This server does not host this topic-partition.
INVALID_FETCH_SIZE	4	False	The requested fetch size is invalid.
LEADER_NOT_AVAILABLE	5	True	There is no leader for this topic-partition as we are in the middle of a leadership election.
NOT_LEADER_FOR_PARTITION	6	True	This server is not the leader for that topic-partition.
REQUEST_TIMED_OUT	7	True	The request timed out.
BROKER_NOT_AVAILABLE	8	False	The broker is not available.
REPLICA_NOT_AVAILABLE	9	False	The replica is not available for the requested topic-partition
MESSAGE_TOO_LARGE	10	False	The request included a message larger than the max message size the server will accept.
STALE_CONTROLLER_EPOCH	11	False	The controller moved to another broker.
OFFSET_METADATA_TOO_LARGE	12	False	The metadata field of the offset request was too large.
NETWORK_EXCEPTION	13	True	The server disconnected before a response was received.
GROUP_LOAD_IN_PROGRESS	14	True	The coordinator is loading and hence can't process requests for this group.
GROUP_COORDINATOR_NOT_AVAILABLE	15	True	The group coordinator is not available.
NOT_COORDINATOR_FOR_GROUP	16	True	This is not the correct coordinator for this group.
INVALID_TOPIC_EXCEPTION	17	False	The request attempted to perform an operation on an invalid topic.
RECORD_LIST_TOO_LARGE	18	False	The request included message batch larger than the configured segment size on the server.
NOT_ENOUGH_REPLICAS	19	True	Messages are rejected since there are fewer in-sync replicas than required.
NOT_ENOUGH_REPLICAS_AFTER_APPEND	20	True	Messages are written to the log, but to fewer in-sync replicas than required.
INVALID_REQUIRED_ACKS	21	False	Produce request specified an invalid value for required acks.
ILLEGAL_GENERATION	22	False	Specified group generation id is not valid.
INCONSISTENT_GROUP_PROTOCOL	23	False	The group member's supported protocols are incompatible with those of existing members.
INVALID_GROUP_ID	24	False	The configured groupId is invalid
UNKNOWN_MEMBER_ID	25	False	The coordinator is not aware of this member.
INVALID_SESSION_TIMEOUT	26	False	The session timeout is not within the range allowed by the broker (as configured by group.min.session.timeout.ms and group.max.session.timeout.ms).
REBALANCE_IN_PROGRESS	27	False	The group is rebalancing, so a rejoin is needed.
INVALID_COMMIT_OFFSET_SIZE	28	False	The committing offset data size is not valid
TOPIC_AUTHORIZATION_FAILED	29	False	Not authorized to access topics: [Topic authorization failed.]
GROUP_AUTHORIZATION_FAILED	30	False	Not authorized to access group: Group authorization failed.
CLUSTER_AUTHORIZATION_FAILED	31	False	Cluster authorization failed.
INVALID_TIMESTAMP	32	False	The timestamp of the message is out of acceptable range.
UNSUPPORTED_SASL_MECHANISM	33	False	The broker does not support the requested SASL mechanism.
ILLEGAL_SASL_STATE	34	False	Request is not valid given the current SASL state.
UNSUPPORTED_VERSION	35	False	The version of API is not supported.
TOPIC_ALREADY_EXISTS	36	False	Topic with this name already exists.
INVALID_PARTITIONS	37	False	Number of partitions is invalid.
INVALID_REPLICATION_FACTOR	38	False	Replication-factor is invalid.
INVALID_REPLICA_ASSIGNMENT	39	False	Replica assignment is invalid.
INVALID_CONFIG	40	False	Configuration is invalid.
NOT_CONTROLLER	41	True	This is not the correct controller for this cluster.
INVALID_REQUEST	42	False	This most likely occurs because of a request being malformed by the client library or the message was sent to an incompatible broker. See the broker logs for more details.
UNSUPPORTED_FOR_MESSAGE_FORMAT	43	False	The message format version on the broker does not support the request.
Api Keys

The following are the numeric codes that the ApiKey in the request can take for each of the below request types.

Name	Key
Produce	0
Fetch	1
Offsets	2
Metadata	3
LeaderAndIsr	4
StopReplica	5
UpdateMetadata	6
ControlledShutdown	7
OffsetCommit	8
OffsetFetch	9
GroupCoordinator	10
JoinGroup	11
Heartbeat	12
LeaveGroup	13
SyncGroup	14
DescribeGroups	15
ListGroups	16
SaslHandshake	17
ApiVersions	18
CreateTopics	19
DeleteTopics	20
The Messages

This section gives details on each of the individual API Messages, their usage, their binary format, and the meaning of their fields.

Headers:

Request Header => api_key api_version correlation_id client_id 
  api_key => INT16
  api_version => INT16
  correlation_id => INT32
  client_id => NULLABLE_STRING
Field	Description
api_key	The id of the request type.
api_version	The version of the API.
correlation_id	A user-supplied integer value that will be passed back with the response
client_id	A user specified identifier for the client making the request.
Response Header => correlation_id 
  correlation_id => INT32
Field	Description
correlation_id	The user-supplied value passed in with the request
Produce API (Key: 0):

Requests:
Produce Request (Version: 0) => acks timeout [topic_data] 
  acks => INT16
  timeout => INT32
  topic_data => topic [data] 
    topic => STRING
    data => partition record_set 
      partition => INT32
      record_set => BYTES
Field	Description
acks	The number of acknowledgments the producer requires the leader to have received before considering a request complete. Allowed values: 0 for no acknowledgments, 1 for only the leader and -1 for the full ISR.
timeout	The time to await a response in ms.
topic_data	
topic	
data	
partition	
record_set	
Produce Request (Version: 1) => acks timeout [topic_data] 
  acks => INT16
  timeout => INT32
  topic_data => topic [data] 
    topic => STRING
    data => partition record_set 
      partition => INT32
      record_set => BYTES
Field	Description
acks	The number of acknowledgments the producer requires the leader to have received before considering a request complete. Allowed values: 0 for no acknowledgments, 1 for only the leader and -1 for the full ISR.
timeout	The time to await a response in ms.
topic_data	
topic	
data	
partition	
record_set	
Produce Request (Version: 2) => acks timeout [topic_data] 
  acks => INT16
  timeout => INT32
  topic_data => topic [data] 
    topic => STRING
    data => partition record_set 
      partition => INT32
      record_set => BYTES
Field	Description
acks	The number of acknowledgments the producer requires the leader to have received before considering a request complete. Allowed values: 0 for no acknowledgments, 1 for only the leader and -1 for the full ISR.
timeout	The time to await a response in ms.
topic_data	
topic	
data	
partition	
record_set	
Responses:
Produce Response (Version: 0) => [responses] 
  responses => topic [partition_responses] 
    topic => STRING
    partition_responses => partition error_code base_offset 
      partition => INT32
      error_code => INT16
      base_offset => INT64
Field	Description
responses	
topic	
partition_responses	
partition	
error_code	
base_offset	
Produce Response (Version: 1) => [responses] throttle_time_ms 
  responses => topic [partition_responses] 
    topic => STRING
    partition_responses => partition error_code base_offset 
      partition => INT32
      error_code => INT16
      base_offset => INT64
  throttle_time_ms => INT32
Field	Description
responses	
topic	
partition_responses	
partition	
error_code	
base_offset	
throttle_time_ms	Duration in milliseconds for which the request was throttled due to quota violation. (Zero if the request did not violate any quota.)
Produce Response (Version: 2) => [responses] throttle_time_ms 
  responses => topic [partition_responses] 
    topic => STRING
    partition_responses => partition error_code base_offset timestamp 
      partition => INT32
      error_code => INT16
      base_offset => INT64
      timestamp => INT64
  throttle_time_ms => INT32
Field	Description
responses	
topic	
partition_responses	
partition	
error_code	
base_offset	
timestamp	The timestamp returned by broker after appending the messages. If CreateTime is used for the topic, the timestamp will be -1. If LogAppendTime is used for the topic, the timestamp will be the broker local time when the messages are appended.
throttle_time_ms	Duration in milliseconds for which the request was throttled due to quota violation. (Zero if the request did not violate any quota.)
Fetch API (Key: 1):

Requests:
Fetch Request (Version: 0) => replica_id max_wait_time min_bytes [topics] 
  replica_id => INT32
  max_wait_time => INT32
  min_bytes => INT32
  topics => topic [partitions] 
    topic => STRING
    partitions => partition fetch_offset max_bytes 
      partition => INT32
      fetch_offset => INT64
      max_bytes => INT32
Field	Description
replica_id	Broker id of the follower. For normal consumers, use -1.
max_wait_time	Maximum time in ms to wait for the response.
min_bytes	Minimum bytes to accumulate in the response.
topics	Topics to fetch.
topic	Topic to fetch.
partitions	Partitions to fetch.
partition	Topic partition id.
fetch_offset	Message offset.
max_bytes	Maximum bytes to fetch.
Fetch Request (Version: 1) => replica_id max_wait_time min_bytes [topics] 
  replica_id => INT32
  max_wait_time => INT32
  min_bytes => INT32
  topics => topic [partitions] 
    topic => STRING
    partitions => partition fetch_offset max_bytes 
      partition => INT32
      fetch_offset => INT64
      max_bytes => INT32
Field	Description
replica_id	Broker id of the follower. For normal consumers, use -1.
max_wait_time	Maximum time in ms to wait for the response.
min_bytes	Minimum bytes to accumulate in the response.
topics	Topics to fetch.
topic	Topic to fetch.
partitions	Partitions to fetch.
partition	Topic partition id.
fetch_offset	Message offset.
max_bytes	Maximum bytes to fetch.
Fetch Request (Version: 2) => replica_id max_wait_time min_bytes [topics] 
  replica_id => INT32
  max_wait_time => INT32
  min_bytes => INT32
  topics => topic [partitions] 
    topic => STRING
    partitions => partition fetch_offset max_bytes 
      partition => INT32
      fetch_offset => INT64
      max_bytes => INT32
Field	Description
replica_id	Broker id of the follower. For normal consumers, use -1.
max_wait_time	Maximum time in ms to wait for the response.
min_bytes	Minimum bytes to accumulate in the response.
topics	Topics to fetch.
topic	Topic to fetch.
partitions	Partitions to fetch.
partition	Topic partition id.
fetch_offset	Message offset.
max_bytes	Maximum bytes to fetch.
Fetch Request (Version: 3) => replica_id max_wait_time min_bytes max_bytes [topics] 
  replica_id => INT32
  max_wait_time => INT32
  min_bytes => INT32
  max_bytes => INT32
  topics => topic [partitions] 
    topic => STRING
    partitions => partition fetch_offset max_bytes 
      partition => INT32
      fetch_offset => INT64
      max_bytes => INT32
Field	Description
replica_id	Broker id of the follower. For normal consumers, use -1.
max_wait_time	Maximum time in ms to wait for the response.
min_bytes	Minimum bytes to accumulate in the response.
max_bytes	Maximum bytes to accumulate in the response. Note that this is not an absolute maximum, if the first message in the first non-empty partition of the fetch is larger than this value, the message will still be returned to ensure that progress can be made.
topics	Topics to fetch in the order provided.
topic	Topic to fetch.
partitions	Partitions to fetch.
partition	Topic partition id.
fetch_offset	Message offset.
max_bytes	Maximum bytes to fetch.
Responses:
Fetch Response (Version: 0) => [responses] 
  responses => topic [partition_responses] 
    topic => STRING
    partition_responses => partition error_code high_watermark record_set 
      partition => INT32
      error_code => INT16
      high_watermark => INT64
      record_set => BYTES
Field	Description
responses	
topic	
partition_responses	
partition	Topic partition id.
error_code	
high_watermark	Last committed offset.
record_set	
Fetch Response (Version: 1) => throttle_time_ms [responses] 
  throttle_time_ms => INT32
  responses => topic [partition_responses] 
    topic => STRING
    partition_responses => partition error_code high_watermark record_set 
      partition => INT32
      error_code => INT16
      high_watermark => INT64
      record_set => BYTES
Field	Description
throttle_time_ms	Duration in milliseconds for which the request was throttled due to quota violation. (Zero if the request did not violate any quota.)
responses	
topic	
partition_responses	
partition	Topic partition id.
error_code	
high_watermark	Last committed offset.
record_set	
Fetch Response (Version: 2) => throttle_time_ms [responses] 
  throttle_time_ms => INT32
  responses => topic [partition_responses] 
    topic => STRING
    partition_responses => partition error_code high_watermark record_set 
      partition => INT32
      error_code => INT16
      high_watermark => INT64
      record_set => BYTES
Field	Description
throttle_time_ms	Duration in milliseconds for which the request was throttled due to quota violation. (Zero if the request did not violate any quota.)
responses	
topic	
partition_responses	
partition	Topic partition id.
error_code	
high_watermark	Last committed offset.
record_set	
Fetch Response (Version: 3) => throttle_time_ms [responses] 
  throttle_time_ms => INT32
  responses => topic [partition_responses] 
    topic => STRING
    partition_responses => partition error_code high_watermark record_set 
      partition => INT32
      error_code => INT16
      high_watermark => INT64
      record_set => BYTES
Field	Description
throttle_time_ms	Duration in milliseconds for which the request was throttled due to quota violation. (Zero if the request did not violate any quota.)
responses	
topic	
partition_responses	
partition	Topic partition id.
error_code	
high_watermark	Last committed offset.
record_set	
Offsets API (Key: 2):

Requests:
Offsets Request (Version: 0) => replica_id [topics] 
  replica_id => INT32
  topics => topic [partitions] 
    topic => STRING
    partitions => partition timestamp max_num_offsets 
      partition => INT32
      timestamp => INT64
      max_num_offsets => INT32
Field	Description
replica_id	Broker id of the follower. For normal consumers, use -1.
topics	Topics to list offsets.
topic	Topic to list offset.
partitions	Partitions to list offset.
partition	Topic partition id.
timestamp	Timestamp.
max_num_offsets	Maximum offsets to return.
Offsets Request (Version: 1) => replica_id [topics] 
  replica_id => INT32
  topics => topic [partitions] 
    topic => STRING
    partitions => partition timestamp 
      partition => INT32
      timestamp => INT64
Field	Description
replica_id	Broker id of the follower. For normal consumers, use -1.
topics	Topics to list offsets.
topic	Topic to list offset.
partitions	Partitions to list offset.
partition	Topic partition id.
timestamp	The target timestamp for the partition.
Responses:
Offsets Response (Version: 0) => [responses] 
  responses => topic [partition_responses] 
    topic => STRING
    partition_responses => partition error_code [offsets] 
      partition => INT32
      error_code => INT16
      offsets => INT64
Field	Description
responses	
topic	
partition_responses	
partition	Topic partition id.
error_code	
offsets	A list of offsets.
Offsets Response (Version: 1) => [responses] 
  responses => topic [partition_responses] 
    topic => STRING
    partition_responses => partition error_code timestamp offset 
      partition => INT32
      error_code => INT16
      timestamp => INT64
      offset => INT64
Field	Description
responses	
topic	
partition_responses	
partition	Topic partition id.
error_code	
timestamp	The timestamp associated with the returned offset
offset	offset found
Metadata API (Key: 3):

Requests:
Metadata Request (Version: 0) => [topics] 
  topics => STRING
Field	Description
topics	An array of topics to fetch metadata for. If no topics are specified fetch metadata for all topics.
Metadata Request (Version: 1) => [topics] 
  topics => STRING
Field	Description
topics	An array of topics to fetch metadata for. If the topics array is null fetch metadata for all topics.
Metadata Request (Version: 2) => [topics] 
  topics => STRING
Field	Description
topics	An array of topics to fetch metadata for. If the topics array is null fetch metadata for all topics.
Responses:
Metadata Response (Version: 0) => [brokers] [topic_metadata] 
  brokers => node_id host port 
    node_id => INT32
    host => STRING
    port => INT32
  topic_metadata => topic_error_code topic [partition_metadata] 
    topic_error_code => INT16
    topic => STRING
    partition_metadata => partition_error_code partition_id leader [replicas] [isr] 
      partition_error_code => INT16
      partition_id => INT32
      leader => INT32
      replicas => INT32
      isr => INT32
Field	Description
brokers	Host and port information for all brokers.
node_id	The broker id.
host	The hostname of the broker.
port	The port on which the broker accepts requests.
topic_metadata	
topic_error_code	The error code for the given topic.
topic	The name of the topic
partition_metadata	Metadata for each partition of the topic.
partition_error_code	The error code for the partition, if any.
partition_id	The id of the partition.
leader	The id of the broker acting as leader for this partition.
replicas	The set of all nodes that host this partition.
isr	The set of nodes that are in sync with the leader for this partition.
Metadata Response (Version: 1) => [brokers] controller_id [topic_metadata] 
  brokers => node_id host port rack 
    node_id => INT32
    host => STRING
    port => INT32
    rack => NULLABLE_STRING
  controller_id => INT32
  topic_metadata => topic_error_code topic is_internal [partition_metadata] 
    topic_error_code => INT16
    topic => STRING
    is_internal => BOOLEAN
    partition_metadata => partition_error_code partition_id leader [replicas] [isr] 
      partition_error_code => INT16
      partition_id => INT32
      leader => INT32
      replicas => INT32
      isr => INT32
Field	Description
brokers	Host and port information for all brokers.
node_id	The broker id.
host	The hostname of the broker.
port	The port on which the broker accepts requests.
rack	The rack of the broker.
controller_id	The broker id of the controller broker.
topic_metadata	
topic_error_code	The error code for the given topic.
topic	The name of the topic
is_internal	Indicates if the topic is considered a Kafka internal topic
partition_metadata	Metadata for each partition of the topic.
partition_error_code	The error code for the partition, if any.
partition_id	The id of the partition.
leader	The id of the broker acting as leader for this partition.
replicas	The set of all nodes that host this partition.
isr	The set of nodes that are in sync with the leader for this partition.
Metadata Response (Version: 2) => [brokers] cluster_id controller_id [topic_metadata] 
  brokers => node_id host port rack 
    node_id => INT32
    host => STRING
    port => INT32
    rack => NULLABLE_STRING
  cluster_id => NULLABLE_STRING
  controller_id => INT32
  topic_metadata => topic_error_code topic is_internal [partition_metadata] 
    topic_error_code => INT16
    topic => STRING
    is_internal => BOOLEAN
    partition_metadata => partition_error_code partition_id leader [replicas] [isr] 
      partition_error_code => INT16
      partition_id => INT32
      leader => INT32
      replicas => INT32
      isr => INT32
Field	Description
brokers	Host and port information for all brokers.
node_id	The broker id.
host	The hostname of the broker.
port	The port on which the broker accepts requests.
rack	The rack of the broker.
cluster_id	The cluster id that this broker belongs to.
controller_id	The broker id of the controller broker.
topic_metadata	
topic_error_code	The error code for the given topic.
topic	The name of the topic
is_internal	Indicates if the topic is considered a Kafka internal topic
partition_metadata	Metadata for each partition of the topic.
partition_error_code	The error code for the partition, if any.
partition_id	The id of the partition.
leader	The id of the broker acting as leader for this partition.
replicas	The set of all nodes that host this partition.
isr	The set of nodes that are in sync with the leader for this partition.
LeaderAndIsr API (Key: 4):

Requests:
LeaderAndIsr Request (Version: 0) => controller_id controller_epoch [partition_states] [live_leaders] 
  controller_id => INT32
  controller_epoch => INT32
  partition_states => topic partition controller_epoch leader leader_epoch [isr] zk_version [replicas] 
    topic => STRING
    partition => INT32
    controller_epoch => INT32
    leader => INT32
    leader_epoch => INT32
    isr => INT32
    zk_version => INT32
    replicas => INT32
  live_leaders => id host port 
    id => INT32
    host => STRING
    port => INT32
Field	Description
controller_id	The controller id.
controller_epoch	The controller epoch.
partition_states	
topic	Topic name.
partition	Topic partition id.
controller_epoch	The controller epoch.
leader	The broker id for the leader.
leader_epoch	The leader epoch.
isr	The in sync replica ids.
zk_version	The ZK version.
replicas	The replica ids.
live_leaders	
id	The broker id.
host	The hostname of the broker.
port	The port on which the broker accepts requests.
Responses:
LeaderAndIsr Response (Version: 0) => error_code [partitions] 
  error_code => INT16
  partitions => topic partition error_code 
    topic => STRING
    partition => INT32
    error_code => INT16
Field	Description
error_code	Error code.
partitions	
topic	Topic name.
partition	Topic partition id.
error_code	Error code.
StopReplica API (Key: 5):

Requests:
StopReplica Request (Version: 0) => controller_id controller_epoch delete_partitions [partitions] 
  controller_id => INT32
  controller_epoch => INT32
  delete_partitions => BOOLEAN
  partitions => topic partition 
    topic => STRING
    partition => INT32
Field	Description
controller_id	The controller id.
controller_epoch	The controller epoch.
delete_partitions	Boolean which indicates if replica's partitions must be deleted.
partitions	
topic	Topic name.
partition	Topic partition id.
Responses:
StopReplica Response (Version: 0) => error_code [partitions] 
  error_code => INT16
  partitions => topic partition error_code 
    topic => STRING
    partition => INT32
    error_code => INT16
Field	Description
error_code	Error code.
partitions	
topic	Topic name.
partition	Topic partition id.
error_code	Error code.
UpdateMetadata API (Key: 6):

Requests:
UpdateMetadata Request (Version: 0) => controller_id controller_epoch [partition_states] [live_brokers] 
  controller_id => INT32
  controller_epoch => INT32
  partition_states => topic partition controller_epoch leader leader_epoch [isr] zk_version [replicas] 
    topic => STRING
    partition => INT32
    controller_epoch => INT32
    leader => INT32
    leader_epoch => INT32
    isr => INT32
    zk_version => INT32
    replicas => INT32
  live_brokers => id host port 
    id => INT32
    host => STRING
    port => INT32
Field	Description
controller_id	The controller id.
controller_epoch	The controller epoch.
partition_states	
topic	Topic name.
partition	Topic partition id.
controller_epoch	The controller epoch.
leader	The broker id for the leader.
leader_epoch	The leader epoch.
isr	The in sync replica ids.
zk_version	The ZK version.
replicas	The replica ids.
live_brokers	
id	The broker id.
host	The hostname of the broker.
port	The port on which the broker accepts requests.
UpdateMetadata Request (Version: 1) => controller_id controller_epoch [partition_states] [live_brokers] 
  controller_id => INT32
  controller_epoch => INT32
  partition_states => topic partition controller_epoch leader leader_epoch [isr] zk_version [replicas] 
    topic => STRING
    partition => INT32
    controller_epoch => INT32
    leader => INT32
    leader_epoch => INT32
    isr => INT32
    zk_version => INT32
    replicas => INT32
  live_brokers => id [end_points] 
    id => INT32
    end_points => port host security_protocol_type 
      port => INT32
      host => STRING
      security_protocol_type => INT16
Field	Description
controller_id	The controller id.
controller_epoch	The controller epoch.
partition_states	
topic	Topic name.
partition	Topic partition id.
controller_epoch	The controller epoch.
leader	The broker id for the leader.
leader_epoch	The leader epoch.
isr	The in sync replica ids.
zk_version	The ZK version.
replicas	The replica ids.
live_brokers	
id	The broker id.
end_points	
port	The port on which the broker accepts requests.
host	The hostname of the broker.
security_protocol_type	The security protocol type.
UpdateMetadata Request (Version: 2) => controller_id controller_epoch [partition_states] [live_brokers] 
  controller_id => INT32
  controller_epoch => INT32
  partition_states => topic partition controller_epoch leader leader_epoch [isr] zk_version [replicas] 
    topic => STRING
    partition => INT32
    controller_epoch => INT32
    leader => INT32
    leader_epoch => INT32
    isr => INT32
    zk_version => INT32
    replicas => INT32
  live_brokers => id [end_points] rack 
    id => INT32
    end_points => port host security_protocol_type 
      port => INT32
      host => STRING
      security_protocol_type => INT16
    rack => NULLABLE_STRING
Field	Description
controller_id	The controller id.
controller_epoch	The controller epoch.
partition_states	
topic	Topic name.
partition	Topic partition id.
controller_epoch	The controller epoch.
leader	The broker id for the leader.
leader_epoch	The leader epoch.
isr	The in sync replica ids.
zk_version	The ZK version.
replicas	The replica ids.
live_brokers	
id	The broker id.
end_points	
port	The port on which the broker accepts requests.
host	The hostname of the broker.
security_protocol_type	The security protocol type.
rack	The rack
Responses:
UpdateMetadata Response (Version: 0) => error_code 
  error_code => INT16
Field	Description
error_code	Error code.
UpdateMetadata Response (Version: 1) => error_code 
  error_code => INT16
Field	Description
error_code	Error code.
UpdateMetadata Response (Version: 2) => error_code 
  error_code => INT16
Field	Description
error_code	Error code.
ControlledShutdown API (Key: 7):

Requests:
ControlledShutdown Request (Version: 1) => broker_id 
  broker_id => INT32
Field	Description
broker_id	The id of the broker for which controlled shutdown has been requested.
Responses:
ControlledShutdown Response (Version: 1) => error_code [partitions_remaining] 
  error_code => INT16
  partitions_remaining => topic partition 
    topic => STRING
    partition => INT32
Field	Description
error_code	
partitions_remaining	The partitions that the broker still leads.
topic	
partition	Topic partition id.
OffsetCommit API (Key: 8):

Requests:
OffsetCommit Request (Version: 0) => group_id [topics] 
  group_id => STRING
  topics => topic [partitions] 
    topic => STRING
    partitions => partition offset metadata 
      partition => INT32
      offset => INT64
      metadata => NULLABLE_STRING
Field	Description
group_id	The group id.
topics	Topics to commit offsets.
topic	Topic to commit.
partitions	Partitions to commit offsets.
partition	Topic partition id.
offset	Message offset to be committed.
metadata	Any associated metadata the client wants to keep.
OffsetCommit Request (Version: 1) => group_id group_generation_id member_id [topics] 
  group_id => STRING
  group_generation_id => INT32
  member_id => STRING
  topics => topic [partitions] 
    topic => STRING
    partitions => partition offset timestamp metadata 
      partition => INT32
      offset => INT64
      timestamp => INT64
      metadata => NULLABLE_STRING
Field	Description
group_id	The group id.
group_generation_id	The generation of the group.
member_id	The member id assigned by the group coordinator.
topics	Topics to commit offsets.
topic	Topic to commit.
partitions	Partitions to commit offsets.
partition	Topic partition id.
offset	Message offset to be committed.
timestamp	Timestamp of the commit
metadata	Any associated metadata the client wants to keep.
OffsetCommit Request (Version: 2) => group_id group_generation_id member_id retention_time [topics] 
  group_id => STRING
  group_generation_id => INT32
  member_id => STRING
  retention_time => INT64
  topics => topic [partitions] 
    topic => STRING
    partitions => partition offset metadata 
      partition => INT32
      offset => INT64
      metadata => NULLABLE_STRING
Field	Description
group_id	The group id.
group_generation_id	The generation of the consumer group.
member_id	The consumer id assigned by the group coordinator.
retention_time	Time period in ms to retain the offset.
topics	Topics to commit offsets.
topic	Topic to commit.
partitions	Partitions to commit offsets.
partition	Topic partition id.
offset	Message offset to be committed.
metadata	Any associated metadata the client wants to keep.
Responses:
OffsetCommit Response (Version: 0) => [responses] 
  responses => topic [partition_responses] 
    topic => STRING
    partition_responses => partition error_code 
      partition => INT32
      error_code => INT16
Field	Description
responses	
topic	
partition_responses	
partition	Topic partition id.
error_code	
OffsetCommit Response (Version: 1) => [responses] 
  responses => topic [partition_responses] 
    topic => STRING
    partition_responses => partition error_code 
      partition => INT32
      error_code => INT16
Field	Description
responses	
topic	
partition_responses	
partition	Topic partition id.
error_code	
OffsetCommit Response (Version: 2) => [responses] 
  responses => topic [partition_responses] 
    topic => STRING
    partition_responses => partition error_code 
      partition => INT32
      error_code => INT16
Field	Description
responses	
topic	
partition_responses	
partition	Topic partition id.
error_code	
OffsetFetch API (Key: 9):

Requests:
OffsetFetch Request (Version: 0) => group_id [topics] 
  group_id => STRING
  topics => topic [partitions] 
    topic => STRING
    partitions => partition 
      partition => INT32
Field	Description
group_id	The consumer group id.
topics	Topics to fetch offsets.
topic	Topic to fetch offset.
partitions	Partitions to fetch offsets.
partition	Topic partition id.
OffsetFetch Request (Version: 1) => group_id [topics] 
  group_id => STRING
  topics => topic [partitions] 
    topic => STRING
    partitions => partition 
      partition => INT32
Field	Description
group_id	The consumer group id.
topics	Topics to fetch offsets.
topic	Topic to fetch offset.
partitions	Partitions to fetch offsets.
partition	Topic partition id.
Responses:
OffsetFetch Response (Version: 0) => [responses] 
  responses => topic [partition_responses] 
    topic => STRING
    partition_responses => partition offset metadata error_code 
      partition => INT32
      offset => INT64
      metadata => NULLABLE_STRING
      error_code => INT16
Field	Description
responses	
topic	
partition_responses	
partition	Topic partition id.
offset	Last committed message offset.
metadata	Any associated metadata the client wants to keep.
error_code	
OffsetFetch Response (Version: 1) => [responses] 
  responses => topic [partition_responses] 
    topic => STRING
    partition_responses => partition offset metadata error_code 
      partition => INT32
      offset => INT64
      metadata => NULLABLE_STRING
      error_code => INT16
Field	Description
responses	
topic	
partition_responses	
partition	Topic partition id.
offset	Last committed message offset.
metadata	Any associated metadata the client wants to keep.
error_code	
GroupCoordinator API (Key: 10):

Requests:
GroupCoordinator Request (Version: 0) => group_id 
  group_id => STRING
Field	Description
group_id	The unique group id.
Responses:
GroupCoordinator Response (Version: 0) => error_code coordinator 
  error_code => INT16
  coordinator => node_id host port 
    node_id => INT32
    host => STRING
    port => INT32
Field	Description
error_code	
coordinator	Host and port information for the coordinator for a consumer group.
node_id	The broker id.
host	The hostname of the broker.
port	The port on which the broker accepts requests.
JoinGroup API (Key: 11):

Requests:
JoinGroup Request (Version: 0) => group_id session_timeout member_id protocol_type [group_protocols] 
  group_id => STRING
  session_timeout => INT32
  member_id => STRING
  protocol_type => STRING
  group_protocols => protocol_name protocol_metadata 
    protocol_name => STRING
    protocol_metadata => BYTES
Field	Description
group_id	The group id.
session_timeout	The coordinator considers the consumer dead if it receives no heartbeat after this timeout in ms.
member_id	The assigned consumer id or an empty string for a new consumer.
protocol_type	Unique name for class of protocols implemented by group
group_protocols	List of protocols that the member supports
protocol_name	
protocol_metadata	
JoinGroup Request (Version: 1) => group_id session_timeout rebalance_timeout member_id protocol_type [group_protocols] 
  group_id => STRING
  session_timeout => INT32
  rebalance_timeout => INT32
  member_id => STRING
  protocol_type => STRING
  group_protocols => protocol_name protocol_metadata 
    protocol_name => STRING
    protocol_metadata => BYTES
Field	Description
group_id	The group id.
session_timeout	The coordinator considers the consumer dead if it receives no heartbeat after this timeout in ms.
rebalance_timeout	The maximum time that the coordinator will wait for each member to rejoin when rebalancing the group
member_id	The assigned consumer id or an empty string for a new consumer.
protocol_type	Unique name for class of protocols implemented by group
group_protocols	List of protocols that the member supports
protocol_name	
protocol_metadata	
Responses:
JoinGroup Response (Version: 0) => error_code generation_id group_protocol leader_id member_id [members] 
  error_code => INT16
  generation_id => INT32
  group_protocol => STRING
  leader_id => STRING
  member_id => STRING
  members => member_id member_metadata 
    member_id => STRING
    member_metadata => BYTES
Field	Description
error_code	
generation_id	The generation of the consumer group.
group_protocol	The group protocol selected by the coordinator
leader_id	The leader of the group
member_id	The consumer id assigned by the group coordinator.
members	
member_id	
member_metadata	
JoinGroup Response (Version: 1) => error_code generation_id group_protocol leader_id member_id [members] 
  error_code => INT16
  generation_id => INT32
  group_protocol => STRING
  leader_id => STRING
  member_id => STRING
  members => member_id member_metadata 
    member_id => STRING
    member_metadata => BYTES
Field	Description
error_code	
generation_id	The generation of the consumer group.
group_protocol	The group protocol selected by the coordinator
leader_id	The leader of the group
member_id	The consumer id assigned by the group coordinator.
members	
member_id	
member_metadata	
Heartbeat API (Key: 12):

Requests:
Heartbeat Request (Version: 0) => group_id group_generation_id member_id 
  group_id => STRING
  group_generation_id => INT32
  member_id => STRING
Field	Description
group_id	The group id.
group_generation_id	The generation of the group.
member_id	The member id assigned by the group coordinator.
Responses:
Heartbeat Response (Version: 0) => error_code 
  error_code => INT16
Field	Description
error_code	
LeaveGroup API (Key: 13):

Requests:
LeaveGroup Request (Version: 0) => group_id member_id 
  group_id => STRING
  member_id => STRING
Field	Description
group_id	The group id.
member_id	The member id assigned by the group coordinator.
Responses:
LeaveGroup Response (Version: 0) => error_code 
  error_code => INT16
Field	Description
error_code	
SyncGroup API (Key: 14):

Requests:
SyncGroup Request (Version: 0) => group_id generation_id member_id [group_assignment] 
  group_id => STRING
  generation_id => INT32
  member_id => STRING
  group_assignment => member_id member_assignment 
    member_id => STRING
    member_assignment => BYTES
Field	Description
group_id	
generation_id	
member_id	
group_assignment	
member_id	
member_assignment	
Responses:
SyncGroup Response (Version: 0) => error_code member_assignment 
  error_code => INT16
  member_assignment => BYTES
Field	Description
error_code	
member_assignment	
DescribeGroups API (Key: 15):

Requests:
DescribeGroups Request (Version: 0) => [group_ids] 
  group_ids => STRING
Field	Description
group_ids	List of groupIds to request metadata for (an empty groupId array will return empty group metadata).
Responses:
DescribeGroups Response (Version: 0) => [groups] 
  groups => error_code group_id state protocol_type protocol [members] 
    error_code => INT16
    group_id => STRING
    state => STRING
    protocol_type => STRING
    protocol => STRING
    members => member_id client_id client_host member_metadata member_assignment 
      member_id => STRING
      client_id => STRING
      client_host => STRING
      member_metadata => BYTES
      member_assignment => BYTES
Field	Description
groups	
error_code	
group_id	
state	The current state of the group (one of: Dead, Stable, AwaitingSync, or PreparingRebalance, or empty if there is no active group)
protocol_type	The current group protocol type (will be empty if there is no active group)
protocol	The current group protocol (only provided if the group is Stable)
members	Current group members (only provided if the group is not Dead)
member_id	The memberId assigned by the coordinator
client_id	The client id used in the member's latest join group request
client_host	The client host used in the request session corresponding to the member's join group.
member_metadata	The metadata corresponding to the current group protocol in use (will only be present if the group is stable).
member_assignment	The current assignment provided by the group leader (will only be present if the group is stable).
ListGroups API (Key: 16):

Requests:
ListGroups Request (Version: 0) => 
Field	Description
Responses:
ListGroups Response (Version: 0) => error_code [groups] 
  error_code => INT16
  groups => group_id protocol_type 
    group_id => STRING
    protocol_type => STRING
Field	Description
error_code	
groups	
group_id	
protocol_type	
SaslHandshake API (Key: 17):

Requests:
SaslHandshake Request (Version: 0) => mechanism 
  mechanism => STRING
Field	Description
mechanism	SASL Mechanism chosen by the client.
Responses:
SaslHandshake Response (Version: 0) => error_code [enabled_mechanisms] 
  error_code => INT16
  enabled_mechanisms => STRING
Field	Description
error_code	
enabled_mechanisms	Array of mechanisms enabled in the server.
ApiVersions API (Key: 18):

Requests:
ApiVersions Request (Version: 0) => 
Field	Description
Responses:
ApiVersions Response (Version: 0) => error_code [api_versions] 
  error_code => INT16
  api_versions => api_key min_version max_version 
    api_key => INT16
    min_version => INT16
    max_version => INT16
Field	Description
error_code	Error code.
api_versions	API versions supported by the broker.
api_key	API key.
min_version	Minimum supported version.
max_version	Maximum supported version.
CreateTopics API (Key: 19):

Requests:
CreateTopics Request (Version: 0) => [create_topic_requests] timeout 
  create_topic_requests => topic num_partitions replication_factor [replica_assignment] [configs] 
    topic => STRING
    num_partitions => INT32
    replication_factor => INT16
    replica_assignment => partition_id [replicas] 
      partition_id => INT32
      replicas => INT32
    configs => config_key config_value 
      config_key => STRING
      config_value => STRING
  timeout => INT32
Field	Description
create_topic_requests	An array of single topic creation requests. Can not have multiple entries for the same topic.
topic	Name for newly created topic.
num_partitions	Number of partitions to be created. -1 indicates unset.
replication_factor	Replication factor for the topic. -1 indicates unset.
replica_assignment	Replica assignment among kafka brokers for this topic partitions. If this is set num_partitions and replication_factor must be unset.
partition_id	
replicas	The set of all nodes that should host this partition. The first replica in the list is the preferred leader.
configs	Topic level configuration for topic to be set.
config_key	Configuration key name
config_value	Configuration value
timeout	The time in ms to wait for a topic to be completely created on the controller node. Values <= 0 will trigger topic creation and return immediately
Responses:
CreateTopics Response (Version: 0) => [topic_error_codes] 
  topic_error_codes => topic error_code 
    topic => STRING
    error_code => INT16
Field	Description
topic_error_codes	An array of per topic error codes.
topic	
error_code	
DeleteTopics API (Key: 20):

Requests:
DeleteTopics Request (Version: 0) => [topics] timeout 
  topics => STRING
  timeout => INT32
Field	Description
topics	An array of topics to be deleted.
timeout	The time in ms to wait for a topic to be completely deleted on the controller node. Values <= 0 will trigger topic deletion and return immediately
Responses:
DeleteTopics Response (Version: 0) => [topic_error_codes] 
  topic_error_codes => topic error_code 
    topic => STRING
    error_code => INT16
Field	Description
topic_error_codes	An array of per topic error codes.
topic	
error_code	
Some Common Philosophical Questions

Some people have asked why we don't use HTTP. There are a number of reasons, the best is that client implementors can make use of some of the more advanced TCP features--the ability to multiplex requests, the ability to simultaneously poll many connections, etc. We have also found HTTP libraries in many languages to be surprisingly shabby.

Others have asked if maybe we shouldn't support many different protocols. Prior experience with this was that it makes it very hard to add and test new features if they have to be ported across many protocol implementations. Our feeling is that most users don't really see multiple protocols as a feature, they just want a good reliable client in the language of their choice.

Another question is why we don't adopt XMPP, STOMP, AMQP or an existing protocol. The answer to this varies by protocol, but in general the problem is that the protocol does determine large parts of the implementation and we couldn't do what we are doing if we didn't have control over the protocol. Our belief is that it is possible to do better than existing messaging systems have in providing a truly distributed messaging system, and to do this we need to build something that works differently.

A final question is why we don't use a system like Protocol Buffers or Thrift to define our request messages. These packages excel at helping you to managing lots and lots of serialized messages. However we have only a few messages. Support across languages is somewhat spotty (depending on the package). Finally the mapping between binary log format and wire protocol is something we manage somewhat carefully and this would not be possible with these systems. Finally we prefer the style of versioning APIs explicitly and checking this to inferring new values as nulls as it allows more nuanced control of compatibility.

[an error occurred while processing this directive]