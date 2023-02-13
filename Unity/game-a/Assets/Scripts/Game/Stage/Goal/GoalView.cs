using System;
using UnityEngine;

namespace Game.Stage.Goal
{
    public class GoalView : MonoBehaviour
    {
        // --- メンバ変数 --- //
        // ゴール時に表示するUIにつけるCanvasGroup
        [SerializeField] private CanvasGroup goalCanvasGroup;

        private void Start()
        {
            // ゴール演出を初期化
            ResetGoalAnimation();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnTriggerEnterGoal();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnTriggerExitGoal();
            }
        }

        /// <summary>
        /// ゴールしたとき
        /// </summary>
        private void OnTriggerEnterGoal()
        {
            PlayGoalAnimation();
        }
        
        /// <summary>
        /// ゴールから離れたとき
        /// </summary>
        private void OnTriggerExitGoal()
        {
            ResetGoalAnimation();
        }

        /// <summary>
        /// ゴール演出を再生
        /// </summary>
        private void PlayGoalAnimation()
        {
            goalCanvasGroup.alpha = 1;
        }

        /// <summary>
        /// ゴール演出を初期状態にリセット
        /// </summary>
        private void ResetGoalAnimation()
        {
            goalCanvasGroup.alpha = 0;
        }
    }
}