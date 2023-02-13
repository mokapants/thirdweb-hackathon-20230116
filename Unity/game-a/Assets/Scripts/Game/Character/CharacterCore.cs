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
        private static readonly string contractAddress = "0x115Dba625Dc1E9AA6bb0Bbd4674FF09C701898C7";

        [Inject]
        public void Inject(IPlayerStatus playerStatus)
        {
            this.playerStatus = playerStatus;
        }

        private void Start()
        {
            // 初期化
            ThirdwebManager.Instance.IsConnectWalletObservable.Subscribe(isConnectWallet => InitializeAsync(isConnectWallet)).AddTo(this);

            // 選択ボタンが押されたとき
            characterUIView.OnChose.Subscribe(_ => OnClickedChooseButton()).AddTo(this);
        }

        /// <summary>
        /// データを初期化
        /// </summary>
        private async UniTask InitializeAsync(bool isConnectWallet)
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

            // ウォレットを接続済みか
            if (!isConnectWallet)
            {
                isOwned = false;
                characterUIView.SetButtonInteractable(isOwned);
                return;
            }

            // NFTを保持しているか
            var walletAddress = await ThirdwebManager.Instance.SDK.wallet.GetAddress();
            var balance = int.Parse(await ThirdwebManager.Instance.SDK.GetContract(contractAddress).ERC1155.BalanceOf(walletAddress, tokenId));
            isOwned = 0 < balance;
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