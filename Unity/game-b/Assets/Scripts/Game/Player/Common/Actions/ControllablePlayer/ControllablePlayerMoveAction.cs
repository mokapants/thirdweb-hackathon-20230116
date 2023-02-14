using UnityEngine;

namespace Game.Player.Actions
{
    public class ControllablePlayerMoveAction : MonoBehaviour, IPlayerMoveAction
    {
        // --- メンバ変数 --- //
        // プレイヤーの位置情報を適用させるためのTransform
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