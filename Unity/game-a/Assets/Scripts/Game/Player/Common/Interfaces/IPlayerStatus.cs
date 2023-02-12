using UniRx;
using UnityEngine;

namespace Game.Player.Interfaces
{
    /// <summary>
    /// プレイヤーのステータス情報用インターフェース
    /// </summary>
    public interface IPlayerStatus
    {
        // --- プロパティ --- //
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        
        // --- イベント --- //
        public IReadOnlyReactiveProperty<Vector3> OnUpdatePosition { get; }
        public IReadOnlyReactiveProperty<Quaternion> OnUpdateRotation { get; }

        /// <summary>
        /// プレイヤーの位置を設定
        /// </summary>
        public void SetPosition(Vector3 position);

        /// <summary>
        /// プレイヤーの向きを設定
        /// </summary>
        public void SetRotation(Quaternion rotation);
    }
}