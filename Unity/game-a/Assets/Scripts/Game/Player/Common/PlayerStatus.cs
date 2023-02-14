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
        private ReactiveProperty<float> currentSpeedProperty;
        private ReactiveProperty<Vector3> positionProperty;
        private ReactiveProperty<Quaternion> rotationProperty;
        private ReactiveProperty<string> characterAddressProperty;

        // --- プロパティ --- //
        public float CurrentSpeed => currentSpeedProperty.Value;
        public Vector3 Position => positionProperty.Value;
        public Quaternion Rotation => rotationProperty.Value;
        public string CharacterAddress => characterAddressProperty.Value;

        // --- イベント --- //
        public IReadOnlyReactiveProperty<float> OnUpdateCurrentSpeed => currentSpeedProperty;
        public IReadOnlyReactiveProperty<Vector3> OnUpdatePosition => positionProperty;
        public IReadOnlyReactiveProperty<Quaternion> OnUpdateRotation => rotationProperty;
        public IReadOnlyReactiveProperty<string> OnUpdateCharacterAddress => characterAddressProperty;

        [Inject]
        public PlayerStatus()
        {
            currentSpeedProperty = new ReactiveProperty<float>();
            positionProperty = new ReactiveProperty<Vector3>();
            rotationProperty = new ReactiveProperty<Quaternion>();
            characterAddressProperty = new ReactiveProperty<string>("empty");
        }

        /// <summary>
        /// 現在の移動速度を設定
        /// </summary>
        public void SetCurrentSpeed(float currentSpeed)
        {
            currentSpeedProperty.Value = currentSpeed;
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

        /// <summary>
        /// キャラクターのモデルのAAS上のアドレスを設定
        /// </summary>
        public void SetCharacterAddress(string address)
        {
            characterAddressProperty.Value = address;
        }
    }
}