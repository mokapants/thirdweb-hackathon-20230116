using Game.Player.Interfaces;
using UniRx;
using UnityEngine;
using VContainer;

namespace Game.Player
{
    /// <summary>
    /// プレイヤーのステータス情報を管理
    /// </summary>
    public class PlayerStatus : IPlayerStatus
    {
        // --- メンバ変数 --- //
        private ReactiveProperty<Vector3> positionProperty;
        private ReactiveProperty<Quaternion> rotationProperty;

        // --- プロパティ --- //
        public Vector3 Position => positionProperty.Value;
        public Quaternion Rotation => rotationProperty.Value;

        // --- イベント --- //
        public IReadOnlyReactiveProperty<Vector3> OnUpdatePosition => positionProperty;
        public IReadOnlyReactiveProperty<Quaternion> OnUpdateRotation => rotationProperty;

        [Inject]
        public PlayerStatus()
        {
            positionProperty = new ReactiveProperty<Vector3>();
            rotationProperty = new ReactiveProperty<Quaternion>();
        }

        /// <summary>
        /// プレイヤーの位置を設定
        /// </summary>
        public void SetPosition(Vector3 position)
        {
            positionProperty.Value = position;
        }

        /// <summary>
        /// プレイヤーの向きを設定
        /// </summary>
        public void SetRotation(Quaternion rotation)
        {
            rotationProperty.Value = rotation;
        }
    }
}