using System;
using Game.Player.Interfaces;
using UnityEngine;

namespace Game.Core
{
    public class OnlinePlayerData
    {
        // --- メンバ変数 --- //
        // セッションID
        private readonly string sessionId;
        // プレイヤーのオブジェクト
        private readonly GameObject gameObject;
        // ステータス
        private readonly IPlayerStatus status;
        // 最後に情報を受け取った時間
        private long lastReceivedDataUnixtime;

        // --- プロパティ --- //
        public string SessionId => sessionId;
        public GameObject GameObject => gameObject;
        public IPlayerStatus Status => status;
        public long LastReceivedDataUnixtime => lastReceivedDataUnixtime;
        
        public OnlinePlayerData(string sessionId, GameObject gameObject, IPlayerStatus status)
        {
            this.sessionId = sessionId;
            this.gameObject = gameObject;
            this.status = status;
        }

        /// <summary>
        /// 最後にデータを受信した時刻を更新
        /// </summary>
        public void UpdateReceivedDataUnixtime()
        {
            lastReceivedDataUnixtime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        }
    }
}