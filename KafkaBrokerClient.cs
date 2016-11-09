// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KafkaBrokerClient.cs" company="">
//   Copyright by Hongwei Liu(hongwei_liu@outlook.com).
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kafka.Management
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using Kafka.Management.KafkaProtocol;

    /// <summary>
    /// The kafka broker client managements network communications.
    /// </summary>
    public class KafkaBrokerClient : IDisposable
    {
        /// <summary>
        /// The tcp client to communicate with broker.
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// The network stream of tcp connection.
        /// </summary>
        private NetworkStream tcpStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaBrokerClient"/> class.
        /// </summary>
        public KafkaBrokerClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaBrokerClient"/> class.
        /// </summary>
        /// <param name="ip">the ip address of broker.</param>
        /// <param name="port">the port of broker.</param>
        public KafkaBrokerClient(string ip, string port)
        {
            this.BrokerEndPoint = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Gets or sets the ip endpoint to access a kafka broker.
        /// </summary>
        public IPEndPoint BrokerEndPoint { get; set; }
        
        /// <summary>
        /// Send the request to kafka and wait for the response.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response from kafka.</returns>
        public KafkaProtocolResponse Send(KafkaProtocolRequest request)
        {
            if (request == null)
            {
                return null;
            }

            this.ConnectToBroker();

            var packet = request.Packet;
            if (packet != null && packet.Length > 0)
            {
                this.tcpStream.Write(packet, 0, packet.Length);
                this.tcpStream.Flush();
            }

            // read the response
            var bytes = new List<byte>();
            ////StringBuilder responseText = new StringBuilder();
            if (this.tcpStream.CanRead)
            {
                var buffer = new byte[1024];
                var bytesRead = 0;

                do
                {
                    bytesRead = this.tcpStream.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < bytesRead; i++)
                    {
                        bytes.Add(buffer[i]);
                    }

                    // the data might not ready on server, so give server some time
                    Thread.Sleep(5);
                }
                while (this.tcpStream.DataAvailable && bytesRead > 0);
            }

            byte[] responseBytes = bytes.ToArray();
            if (responseBytes == null || responseBytes.Length < 4)
            {
                return null;
            }

            // size 
            var bytesOfSize = new byte[4];
            Array.Copy(responseBytes, bytesOfSize, 4);
            var responseSize = KafkaProtocolPrimitiveType.GetInt32(bytesOfSize);
            if (responseSize <= 0 || responseBytes.Length - 4 < responseSize)
            {
                return null;
            }

            // correlationId
            var correlationIdBytes = new byte[4];
            Array.Copy(responseBytes, 4, correlationIdBytes, 0, 4);
            var correlationId = KafkaProtocolPrimitiveType.GetInt32(correlationIdBytes);
            if (correlationId != request.CorrelationId)
            {
                // this is not the response to the request
                return null;
            }

            var data = new byte[responseSize - 4];
            Array.Copy(responseBytes, 8, data, 0, data.Length);

            KafkaProtocolResponse response = null;
            switch (request.ApiKey)
            {
                case KafkaProtocolApiKey.ProduceRequest:
                    break;
                case KafkaProtocolApiKey.FetchRequest:
                    break;
                case KafkaProtocolApiKey.OffsetRequest:
                    break;
                case KafkaProtocolApiKey.MetadataRequest:
                    response = new KafkaProtocolTopicMetadataResponse();
                    break;
                case KafkaProtocolApiKey.LeaderAndIsrRequest:
                    break;
                case KafkaProtocolApiKey.StopReplicaRequest:
                    break;
                case KafkaProtocolApiKey.UpdateMetadataRequest:
                    break;
                case KafkaProtocolApiKey.ControlledShutdownRequest:
                    break;
                case KafkaProtocolApiKey.OffsetCommitRequest:
                    break;
                case KafkaProtocolApiKey.OffsetFetchRequest:
                    break;
                case KafkaProtocolApiKey.GroupCoordinatorRequest:
                    break;
                case KafkaProtocolApiKey.JoinGroupRequest:
                    break;
                case KafkaProtocolApiKey.HeartbeatRequest:
                    break;
                case KafkaProtocolApiKey.LeaveGroupRequest:
                    break;
                case KafkaProtocolApiKey.SyncGroupRequest:
                    break;
                case KafkaProtocolApiKey.DescribeGroupsRequest:
                    break;
                case KafkaProtocolApiKey.ListGroupsRequest:
                    response = new KafkaProtocolConsumerListGroupResponse();
                    break;
                case KafkaProtocolApiKey.SaslHandshakeRequest:
                    break;
                case KafkaProtocolApiKey.ApiVersionsRequest:
                    break;
                case KafkaProtocolApiKey.CreateTopicsRequest:
                    break;
                case KafkaProtocolApiKey.DeleteTopicsRequest:
                    break;
            }

            if (response != null)
            {
                response.Parse(data);
            }

            return response;
        }
        
        /// <summary>
        /// Connect to the kafka broker for communication.
        /// </summary>
        private void ConnectToBroker()
        {
            if (this.tcpClient != null && this.tcpStream != null && this.tcpClient.Connected)
            {
                return;
            }

            if (this.tcpClient == null)
            {
                if (this.BrokerEndPoint == null)
                {
                    throw new ArgumentException("BrokerEndPoint is not set.");
                }

                this.tcpClient = new TcpClient();
                this.tcpClient.SendTimeout = int.MaxValue;
                this.tcpClient.ReceiveTimeout = int.MaxValue;
                this.tcpClient.SendBufferSize = 100 * 1024;
            }

            if (!this.tcpClient.Connected)
            {
                this.tcpClient.Connect(this.BrokerEndPoint);
            }

            if (this.tcpStream == null)
            {
                this.tcpStream = this.tcpClient.GetStream();
            }
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
                if (this.tcpClient != null)
                {
                    this.tcpClient.Close();
                    this.tcpClient = null;
                }

                if (this.tcpStream != null)
                {
                    this.tcpStream.Dispose();
                    this.tcpStream = null;
                }
            }
        }
    }
}