using System;
using System.IO;
using System.Text.RegularExpressions;
using Game.WebSocket.Hub.Action;
using Game.WebSocket.Hub.ValueObjects;
using UnityEngine;

namespace Game.WebSocket.Hub.Actions
{
    public class WSMoveAction
    {
        // --- メンバ変数 --- //
        private readonly Vector3 position;
        private static readonly string PositionKey = "pos";

        // --- プロパティ --- //
        public Vector3 Position => position;

        private WSMoveAction(Vector3 position)
        {
            this.position = position;
        }

        /// <summary>
        /// データを文字列にまとめて変換
        /// </summary>
        public static string ConvertToString(Vector3 position)
        {
            // pos:3.15,0.00,12.28
            return $"{PositionKey}:{position.x:0.00},{position.y:0.00},{position.z:0.00}";
        }

        /// <summary>
        /// 文字列からデータを抽出
        /// </summary>
        public static WSMoveAction FromString(string value)
        {
            // 正しいデータか確認
            var regex = new Regex($"{PositionKey}" + @":\d+.\d{2},\d+.\d{2},\d+.\d{2}");
            var isValidData = regex.Match(value).Success;
            if (!isValidData)
            {
                throw new InvalidDataException($"WSMoveAction: Invalid data={value}");
            }

            // Positionの抽出
            var positionString = value.Replace(PositionKey + ":", "");
            var positionArray = positionString.Split(',');
            var x = float.Parse(positionArray[0]);
            var y = float.Parse(positionArray[1]);
            var z = float.Parse(positionArray[2]);
            var position = new Vector3(x, y, z);

            return new WSMoveAction(position);
        }
    }
}