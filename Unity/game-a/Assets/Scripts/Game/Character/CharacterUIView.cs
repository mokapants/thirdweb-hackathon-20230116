using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Character
{
    public class CharacterUIView : MonoBehaviour
    {
        // --- メンバ変数 --- //
        // 選択用ボタン
        [SerializeField] private Button button;
        // 各項目の反映先
        [SerializeField] private Image buttonImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text speedText;
        [SerializeField] private TMP_Text jumpText;
        [SerializeField] private RawImage image;
        // 選択ボタンが押されたときのイベント
        private Subject<Unit> onClickedChooseButton;

        // --- イベント --- //
        public IObservable<Unit> OnChose => onClickedChooseButton;

        private void Awake()
        {
            onClickedChooseButton = new Subject<Unit>();
        }

        private void Start()
        {
            button.onClick.AddListener(OnClickedChooseButton);
        }

        /// <summary>
        /// ボタンの状態を設定
        /// </summary>
        public void SetButtonInteractable(bool interactable)
        {
            button.interactable = interactable;
            var imageColor = buttonImage.color;
            imageColor.a = interactable ? 1f : 0.1f;
            buttonImage.color = imageColor;
        }

        /// <summary>
        /// 名前を設定
        /// </summary>
        public void SetName(string name)
        {
            nameText.text = name;
        }

        /// <summary>
        /// 移動速度の能力値を設定
        /// </summary>
        public void SetSpeedValue(string speed)
        {
            speedText.text = speed;
        }

        /// <summary>
        /// ジャンプの能力値を設定
        /// </summary>
        public void SetJumpValue(string jump)
        {
            jumpText.text = jump;
        }

        /// <summary>
        /// アイコンを設定
        /// </summary>
        public void SetIconImage(Texture2D texture2D)
        {
            image.texture = texture2D;
        }

        /// <summary>
        /// 選択ボタンが押されたとき
        /// </summary>
        private void OnClickedChooseButton()
        {
            onClickedChooseButton.OnNext(Unit.Default);
        }
    }
}