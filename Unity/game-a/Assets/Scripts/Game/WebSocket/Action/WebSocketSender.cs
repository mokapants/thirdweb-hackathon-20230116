using Cysharp.Threading.Tasks;
using Game.WebSocket.Hub.Action;
using Game.WebSocket.Hub.ValueObjects;
using Game.WebSocket.Interfaces;
using UnityEngine;
using VContainer;
using Debug = Utils.Debug;

namespace Game.WebSocket
{
    /// <summary>
    /// WebSocket関連を利用するためのクラス
    /// </summary>
    public class WebSocketSender : MonoBehaviour
    {
        // --- メンバ変数 --- //
        // WS送信用
        private static IUserWebSocketCore webSocketCore;

        [Inject]
        public void Inject(IUserWebSocketCore webSocketCore)
        {
            WebSocketSender.webSocketCore = webSocketCore;
        }

        /// <summary>
        /// データを送信
        /// </summary>
        public static async UniTask SendAsync(WebSocketAction action, string data)
        {
            var actionSyncData = ActionSyncData.ConvertToString(action, data);
            await webSocketCore.SendAsync(actionSyncData);
        }
    }
}