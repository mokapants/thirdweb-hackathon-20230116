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
        private readonly Quaternion rotation;
        private static readonly string PositionKey = "pos";
        private static readonly string RotationKey = "rot";

        // --- プロパティ --- //
        public Vector3 Position => position;
        public Quaternion Rotation => rotation;

        private WSMoveAction(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        /// <summary>
        /// データを文字列にまとめて変換
        /// </summary>
        public static string ConvertToString(Vector3 pos, Quaternion rot)
        {
            // pos:3.15-0.00-12.28,rot:1.01-4.32-54.38-0.24
            return $"{PositionKey}:{pos.x:0.00}-{pos.y:0.00}-{pos.z:0.00},{RotationKey}:{rot.x:0.00}-{rot.y:0.00}-{rot.z:0.00}-{rot.w:0.00}";
        }

        /// <summary>
        /// 文字列からデータを抽出
        /// </summary>
        public static WSMoveAction FromString(string value)
        {
            // 正しいデータか確認
            var regex = new Regex($"{PositionKey}" + @":\d+.\d{2}-\d+.\d{2}-\d+.\d{2}," + $"{RotationKey}" + @":\d+.\d{2}-\d+.\d{2}-\d+.\d{2}-\d+.\d{2}");
            var isValidData = regex.Match(value).Success;
            if (!isValidData)
            {
                throw new InvalidDataException($"WSMoveAction: Invalid data={value}");
            }

            // ,の位置を取得
            var commaIndex = value.IndexOf(',');

            // Positionの抽出
            var positionString = value.Substring(0, commaIndex);
            var positionValue = positionString.Replace(PositionKey + ":", "");
            var positionArray = positionValue.Split('-');
            var x = float.Parse(positionArray[0]);
            var y = float.Parse(positionArray[1]);
            var z = float.Parse(positionArray[2]);
            var position = new Vector3(x, y, z);

            // Rotationの抽出
            var rotationString = value.Substring(commaIndex + 1);
            var rotationValue = rotationString.Replace(RotationKey + ":", "");
            var rotationArray = rotationValue.Split('-');
            x = float.Parse(rotationArray[0]);
            y = float.Parse(rotationArray[1]);
            z = float.Parse(rotationArray[2]);
            var w = float.Parse(rotationArray[3]);
            var rotation = new Quaternion(x, y, z, w);

            return new WSMoveAction(position, rotation);
        }
    }
}