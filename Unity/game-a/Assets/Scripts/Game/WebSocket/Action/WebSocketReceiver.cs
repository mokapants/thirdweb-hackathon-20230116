using System;
using System.Collections.Generic;
using Game.WebSocket.Hub.Action;
using Game.WebSocket.Hub.Actions;
using Game.WebSocket.Hub.ValueObjects;
using Game.WebSocket.Interfaces;
using UniRx;
using UnityEngine;
using VContainer;

namespace Game.WebSocket.Hub
{
    public class WebSocketReceiver : MonoBehaviour
    {
        // --- メンバ変数 --- //
        // WebSocket関連のイベントを通知
        private IUserWebSocketEvents webSocketEvents;
        // 各アクションのイベント(セッションID, 独自のデータ)
        private static Subject<(string, WSPlayerStateAction)> receivedPlayerStateActionSubject;

        // --- イベント --- //
        public static IObservable<(string, WSPlayerStateAction)> OnReceivedPlayerStateAction => receivedPlayerStateActionSubject;

        [Inject]
        public void Inject(IUserWebSocketEvents webSocketEvents)
        {
            this.webSocketEvents = webSocketEvents;
        }

        private void Awake()
        {
            receivedPlayerStateActionSubject = new Subject<(string, WSPlayerStateAction)>();
        }

        private void Start()
        {
            webSocketEvents.OnMessageEvent.Subscribe(OnReceivedAnyData).AddTo(this);
        }

        private void OnReceivedAnyData(ReceivedPacketAchex receivedPacketAchex)
        {
            // データを変換
            ActionSyncData actionSyncData;
            try
            {
                actionSyncData = ActionSyncData.FromString(receivedPacketAchex.Message);
            }
            catch (Exception e)
            {
                Utils.Debug.LogError(e);
                throw;
            }

            // 各アクションに処理を任せる
            switch (actionSyncData.Action)
            {
                case WebSocketAction.State:
                    var wsMoveAction = WSPlayerStateAction.FromString(actionSyncData.Data);
                    receivedPlayerStateActionSubject.OnNext((receivedPacketAchex.SessionId, wsMoveAction));
                    break;
            }
        }
    }
}