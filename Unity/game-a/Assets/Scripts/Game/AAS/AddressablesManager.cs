using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AAS
{
    public class AddressablesManager : MonoBehaviour
    {
        // --- メンバ変数 --- //
        // リモートのカタログをロードできているか
        private static bool isLoadedRemoteCatalog;
        
        // --- プロパティ --- //
        public static bool IsLoadedRemoteCatalog => isLoadedRemoteCatalog;
        
        private async void Awake()
        {
            await LoadRemoteCatalog();
        }

        /// <summary>
        /// リモートにあるカタログを取得
        /// </summary>
        private async UniTask LoadRemoteCatalog()
        {
            // カタログを取得
            var url = $"https://raw.githubusercontent.com/mokapants/thirdweb-hackathon-20230116/main/Database/AAS/catalog_v0.0.1.json";
            await Addressables.InitializeAsync();
            await Addressables.LoadContentCatalogAsync(url, true);

            isLoadedRemoteCatalog = true;
        }
    }
}