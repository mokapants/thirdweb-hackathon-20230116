﻿using Game.WebSocket;
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
            builder.Register<WebSocketCore>(Lifetime.Singleton);
            builder.Register<WebSocketEvents>(Lifetime.Singleton);
            builder.Register<WebSocketData>(Lifetime.Singleton);
        }
    }
}