using System;
using AAS;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Player.Actions.Common
{
    public class PlayerCharacterAction : MonoBehaviour
    {
        // --- メンバ変数 --- //
        [SerializeField] private Transform graphics;

        /// <summary>
        /// アドレスをもとにキャラクターを変更
        /// </summary>
        public async UniTask<Animator> UpdateCharacterAddress(string address)
        {
            // リモートのカタログをロードするまで待つ
            await UniTask.WaitUntil(() => AddressablesManager.IsLoadedRemoteCatalog);

            // 子オブジェクトを全削除
            foreach (Transform child in graphics)
            {
                Destroy(child.gameObject);
            }
            
            // 新たなキャラクターを配置
            var instance = await Addressables.InstantiateAsync(address).Task;
            var animator = instance.GetComponent<Animator>();
            return animator;
        }
    }
}