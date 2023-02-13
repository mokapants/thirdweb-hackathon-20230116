using System;
using AAS;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

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

            // ロードと同時にアドレスが存在するか確認
            GameObject instance;
            try
            {
                instance = await Addressables.InstantiateAsync(address, Vector3.zero, Quaternion.identity);
            }
            catch (Exception e)
            {
                Utils.Debug.Log(e.Message);
                return null;
            }
            if (instance == null) return null;

            // 子オブジェクトを全削除
            foreach (Transform child in graphics)
            {
                Destroy(child.gameObject);
            }

            // 新たなキャラクターを配置
            instance.transform.localScale *= 0.5f;
            instance.transform.SetParent(graphics, false);
            var animator = instance.GetComponent<Animator>();
            return animator;
        }
    }
}