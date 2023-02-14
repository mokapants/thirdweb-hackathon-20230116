using Game.Player;
using Game.Player.Actions;
using Game.Player.Actions.Common;
using Game.Player.ControllablePlayer;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LifetimeScopes
{
    public class ControllablePlayerLifetimeScope : LifetimeScope
    {
        // --- メンバ変数 --- //
        [SerializeField] private ControllablePlayerMoveAction controllablePlayerMoveAction;
        [SerializeField] private PlayerCharacterAction playerCharacterAction;
        [SerializeField] private PlayerAnimationAction playerAnimationAction;
        // 操作可能プレイヤー限定
        [SerializeField] private PlayerControl playerControl;
        [SerializeField] private PlayerSync playerSync;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(controllablePlayerMoveAction).AsImplementedInterfaces();
            builder.RegisterComponent(playerCharacterAction);
            builder.RegisterComponent(playerAnimationAction);
            builder.RegisterEntryPoint<PlayerAction>();
            builder.Register<PlayerStatus>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.RegisterComponent(playerControl);
            builder.RegisterComponent(playerSync);
        }
    }
}