﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using HA4IoT.Contracts.Networking;
using Buffer = Windows.Storage.Streams.Buffer;

namespace HA4IoT.Networking.Http
{
    public class WebSocketClientSession : IWebSocketClientSession
    {
        private readonly StreamSocket _clientSocket;

        public WebSocketClientSession(StreamSocket clientSocket)
        {
            if (clientSocket == null) throw new ArgumentNullException(nameof(clientSocket));

            _clientSocket = clientSocket;
        }

        public async Task WaitForDataAsync()
        {
            var buffer = new Buffer(16*1024);
            var data = await _clientSocket.InputStream.ReadAsync(buffer, 1024, InputStreamOptions.Partial);

            Debug.WriteLine("Received something!?");
        }

        public async Task SendAsync(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var frame = WebSocketFrame.Create(data);
            var frameBuffer = frame.ToByteArray().AsBuffer();

            await _clientSocket.OutputStream.WriteAsync(frameBuffer);
            await _clientSocket.OutputStream.FlushAsync();
        }
    }
}
