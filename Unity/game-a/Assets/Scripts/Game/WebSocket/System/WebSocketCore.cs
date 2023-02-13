using System;
using Cysharp.Threading.Tasks;
using Game.WebSocket.Interfaces;
using NativeWebSocket;
using UnityEngine;
using VContainer;

namespace Game.WebSocket
{
    /// <summary>
    /// WebSocketの送受信を担当
    /// </summary>
    public class WebSocketCore : IUserWebSocketCore, IDisposable
    {
        // WebSocketのイベント受信時の制御
        private WebSocketEvents webSocketEvents;
        // WebSocketの送受信に必要なデータやインスタンス群
        private WebSocketData webSocketData;

        [Inject]
        public WebSocketCore(WebSocketEvents webSocketEvents, WebSocketData webSocketData)
        {
            this.webSocketEvents = webSocketEvents;
            this.webSocketData = webSocketData;
        }

        public async void Start()
        {
            // 接続
            await Connect();
        }

        public void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            webSocketData.WebSocket.DispatchMessageQueue();
#endif
        }

        /// <summary>
        /// 接続
        /// </summary>
        private async UniTask Connect()
        {
            var webSocket = new NativeWebSocket.WebSocket(webSocketData.WebSocketUrl);
            webSocketData.SetWebsocket(webSocket);

            // イベントを登録
            webSocketData.WebSocket.OnOpen += webSocketEvents.OnOpen;
            webSocketData.WebSocket.OnError += webSocketEvents.OnError;
            webSocketData.WebSocket.OnClose += webSocketEvents.OnClose;
            webSocketData.WebSocket.OnMessage += webSocketEvents.OnMessage;
            // セッション開通時にAchexを初期化するためのパケットを送信
            webSocketData.WebSocket.OnOpen += () => SendMsgForInitAchexAsync().Forget();

            await webSocketData.WebSocket.Connect();
        }

        /// <summary>
        /// メッセージを送信
        /// </summary>
        public async UniTask SendAsync(string msg)
        {
            if (webSocketData.WebSocket.State != WebSocketState.Open)
            {
                Utils.Debug.Log("WebSocket: WebSocket is closed");
                return;
            }

            if (!webSocketData.IsInitializedAchex)
            {
                Utils.Debug.Log("WebSocket: WebSocket not initialized");
                return;
            }

            Utils.Debug.Log($"WebSocket: Send message = {msg}");

            // 送信
            var json = webSocketData.SendPacketAchex.ToJsonWithMessage(msg);
            await webSocketData.WebSocket.SendText(json);
        }

        /// <summary>
        /// Achexの初期化用メッセージを送信
        /// </summary>
        public async UniTask SendMsgForInitAchexAsync()
        {
            if (webSocketData.WebSocket.State != WebSocketState.Open)
            {
                Utils.Debug.Log("WebSocket: WebSocket is closed");
                return;
            }

            var json = new PacketInitializeAchex(webSocketData.AchexId, webSocketData.AchexPassword).ToJson();

            Utils.Debug.Log($"WebSocket: Initialize Achex = {json}");
            await webSocketData.WebSocket.SendText(json);
        }

        public async void Dispose()
        {
            await webSocketData.WebSocket.Close();
        }
    }
}