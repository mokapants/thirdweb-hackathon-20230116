using System;
using NativeWebSocket;
using UniRx;

namespace Game.WebSocket.Interfaces
{
    public interface IUserWebSocketEvents
    {
        // --- プロパティ --- //
        public IObservable<Unit> OnOpenEvent { get; }
        public IObservable<string> OnErrorEvent { get; }
        public IObservable<WebSocketCloseCode> OnCloseEvent { get; }
        public IObservable<ReceivedPacketAchex> OnMessageEvent { get; }
    }
}