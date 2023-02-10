namespace Game.WebSocket
{
    /// <summary>
    /// Achexでメッセージを送信する際のデータ構造
    /// </summary>
    public class SendPacketAchex
    {
        // --- メンバ変数 --- //
        private readonly string to;

        public SendPacketAchex(string to)
        {
            this.to = to;
        }

        /// <summary>
        /// メッセージ付きのJsonを生成
        /// </summary>
        public string ToJsonWithMessage(string message)
        {
            return $@"{{""to"":""{to}"",""msg"":""{message}""}}";
        }
    }
}