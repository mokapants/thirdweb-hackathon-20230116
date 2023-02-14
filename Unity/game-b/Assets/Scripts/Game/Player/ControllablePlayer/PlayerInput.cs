using System;
using UniRx;
using UnityEngine;
using Debug = Utils.Debug;

namespace Game.Player.ControllablePlayer
{
    public class PlayerInput : MonoBehaviour
    {
        // --- メンバ変数 --- //
        // 縦方向の入力のイベント用
        private Subject<float> inputVerticalSubject;
        // 横方向の入力のイベント用
        private Subject<float> inputHorizontalSubject;
        // ジャンプ入力のイベント用
        private Subject<Unit> inputJumpSubject;

        // --- イベント --- //
        public IObservable<float> OnInputVertical => inputVerticalSubject;
        public IObservable<float> OnInputHorizontal => inputHorizontalSubject;
        public IObservable<Unit> OnInputJump => inputJumpSubject;

        private void Awake()
        {
            inputVerticalSubject = new Subject<float>();
            inputHorizontalSubject = new Subject<float>();
            inputJumpSubject = new Subject<Unit>();
        }

        private void Update()
        {
            // 縦方向の入力
            var inputZ = Input.GetAxisRaw("Vertical");
            inputVerticalSubject.OnNext(inputZ);
            
            // 横方向の入力
            var inputX = Input.GetAxisRaw("Horizontal");
            inputHorizontalSubject.OnNext(inputX);

            // ジャンプ入力
            var isInputJump = 0f < Input.GetAxis("Jump");
            if (isInputJump)
            {
                inputJumpSubject.OnNext(Unit.Default);
            }
        }
    }
}