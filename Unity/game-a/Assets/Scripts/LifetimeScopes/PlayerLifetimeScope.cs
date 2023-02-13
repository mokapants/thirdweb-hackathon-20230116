using Game.Player;
using Game.Player.Actions;
using Game.Player.Actions.Common;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LifetimeScopes
{
    public class PlayerLifetimeScope : LifetimeScope
    {
        // --- メンバ変数 --- //
        // 操作可能なプレイヤーかオンラインプレイヤーでインスタンスが異なる
        [SerializeField] private bool isControllable;
        [SerializeField] private ControllablePlayerMoveAction controllablePlayerMoveAction;
        [SerializeField] private OnlinePlayerMoveAction onlinePlayerMoveAction;
        // 共通のインスタンス
        [SerializeField] private PlayerCharacterAction playerCharacterAction;
        [SerializeField] private PlayerAnimationAction playerAnimationAction;

        protected override void Configure(IContainerBuilder builder)
        {
            if (isControllable)
            {
                builder.RegisterComponent(controllablePlayerMoveAction).AsImplementedInterfaces();
                onlinePlayerMoveAction.enabled = false;
            }
            else
            {
                builder.RegisterComponent(onlinePlayerMoveAction).AsImplementedInterfaces();
                controllablePlayerMoveAction.enabled = false;
            }
            builder.RegisterComponent(playerCharacterAction);
            builder.RegisterComponent(playerAnimationAction);
            builder.RegisterEntryPoint<PlayerAction>();
            builder.Register<PlayerStatus>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}