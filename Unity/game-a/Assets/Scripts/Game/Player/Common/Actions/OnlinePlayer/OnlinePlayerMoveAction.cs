using Game.Player.Actions;
using UnityEngine;

namespace Game.Player.Actions
{
    public class OnlinePlayerMoveAction : MonoBehaviour, IPlayerMoveAction
    {
        // --- メンバ変数 --- //
        [SerializeField] private Transform playerTransform;

        /// <summary>
        /// 位置を更新
        /// </summary>
        public void UpdatePosition(Vector3 position)
        {
            playerTransform.position = position;
        }

        /// <summary>
        /// 向きを更新
        /// </summary>
        public void UpdateRotation(Quaternion rotation)
        {
            playerTransform.rotation = rotation;
        }
    }
}