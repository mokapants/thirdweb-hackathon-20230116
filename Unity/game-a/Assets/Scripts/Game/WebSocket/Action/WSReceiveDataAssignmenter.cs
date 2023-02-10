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
    public class WSReceiveDataAssignmenter : MonoBehaviour
    {
        // --- メンバ変数 --- //
        // WebSocket関連のイベントを通知
        private IUserWebSocketEvents webSocketEvents;
        // 各アクションのイベント
        private static Subject<WSMoveAction> receivedMoveActionSubject;

        // --- イベント --- //
        public static IObservable<WSMoveAction> OnReceivedMoveAction => receivedMoveActionSubject;

        [Inject]
        public void Inject(IUserWebSocketEvents webSocketEvents)
        {
            this.webSocketEvents = webSocketEvents;

            receivedMoveActionSubject = new Subject<WSMoveAction>();
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
                case WebSocketAction.Move:
                    var wsMoveAction = WSMoveAction.FromString(actionSyncData.Data);
                    receivedMoveActionSubject.OnNext(wsMoveAction);
                    break;
            }
        }
    }
}