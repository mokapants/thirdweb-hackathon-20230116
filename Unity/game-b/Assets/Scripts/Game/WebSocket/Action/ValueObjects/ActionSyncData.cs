using System;
using System.IO;
using System.Text.RegularExpressions;
using Game.WebSocket.Hub.Action;
using Utils;

namespace Game.WebSocket.Hub.ValueObjects
{
    public class ActionSyncData
    {
        // --- メンバ変数 --- //
        private readonly WebSocketAction action;
        private readonly string data;
        private static readonly string ActionKey = "action";
        private static readonly string DataKey = "data";
        
        // --- プロパティ --- //
        public WebSocketAction Action => action;
        public string Data => data;

        private ActionSyncData(WebSocketAction action, string data)
        {
            this.action = action;
            this.data = data;
        }

        /// <summary>
        /// データを文字列にまとめて変換
        /// </summary>
        public static string ConvertToString(WebSocketAction action, string data)
        {
            // action:Move,data:{any data}
            return $"{ActionKey}:{action.ToString()},{DataKey}:{data}";
        }

        /// <summary>
        /// 文字列からデータを抽出
        /// </summary>
        public static ActionSyncData FromString(string value)
        {
            // 正しいデータか確認
            var regex = new Regex($"{ActionKey}:.*{DataKey}:");
            var isValidData = regex.Match(value).Success;
            if (!isValidData)
            {
                throw new InvalidDataException($"ActionSyncData: Invalid data={value}");
            }

            // ,の位置を取得
            var commaIndex = value.IndexOf(',', ActionKey.Length);

            // Actionの抽出
            var actionString = value.Substring(0, commaIndex);
            var actionValue = actionString.Replace(ActionKey + ":", "");
            var action = (WebSocketAction)Enum.Parse(typeof(WebSocketAction), actionValue);

            // Dataの抽出
            var dataString = value.Substring(commaIndex + 1);
            var data = dataString.Replace(DataKey + ":", "");

            return new ActionSyncData(action, data);
        }
    }
}