using System;
using Cysharp.Threading.Tasks;
using Game.Player.Interfaces;
using UniRx;
using UnityEngine;
using VContainer;

namespace Game.Character
{
    public class CharacterCore : MonoBehaviour
    {
        // --- メンバ変数 --- //
        // トークンID
        [SerializeField] private string tokenId;
        // ステータスを表示するUI上のView
        [SerializeField] private CharacterUIView characterUIView;
        // プレイヤーのステータス
        private IPlayerStatus playerStatus;
        // NFTを保有しているか
        private bool isOwned;
        // コントラクトアドレス
        private static readonly string contractAddress = "0x9F925C42c55D607CD30D0481B20476f8BCC25C5e";

        [Inject]
        public void Inject(IPlayerStatus playerStatus)
        {
            this.playerStatus = playerStatus;
        }

        private void Start()
        {
            // 初期化
            ThirdwebManager.Instance.IsConnectWalletObservable.Subscribe(_ => InitializeAsync()).AddTo(this);

            // 選択ボタンが押されたとき
            characterUIView.OnChose.Subscribe(_ => OnClickedChooseButton()).AddTo(this);
        }

        /// <summary>
        /// データを初期化
        /// </summary>
        private async UniTask InitializeAsync()
        {
            // データを取得
            var nft = await ThirdwebManager.Instance.SDK.GetContract(contractAddress).ERC1155.Get(tokenId);
            Debug.Log(nft.ToString());

            // キャラクターデータをダウンロード
            var jsonUrl = nft.metadata.external_url;
            var characterData = await CharacterDataDownloader.DownloadCharacterDataAsync(jsonUrl);
            var name = characterData.name;
            var speed = "-";
            var jump = "-";
            for (var i = 0; i < characterData.attributes.Length; i++)
            {
                switch (characterData.attributes[i].trait_type)
                {
                    case "Speed":
                        speed = characterData.attributes[i].value;
                        break;
                    case "Jump":
                        jump = characterData.attributes[i].value;
                        break;
                }
            }
            var iconUrl = characterData.image;
            var icon = await CharacterDataDownloader.DownloadTexture2DAsync(iconUrl);

            // データをUIに適用
            characterUIView.SetName(name);
            characterUIView.SetSpeedValue(speed);
            characterUIView.SetJumpValue(jump);
            characterUIView.SetIconImage(icon);

            // NFTを保持しているか
            var isOwned = 0 < nft.quantityOwned;
            characterUIView.SetButtonInteractable(isOwned);
        }

        /// <summary>
        /// 選択ボタンが押されたとき
        /// </summary>
        private void OnClickedChooseButton()
        {
            if (!isOwned) return;
            playerStatus.SetCharacterAddress(tokenId);
        }
    }
}