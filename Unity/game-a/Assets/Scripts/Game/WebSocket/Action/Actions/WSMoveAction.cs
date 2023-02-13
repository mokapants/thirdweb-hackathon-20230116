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
        private readonly float currentSpeed;
        private readonly Vector3 position;
        private readonly Quaternion rotation;
        private static readonly string CurrentSpeedKey = "spe";
        private static readonly string PositionKey = "pos";
        private static readonly string RotationKey = "rot";

        // --- プロパティ --- //
        public float CurrentSpeed => currentSpeed;
        public Vector3 Position => position;
        public Quaternion Rotation => rotation;

        private WSMoveAction(float currentSpeed, Vector3 position, Quaternion rotation)
        {
            this.currentSpeed = currentSpeed;
            this.position = position;
            this.rotation = rotation;
        }

        /// <summary>
        /// データを文字列にまとめて変換
        /// </summary>
        public static string ConvertToString(float spe, Vector3 pos, Quaternion rot)
        {
            // spe:1.24,pos:3.15=0.00=12.28,rot:1.01=4.32=54.38=0.24
            return $"{CurrentSpeedKey}:{spe:0.00},{PositionKey}:{pos.x:0.00}={pos.y:0.00}={pos.z:0.00},{RotationKey}:{rot.x:0.00}={rot.y:0.00}={rot.z:0.00}={rot.w:0.00}";
        }

        /// <summary>
        /// 文字列からデータを抽出
        /// </summary>
        public static WSMoveAction FromString(string value)
        {
            // 正しいデータか確認
            var regex = new Regex($"{CurrentSpeedKey}" + @":(-?[0-9]+).\d{2}," + $"{PositionKey}" + @":(-?[0-9]+).\d{2}=(-?[0-9]+).\d{2}=(-?[0-9]+).\d{2}," + $"{RotationKey}" + @":(-?[0-9]+).\d{2}=(-?[0-9]+).\d{2}=(-?[0-9]+).\d{2}");
            var isValidData = regex.Match(value).Success;
            if (!isValidData)
            {
                throw new InvalidDataException($"WSMoveAction: Invalid data={value}");
            }

            // ,の位置を取得
            var splitData = value.Split(',');
            
            // CurrentSpeedの抽出
            var currentSpeedString = splitData[0];
            var currentSpeedValue = currentSpeedString.Replace(CurrentSpeedKey + ":", "");
            var currentSpeed = float.Parse(currentSpeedValue);

            // Positionの抽出
            var positionString = splitData[1];
            var positionValue = positionString.Replace(PositionKey + ":", "");
            var positionArray = positionValue.Split('=');
            var x = float.Parse(positionArray[0]);
            var y = float.Parse(positionArray[1]);
            var z = float.Parse(positionArray[2]);
            var position = new Vector3(x, y, z);

            // Rotationの抽出
            var rotationString = splitData[2];
            var rotationValue = rotationString.Replace(RotationKey + ":", "");
            var rotationArray = rotationValue.Split('=');
            x = float.Parse(rotationArray[0]);
            y = float.Parse(rotationArray[1]);
            z = float.Parse(rotationArray[2]);
            var w = float.Parse(rotationArray[3]);
            var rotation = new Quaternion(x, y, z, w);

            return new WSMoveAction(currentSpeed, position, rotation);
        }
    }
}