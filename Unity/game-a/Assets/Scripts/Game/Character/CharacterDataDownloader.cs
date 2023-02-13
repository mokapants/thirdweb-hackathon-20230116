using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Character
{
    public class CharacterDataDownloader
    {
        /// <summary>
        /// キャラクターデータを取得
        /// </summary>
        public static async UniTask<CharacterData> DownloadCharacterDataAsync(string jsonUrl)
        {
            var webRequest = UnityWebRequest.Get(jsonUrl);

            if (webRequest == null) return null;

            using (webRequest)
            {
                // タイムアウトを設定
                webRequest.timeout = 30;

                // 取得できるまで待機
                await webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Utils.Debug.LogError(new Exception());
                    return null;
                }

                webRequest.disposeDownloadHandlerOnDispose = false;
                
                var characterData = JsonUtility.FromJson<CharacterData>(webRequest.downloadHandler.text);
                return characterData;
            }
        }

        /// <summary>
        /// 画像を取得
        /// </summary>
        public static async UniTask<Texture2D> DownloadTexture2DAsync(string url)
        {
            var webRequest = UnityWebRequestTexture.GetTexture(url);

            if (webRequest == null) return null;

            using (webRequest)
            {
                // タイムアウトを設定
                webRequest.timeout = 30;

                // 取得できるまで待機
                await webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Utils.Debug.LogError(new Exception());
                    return null;
                }

                webRequest.disposeDownloadHandlerOnDispose = false;

                var downloadHandlerTexture = webRequest.downloadHandler as DownloadHandlerTexture;
                var texture = downloadHandlerTexture.texture;

                return texture;
            }
        }
    }
}