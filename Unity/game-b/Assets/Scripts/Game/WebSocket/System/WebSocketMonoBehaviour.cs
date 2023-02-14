using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.WebSocket
{
    /// <summary>
    /// WebSocket用のMonoBehaviour系のイベント
    /// </summary>
    public class WebSocketMonoBehaviour : IStartable, ITickable
    {
        private WebSocketCore webSocketCore;

        [Inject]
        public WebSocketMonoBehaviour(WebSocketCore webSocketCore)
        {
            this.webSocketCore = webSocketCore;
        }

        public void Start()
        {
            webSocketCore.Start();
        }

        public void Tick()
        {
            webSocketCore.Update();
        }
    }
}