using System;
using Cysharp.Threading.Tasks;
using Game.Player.Actions;
using Game.Player.Actions.Common;
using Game.Player.Interfaces;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Player
{
    public class PlayerAction : IStartable, IDisposable
    {
        // --- メンバ変数 --- //
        // プレイヤーのステータス情報
        private IPlayerStatus playerStatus;
        // 各アクション
        private IPlayerMoveAction playerMoveAction;
        private PlayerCharacterAction playerCharacterAction;
        private PlayerAnimationAction playerAnimationAction;
        // 監視しているイベントを破棄するため
        private CompositeDisposable compositeDisposable;

        [Inject]
        public PlayerAction(IPlayerStatus playerStatus, IPlayerMoveAction playerMoveAction, PlayerCharacterAction playerCharacterAction, PlayerAnimationAction playerAnimationAction)
        {
            this.playerStatus = playerStatus;
            this.playerMoveAction = playerMoveAction;
            this.playerCharacterAction = playerCharacterAction;
            this.playerAnimationAction = playerAnimationAction;

            compositeDisposable = new CompositeDisposable();
        }

        public void Start()
        {
            // ステータスの情報を元にアクションを実行
            playerStatus.OnUpdateCurrentSpeed.Subscribe(playerAnimationAction.SetCurrentSpeed).AddTo(compositeDisposable);
            playerStatus.OnUpdatePosition.Subscribe(playerMoveAction.UpdatePosition).AddTo(compositeDisposable);
            playerStatus.OnUpdateRotation.Subscribe(playerMoveAction.UpdateRotation).AddTo(compositeDisposable);
            playerStatus.OnUpdateCharacterAddress.Subscribe(address => UpdateCharacterAddress(address).Forget()).AddTo(compositeDisposable);
        }

        /// <summary>
        /// キャラクターを切り替えたとき
        /// </summary>
        private async UniTask UpdateCharacterAddress(string address)
        {
            var animator = await playerCharacterAction.UpdateCharacterAddress(address);
            playerAnimationAction.SetAnimator(animator);
        }

        public void Dispose()
        {
            compositeDisposable?.Dispose();
        }
    }
}