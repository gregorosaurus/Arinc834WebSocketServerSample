using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ARINC834WebSocketsSample
{
    public class WebSocketsDataStream
    {
        private bool _running = false;

        private IARINCDataSource _dataSource;

        private TimeSpan FrequencyOfDataSend = TimeSpan.FromMilliseconds(125);

        #region WebSockets Properties
        /// <summary>
        /// A mapping between the websocket and the completed task.
        /// The task should be called when we are done with the client (ie, disconnect
        /// or errors)
        /// </summary>
        public Dictionary<WebSocket, TaskCompletionSource<object>> clients = new Dictionary<WebSocket, TaskCompletionSource<object>>();
        #endregion

        public WebSocketsDataStream(IARINCDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public void StartStreamingData()
        {
            _running = true;
            Task.Run(async () => await _StartStreamingDataAsync());
        }

        public void AddWebSocketClient(WebSocket client, TaskCompletionSource<object> socketCompletionTask)
        {
            lock(clients)
            {
                clients.Add(client, socketCompletionTask);
            }
        }


        private async Task _StartStreamingDataAsync()
        {
            var receiveBuffer = new byte[1024 * 4];

            while(_running)
            {
                List<A834DataParameter> dataToBroadcast = await _dataSource.GetCurrentDataAsync();

                //serialization, it's a string for simplicity.  In production, this should be a buffer or memorystream.
                string json = JsonSerializer.Serialize(dataToBroadcast);
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(json);

                //If an error occurred on a client, we will close them. This
                //list is used to keep track of the clients to close. 
                List<WebSocket> clientsToClose = new List<WebSocket>();
                foreach(WebSocket client in clients.Keys)
                {
                    try
                    {
                        await client.SendAsync(buffer, WebSocketMessageType.Text, true /*The data in the buffer is the last part of the message*/, CancellationToken.None);
                    }
                    catch(WebSocketException ex)
                    {
                        Console.WriteLine("Websocket exception:" + ex.WebSocketErrorCode.ToString());
                        clientsToClose.Add(client);
                    }
                }

                foreach(WebSocket clientToClose in clientsToClose)
                {
                    await clientToClose.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Disconnect", CancellationToken.None);
                    clients[clientToClose].SetResult(false);
                    lock (clients)
                    {
                        clients.Remove(clientToClose);
                    }
                }

                Thread.Sleep((int)FrequencyOfDataSend.TotalMilliseconds);
            }



            foreach (WebSocket client in clients.Keys)
            {
                try
                {
                    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Shutting down", CancellationToken.None);
                }
                catch { }
            }
        }

    }
}
