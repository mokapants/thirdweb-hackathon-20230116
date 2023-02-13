using System;
using Cysharp.Threading.Tasks;
using Game.Player.ControllablePlayer;
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
        // 操作可能プレイヤーのコア
        private PlayerControl playerControl;
        // NFTを保有しているか
        private bool isOwned;
        // 最後に取得したキャラクター関連のデータ
        private CharacterData latestCharacterData;
        private float speed;
        private float jump;
        // コントラクトアドレス
        private static readonly string contractAddress = "0x115Dba625Dc1E9AA6bb0Bbd4674FF09C701898C7";

        [Inject]
        public void Inject(IPlayerStatus playerStatus, PlayerControl playerControl)
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
            latestCharacterData = await CharacterDataDownloader.DownloadCharacterDataAsync(jsonUrl);
            var name = latestCharacterData.name;
            var speedString = "-";
            var jumpString = "-";
            for (var i = 0; i < latestCharacterData.attributes.Length; i++)
            {
                switch (latestCharacterData.attributes[i].trait_type)
                {
                    case "Speed":
                        speedString = latestCharacterData.attributes[i].value;
                        float.TryParse(speedString, out speed);
                        break;
                    case "Jump":
                        jumpString = latestCharacterData.attributes[i].value;
                        float.TryParse(jumpString, out jump);
                        break;
                }
            }
            var iconUrl = latestCharacterData.image;
            var icon = await CharacterDataDownloader.DownloadTexture2DAsync(iconUrl);

            // データをUIに適用
            characterUIView.SetName(name);
            characterUIView.SetSpeedValue(speedString);
            characterUIView.SetJumpValue(jumpString);
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
            // ステータスに反映
            playerStatus.SetCharacterAddress(tokenId);
            // 操作に反映
            if (0 < speed) playerControl.SetMoveSpeed(speed);
            if (0 < jump) playerControl.SetJumpPower(jump);
        }
    }
}