using Game.WebSocket.Interfaces;
using UnityEngine;
using VContainer;
using Debug = Utils.Debug;

namespace Game.WebSocket
{
    /// <summary>
    /// WebSocket関連を利用するためのクラス
    /// </summary>
    public class WebSocketClient : MonoBehaviour
    {
        // --- メンバ変数 --- //
        private static WebSocketCore webSocketCore;
        private static WebSocketEvents webSocketEvents;

        // --- プロパティ --- //
        public static IUserWebSocketCore Core => webSocketCore;
        public static IUserWebSocketEvents Events => webSocketEvents;

        [Inject]
        public void Inject(WebSocketCore webSocketCore, WebSocketEvents webSocketEvents)
        {
            WebSocketClient.webSocketCore = webSocketCore;
            WebSocketClient.webSocketEvents = webSocketEvents;
        }
    }
}