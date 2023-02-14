using System;
using System.Collections.Generic;
using Game.Player.Interfaces;
using Game.WebSocket.Hub.Actions;
using LifetimeScopes;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private OnlinePlayerLifetimeScope onlinePlayerPrefab;
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
        public void AddPlayer(string sessionId, WSPlayerStateAction playerStateAction)
        {
            var playerLifetimeScope = Instantiate(onlinePlayerPrefab, transform);
            var playerGameObject = playerLifetimeScope.gameObject;
            var playerStatus = playerLifetimeScope.Container.Resolve<IPlayerStatus>();
            var onlinePlayerData = new OnlinePlayerData(sessionId, playerGameObject, playerStatus);
            database.Add(sessionId, onlinePlayerData);

            UpdatePlayerStateData(sessionId, playerStateAction);
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
        public void UpdatePlayerStateData(string sessionId, WSPlayerStateAction playerStateAction)
        {
            var onlinePlayerData = database[sessionId];
            onlinePlayerData.Status.SetCurrentSpeed(playerStateAction.CurrentSpeed);
            onlinePlayerData.Status.SetCharacterAddress(playerStateAction.CharacterTokenId);
            onlinePlayerData.Status.SetPosition(playerStateAction.Position);
            onlinePlayerData.Status.SetRotation(playerStateAction.Rotation);
            onlinePlayerData.UpdateReceivedDataUnixtime();
        }
    }
}