using Game.Player;
using Game.Player.Actions;
using Game.Player.Actions.Common;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LifetimeScopes
{
    public class OnlinePlayerLifetimeScope : LifetimeScope
    {
        // --- メンバ変数 --- //
        [SerializeField] private OnlinePlayerMoveAction onlinePlayerMoveAction;
        [SerializeField] private PlayerCharacterAction playerCharacterAction;
        [SerializeField] private PlayerAnimationAction playerAnimationAction;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(onlinePlayerMoveAction).AsImplementedInterfaces();
            builder.RegisterComponent(playerCharacterAction);
            builder.RegisterComponent(playerAnimationAction);
            builder.RegisterEntryPoint<PlayerAction>();
            builder.Register<PlayerStatus>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}