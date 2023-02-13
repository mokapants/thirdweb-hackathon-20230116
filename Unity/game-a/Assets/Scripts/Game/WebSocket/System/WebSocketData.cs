using Newtonsoft.Json.Linq;
using UnityEngine;
using VContainer;

namespace Game.WebSocket
{
    public class WebSocketData
    {
        // --- メンバ変数 --- //
        // WebSocketサーバーのURL
        // private readonly string webSocketUrl = "ws://achex.ca:4010";
        private readonly string webSocketUrl = "wss://cloud.achex.ca";
        // Achex内で利用するID&パスワード
        private readonly string achexId;
        private readonly string achexPassword;
        // WebSocketのインスタンス
        private NativeWebSocket.WebSocket webSocket;
        // パケット送信用のインスタンスのキャッシュ
        private SendPacketAchex sendPacketAchex;
        // 初期化完了しているか
        private bool isInitializedAchex;
        
        // --- プロパティ --- //
        public string WebSocketUrl => webSocketUrl;
        public string AchexId => achexId;
        public string AchexPassword => achexPassword;
        public NativeWebSocket.WebSocket WebSocket => webSocket;
        public SendPacketAchex SendPacketAchex => sendPacketAchex;
        public bool IsInitializedAchex => isInitializedAchex;

        [Inject]
        public WebSocketData()
        {
            // AchexのIDとPasswordを取得
            var keyJsonText = Resources.Load<TextAsset>("Key/Key").text;
            var jsonObject = JObject.Parse(keyJsonText);
            achexId = jsonObject["id"]?.ToString();
            achexPassword = jsonObject["password"]?.ToString();
            
            // 初期化
            sendPacketAchex = new SendPacketAchex(AchexId);
        }

        /// <summary>
        /// WebSocketを設定
        /// </summary>
        public void SetWebsocket(NativeWebSocket.WebSocket webSocket)
        {
            this.webSocket = webSocket;
        }

        /// <summary>
        /// 初期化完了時に呼ばれる
        /// </summary>
        public void OnInitializedAchex()
        {
            isInitializedAchex = true;
        }
    }
}