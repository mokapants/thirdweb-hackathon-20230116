using UnityEngine;

namespace Game.Player.Actions
{
    public interface IPlayerMoveAction
    {
        /// <summary>
        /// 位置を更新
        /// </summary>
        public void UpdatePosition(Vector3 position);
        
        /// <summary>
        /// 向きを更新
        /// </summary>
        public void UpdateRotation(Quaternion rotation);
    }
}