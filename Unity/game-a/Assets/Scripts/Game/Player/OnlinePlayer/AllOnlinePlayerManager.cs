using System;
using Cysharp.Threading.Tasks;
using Game.WebSocket.Hub;
using Game.WebSocket.Hub.Actions;
using UniRx;
using UnityEngine;
using VContainer;

namespace Game.Core
{
    public class AllOnlinePlayerManager : MonoBehaviour
    {
        // --- メンバ変数 --- //
        // 全オンラインプレイヤーのデータベース
        private AllOnlinePlayerDatabase allOnlinePlayerDatabase;
        // データを受信しなくなってから削除するまでの時間
        [SerializeField] private int autoRemoveTimeMilliSec;

        [Inject]
        public void Inject(AllOnlinePlayerDatabase allOnlinePlayerDatabase)
        {
            this.allOnlinePlayerDatabase = allOnlinePlayerDatabase;
        }

        private void Start()
        {
            // 各イベントを監視
            WebSocketReceiver.OnReceivedMoveAction.Subscribe(OnReceiveMoveAction).AddTo(this);

            // 一定時間データを受信していない場合は削除
            IntervalAutoPlayerRemover();
        }

        /// <summary>
        /// 移動時のアクションを受信したとき
        /// </summary>
        private void OnReceiveMoveAction((string sessionId, WSMoveAction moveAction) data)
        {
            var isExistsPlayer = allOnlinePlayerDatabase.IsExistsPlayer(data.sessionId);
            if (isExistsPlayer)
            {
                allOnlinePlayerDatabase.UpdateMoveData(data.sessionId, data.moveAction);
            }
            else
            {
                allOnlinePlayerDatabase.AddPlayer(data.sessionId, data.moveAction);
            }
        }

        /// <summary>
        /// 定期的に削除対象のプレイヤーがいないか確認する
        /// </summary>
        private void IntervalAutoPlayerRemover()
        {
            Observable.Interval(TimeSpan.FromSeconds(1f)).Subscribe(RemovePlayerIfNeeded).AddTo(this);
        }

        /// <summary>
        /// 削除対象のプレイヤーが存在した場合削除する
        /// </summary>
        private void RemovePlayerIfNeeded(long _)
        {
            var currentUnixtime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            foreach (var onlinePlayerData in allOnlinePlayerDatabase.Database.Values)
            {
                var elapsedTimeAtLastReceived = currentUnixtime - onlinePlayerData.LastReceivedDataUnixtime;
                if (elapsedTimeAtLastReceived < autoRemoveTimeMilliSec) continue;

                allOnlinePlayerDatabase.RemovePlayer(onlinePlayerData.SessionId);
            }
        }
    }
}