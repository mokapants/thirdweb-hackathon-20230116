using System;
using Game.WebSocket.Interfaces;
using NativeWebSocket;
using UniRx;
using VContainer;

namespace Game.WebSocket
{
    public class WebSocketEvents : IUserWebSocketEvents
    {
        // --- メンバ変数 --- //
        // WebSocketの送受信に必要なデータやインスタンス群
        private WebSocketData webSocketData;
        // 各種イベント用
        private Subject<Unit> openSubject = new Subject<Unit>();
        private Subject<string> errorSubject = new Subject<string>();
        private Subject<WebSocketCloseCode> closeSubject = new Subject<WebSocketCloseCode>();
        private Subject<ReceivedPacketAchex> messageSubject = new Subject<ReceivedPacketAchex>();

        // --- イベント --- //
        public IObservable<Unit> OnOpenEvent => openSubject;
        public IObservable<string> OnErrorEvent => errorSubject;
        public IObservable<WebSocketCloseCode> OnCloseEvent => closeSubject;
        public IObservable<ReceivedPacketAchex> OnMessageEvent => messageSubject;

        [Inject]
        public WebSocketEvents(WebSocketData webSocketData)
        {
            this.webSocketData = webSocketData;
        }

        /// <summary>
        /// 疎通したとき
        /// </summary>
        public void OnOpen()
        {
            Utils.Debug.Log("WebSocket: Open");
            openSubject.OnNext(Unit.Default);
        }

        /// <summary>
        /// エラーが発生したとき
        /// </summary>
        public void OnError(string error)
        {
            Utils.Debug.Log($"WebSocket: Error\n{error}");
            errorSubject.OnNext(error);
        }

        /// <summary>
        /// 切断されたとき
        /// </summary>
        public void OnClose(WebSocketCloseCode code)
        {
            Utils.Debug.Log($"WebSocket: Close\n{code}");
            closeSubject.OnNext(code);
        }

        /// <summary>
        /// メッセージを受信したとき
        /// </summary>
        public void OnMessage(byte[] bytes)
        {
            var json = System.Text.Encoding.UTF8.GetString(bytes);
            Utils.Debug.Log($"WebSocket: Message\n{json}");

            if (json == @"{""auth"":""ok""}")
            {
                // Achexの初期化が完了
                webSocketData.OnInitializedAchex();
                return;
            }

            var isValidJson = ReceivedPacketAchex.TryParseFromJson(json, out var receivedPacketAchex);
            if (!isValidJson) return;
            
            messageSubject.OnNext(receivedPacketAchex);
        }
    }
}