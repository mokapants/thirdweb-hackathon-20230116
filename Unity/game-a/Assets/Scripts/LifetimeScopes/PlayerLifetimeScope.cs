using Game.Player;
using Game.Player.Actions;
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

            builder.RegisterEntryPoint<PlayerAction>();
            builder.Register<PlayerStatus>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}