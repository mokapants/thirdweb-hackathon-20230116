using System;
using Game.Player.Actions;
using UnityEngine;

namespace Game.Player.Actions
{
    public class OnlinePlayerMoveAction : MonoBehaviour, IPlayerMoveAction
    {
        // --- メンバ変数 --- //
        // プレイヤーの位置情報を適用させるためのTransform
        [SerializeField] private Transform playerTransform;
        // 補間移動にかける時間
        [SerializeField] private int lerpMilliSec;
        // データを受け取ってからの経過時間
        private int elapsedTimeAfterReceivedData;
        // 以前・次の座標
        private Vector3 prevPosition;
        private Vector3 nextPosition;

        /// <summary>
        /// 位置を更新
        /// </summary>
        public void UpdatePosition(Vector3 position)
        {
            elapsedTimeAfterReceivedData = 0;
            prevPosition = playerTransform.position;
            nextPosition = position;
        }

        /// <summary>
        /// 向きを更新
        /// </summary>
        public void UpdateRotation(Quaternion rotation)
        {
            playerTransform.rotation = rotation;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            var deltaTimeMilliSec = (int)(Time.deltaTime * 1000);
            elapsedTimeAfterReceivedData += deltaTimeMilliSec;
            var time = (float)elapsedTimeAfterReceivedData / lerpMilliSec;
            playerTransform.position = Vector3.Lerp(prevPosition, nextPosition, time);
        }
    }
}