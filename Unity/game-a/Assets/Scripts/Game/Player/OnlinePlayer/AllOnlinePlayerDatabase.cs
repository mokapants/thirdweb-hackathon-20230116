using System;
using System.Collections.Generic;
using Game.Player.Interfaces;
using Game.WebSocket.Hub.Actions;
using LifetimeScopes;
using UnityEngine;
using VContainer;

namespace Game.Core
{
    /// <summary>
    /// オンラインプレイヤーを管理するためのデータベース
    /// </summary>
    public class AllOnlinePlayerDatabase : MonoBehaviour
    {
        // --- メンバ変数 --- //
        // オンラインプレイヤーのプレハブ
        [SerializeField] private PlayerLifetimeScope playerPrefab;
        // オンラインプレイヤーのテーブル(セッションID, データ用クラス)
        private Dictionary<string, OnlinePlayerData> database;

        // --- プロパティ --- //
        public IDictionary<string, OnlinePlayerData> Database => database;

        private void Awake()
        {
            database = new Dictionary<string, OnlinePlayerData>();
        }

        /// <summary>
        /// プレイヤーを新たに追加
        /// </summary>
        public void AddPlayer(string sessionId, WSMoveAction moveAction)
        {
            var playerLifetimeScope = Instantiate(playerPrefab, transform);
            var playerGameObject = playerLifetimeScope.gameObject;
            var playerStatus = playerLifetimeScope.Container.Resolve<IPlayerStatus>();
            var onlinePlayerData = new OnlinePlayerData(sessionId, playerGameObject, playerStatus);
            database.Add(sessionId, onlinePlayerData);
            
            UpdateMoveData(sessionId, moveAction);
        }

        /// <summary>
        /// プレイヤーを削除
        /// </summary>
        public void RemovePlayer(string sessionId)
        {
            Destroy(database[sessionId].GameObject);
            database.Remove(sessionId);
        }

        /// <summary>
        /// 特定のプレイヤーの存在を確認する
        /// </summary>
        public bool IsExistsPlayer(string sessionId)
        {
            return database.ContainsKey(sessionId);
        }

        /// <summary>
        /// 特定のプレイヤーのデータを取得する
        /// </summary>
        public void UpdateMoveData(string sessionId, WSMoveAction moveAction)
        {
            var onlinePlayerData = database[sessionId];
            onlinePlayerData.Status.SetPosition(moveAction.Position);
            onlinePlayerData.Status.SetRotation(moveAction.Rotation);
            onlinePlayerData.UpdateReceivedDataUnixtime();
        }
    }
}