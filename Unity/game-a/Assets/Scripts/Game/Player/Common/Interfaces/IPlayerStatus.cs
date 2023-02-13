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
        public float CurrentSpeed { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public string CharacterAddress { get; }
        
        // --- イベント --- //
        public IReadOnlyReactiveProperty<float> OnUpdateCurrentSpeed { get; }
        public IReadOnlyReactiveProperty<Vector3> OnUpdatePosition { get; }
        public IReadOnlyReactiveProperty<Quaternion> OnUpdateRotation { get; }
        public IReadOnlyReactiveProperty<string> OnUpdateCharacterAddress { get; }

        /// <summary>
        /// 現在の移動速度を設定
        /// </summary>
        public void SetCurrentSpeed(float currentSpeed);

        /// <summary>
        /// プレイヤーの位置を設定
        /// </summary>
        public void SetPosition(Vector3 position);

        /// <summary>
        /// プレイヤーの向きを設定
        /// </summary>
        public void SetRotation(Quaternion rotation);

        /// <summary>
        /// キャラクターのモデルのAAS上のアドレスを設定
        /// </summary>
        public void SetCharacterAddress(string address);
    }
}