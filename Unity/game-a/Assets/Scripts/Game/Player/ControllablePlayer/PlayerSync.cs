using System;
using Cysharp.Threading.Tasks;
using Game.Player.Interfaces;
using Game.WebSocket;
using Game.WebSocket.Hub.Action;
using Game.WebSocket.Hub.Actions;
using UniRx;
using UnityEngine;
using VContainer;

namespace Game.Player.ControllablePlayer
{
    public class PlayerSync : MonoBehaviour
    {
        // --- メンバ変数 --- //
        // キャラクターの位置情報反映用
        private IPlayerStatus playerStatus;
        // データの送信感覚
        [SerializeField] private float sendMoveDataSpanMilliSec;
        // 前回送信したときのデータ
        private float prevCurrentSpeed;
        private Vector3 prevPosition;
        private Quaternion prevRotation;

        [Inject]
        public void Inject(IPlayerStatus playerStatus)
        {
            this.playerStatus = playerStatus;
        }

        private void Start()
        {
            IntervalSendMoveData();
        }

        /// <summary>
        /// 定期的に位置・向きの情報の送信を試みる
        /// </summary>
        private void IntervalSendMoveData()
        {
            Observable.Interval(TimeSpan.FromMilliseconds(sendMoveDataSpanMilliSec))
                .Subscribe(_ => SendMoveDataIfNeededAsync().Forget()).AddTo(this);
        }

        /// <summary>
        /// 位置・向きの情報に変更があれば送信する
        /// </summary>
        private async UniTask SendMoveDataIfNeededAsync()
        {
            var moveActionString = WSPlayerStateAction.ConvertToString(playerStatus.CurrentSpeed, playerStatus.CharacterAddress, playerStatus.Position, playerStatus.Rotation);
            await WebSocketSender.SendAsync(WebSocketAction.State, moveActionString);

            prevCurrentSpeed = playerStatus.CurrentSpeed;
            prevPosition = playerStatus.Position;
            prevRotation = playerStatus.Rotation;
        }
    }
}