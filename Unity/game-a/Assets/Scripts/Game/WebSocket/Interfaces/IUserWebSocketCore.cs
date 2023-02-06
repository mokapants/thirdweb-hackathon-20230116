using Cysharp.Threading.Tasks;

namespace Game.WebSocket.Interfaces
{
    public interface IUserWebSocketCore
    {
        /// <summary>
        /// メッセージ送信
        /// </summary>
        public UniTask SendAsync(string msg);
    }
}