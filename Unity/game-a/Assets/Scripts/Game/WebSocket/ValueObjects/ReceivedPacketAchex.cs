using System.IO;
using Newtonsoft.Json.Linq;

namespace Game.WebSocket
{
    /// <summary>
    /// Achexでメッセージを受信する際のデータ構造
    /// </summary>
    public class ReceivedPacketAchex
    {
        // --- メンバ変数 --- //
        private readonly string to;
        private readonly string message;
        private readonly string from;
        private readonly string sessionId;
        
        // --- プロパティ --- //
        public string To => to;
        public string Message => message;
        public string From => from;
        public string SessionId => sessionId;

        private ReceivedPacketAchex(string to, string message, string from, string sessionId)
        {
            this.to = to;
            this.message = message;
            this.from = from;
            this.sessionId = sessionId;
        }

        /// <summary>
        /// 適切なJsonであるかを判定し、適切であればデータ構造を用意
        /// </summary>
        public static bool TryParseFromJson(string json, out ReceivedPacketAchex value)
        {
            // Json文字列の中に必要なキーが含まれているかチェック
            var jsonObject = JObject.Parse(json);
            var isExistsToKey = jsonObject.TryGetValue("to", out var to);
            var isExistsMsgKey = jsonObject.TryGetValue("msg", out var message);
            var isExistsFromKey = jsonObject.TryGetValue("FROM", out var from);
            var isExistsSIdKey = jsonObject.TryGetValue("sID", out var sessionId);
            if (!(isExistsToKey && isExistsMsgKey && isExistsFromKey && isExistsSIdKey))
            {
                Utils.Debug.Log($"WebSocket: Invalid data from WebSocket = {json}");
                value = null;
                return false;
            }

            // 適切なJsonである場合は適切なデータ構造を用意
            value = new ReceivedPacketAchex(to.ToString(), message.ToString(), from.ToString(), sessionId.ToString());
            return true;
        }
    }
}