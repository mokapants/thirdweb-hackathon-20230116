namespace Game.WebSocket
{
    /// <summary>
    /// Achex初期化時に送信するパケットのデータ構造
    /// https://achex.ca/dev/
    /// </summary>
    public class PacketInitializeAchex
    {
        // --- メンバ変数 --- //
        private readonly string id;
        private readonly string password;

        public PacketInitializeAchex(string id, string password)
        {
            this.id = id;
            this.password = password;
        }
        
        /// <summary>
        /// Jsonを生成
        /// </summary>
        public string ToJson()
        {
            return $@"{{""setID"":""{id}"",""passwd"":""{password}""}}";
        }
    }
}