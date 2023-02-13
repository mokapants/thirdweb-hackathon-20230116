using System;
using UnityEngine;

namespace Game.Player.Actions.Common
{
    public class PlayerAnimationAction : MonoBehaviour
    {
        // --- メンバ変数 --- //
        // キャラクターのAnimator
        private Animator animator;
        // Animatorのパラメータ
        private static readonly string SpeedKey = "Speed";

        /// <summary>
        /// Animatorを設定
        /// </summary>
        public void SetAnimator(Animator animator)
        {
            this.animator = animator;
        }

        /// <summary>
        /// 現在の移動速度をAnimatorに適用
        /// </summary>
        public void SetCurrentSpeed(float currentSpeed)
        {
            if (animator == null) return;
            animator.SetFloat(SpeedKey, currentSpeed);
        }
    }
}