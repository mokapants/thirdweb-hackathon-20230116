using System;
using Game.Player.Actions;
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
        // 監視しているイベントを破棄するため
        private CompositeDisposable compositeDisposable;

        [Inject]
        public PlayerAction(IPlayerStatus playerStatus, IPlayerMoveAction playerMoveAction)
        {
            this.playerStatus = playerStatus;
            this.playerMoveAction = playerMoveAction;
            
            compositeDisposable = new CompositeDisposable();
        }

        public void Start()
        {
            // ステータスの情報を元にアクションを実行
            playerStatus.OnUpdatePosition.Subscribe(playerMoveAction.UpdatePosition).AddTo(compositeDisposable);
            playerStatus.OnUpdateRotation.Subscribe(playerMoveAction.UpdateRotation).AddTo(compositeDisposable);
        }

        public void Dispose()
        {
            compositeDisposable?.Dispose();
        }
    }
}