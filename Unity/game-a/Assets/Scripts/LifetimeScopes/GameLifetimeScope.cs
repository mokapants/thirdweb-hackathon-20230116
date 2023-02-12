using Game.Core;
using Game.WebSocket;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LifetimeScopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // WebSocket関連
            builder.RegisterEntryPoint<WebSocketMonoBehaviour>();
            builder.Register<WebSocketCore>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<WebSocketEvents>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<WebSocketData>(Lifetime.Singleton);
            
            // オンラインプレイヤー関連
            builder.RegisterComponentOnNewGameObject<AllOnlinePlayerDatabase>(Lifetime.Singleton, "AllOnlinePlayers");
            builder.Register<AllOnlinePlayerManager>(Lifetime.Singleton);
        }
    }
}