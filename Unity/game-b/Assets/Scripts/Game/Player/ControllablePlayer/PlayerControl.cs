using System;
using Game.Player.Interfaces;
using UniRx;
using UnityEngine;
using VContainer;
using Debug = Utils.Debug;

namespace Game.Player.ControllablePlayer
{
    public class PlayerControl : MonoBehaviour
    {
        // --- メンバ変数 --- //
        // 入力用
        [SerializeField] private PlayerInput playerInput;
        // キャラクターの位置情報反映用
        private IPlayerStatus playerStatus;
        // キャラクターを動かす用
        [SerializeField] private Rigidbody playerRigidbody;
        // 基本的な移動速度
        [SerializeField] private float baseMoveSpeed;
        // 基本的なジャンプ力
        [SerializeField] private float baseJumpPower;
        // ジャンプのクールタイム
        [SerializeField] private float jumpCoolTime;
        // 落下速度
        [SerializeField] private float fallSpeed;
        // キャラクター固有のパラメータ
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpPower;
        
        // --- プロパティ --- //
        private float MoveSpeed => baseMoveSpeed * moveSpeed;
        private float JumpPower => baseJumpPower * jumpPower;

        [Inject]
        public void Inject(IPlayerStatus playerStatus)
        {
            this.playerStatus = playerStatus;
        }

        private void Start()
        {
            playerInput.OnInputVertical.Subscribe(OnInputVertical).AddTo(this);
            playerInput.OnInputHorizontal.Subscribe(OnInputHorizontal).AddTo(this);
            playerInput.OnInputJump.ThrottleFirst(TimeSpan.FromSeconds(jumpCoolTime))
                .Subscribe(OnInputJump).AddTo(this);
        }

        private void Update()
        {
            playerStatus.SetCurrentSpeed(playerRigidbody.velocity.sqrMagnitude);
            playerStatus.SetPosition(playerRigidbody.position);
            playerStatus.SetRotation(playerRigidbody.rotation);
            
            UpdateForward();
        }

        /// <summary>
        /// 移動速度を設定
        /// </summary>
        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }

        /// <summary>
        /// ジャンプ力を設定
        /// </summary>
        public void SetJumpPower(float jump)
        {
            jumpPower = jump;
        }

        /// <summary>
        /// 前後移動入力があった時の処理
        /// </summary>
        private void OnInputVertical(float inputZ)
        {
            MoveVertical(inputZ);
        }

        /// <summary>
        /// 左右移動入力があった時の処理
        /// </summary>
        private void OnInputHorizontal(float inputX)
        {
            MoveHorizontal(inputX);
        }

        /// <summary>
        /// ジャンプ入力があった時の処理
        /// </summary>
        private void OnInputJump(Unit _)
        {
            JumpIfJumpPossible();
        }

        /// <summary>
        /// 前後方向への移動
        /// </summary>
        private void MoveVertical(float inputZ)
        {
            if (inputZ == 0) return;

            var velocity = playerRigidbody.velocity;
            var sign = 0 < inputZ ? -1 : 1;
            velocity.z = sign * MoveSpeed;
            playerRigidbody.velocity = velocity;
        }
        
        /// <summary>
        /// 横方向への移動
        /// </summary>
        private void MoveHorizontal(float inputX)
        {
            if (inputX == 0) return;

            var velocity = playerRigidbody.velocity;
            var sign = 0 < inputX ? -1 : 1;
            velocity.x = sign * MoveSpeed;
            playerRigidbody.velocity = velocity;
        }
        
        /// <summary>
        /// 横方向への移動
        /// </summary>
        private void UpdateForward()
        {
            if (playerRigidbody.velocity.sqrMagnitude <= 0.05f) return;
            
            // 向きの指定
            var eulerAngles = Quaternion.LookRotation(playerRigidbody.velocity.normalized).eulerAngles;
            eulerAngles.x = 0f;
            playerRigidbody.rotation = Quaternion.Euler(eulerAngles);
        }

        /// <summary>
        /// ジャンプ可能であればジャンプ
        /// </summary>
        private void JumpIfJumpPossible()
        {
            var center = gameObject.transform.position; // 中心
            var halfExtents = Vector3.one * 0.5f; // 当たり判定の中心からのサイズ
            var direction = Vector3.down; // Rayを飛ばす方向
            var rotation = gameObject.transform.rotation; // キューブの向き
            var maxDistance = 0.5f; // Rayを発生させる位置
            var hits = new RaycastHit[10];
            var hitCount = Physics.BoxCastNonAlloc(center, halfExtents, direction, hits, rotation, maxDistance);

            if (hitCount == 0) return;

            // 地面に接地していない場合はジャンプ不可能
            var isHitGround = false;
            foreach (var hit in hits)
            {
                if (hit.collider == null) continue;
                if (hit.collider.CompareTag("Ground"))
                {
                    isHitGround = true;
                    break;
                }
            }
            if (!isHitGround) return;

            Jump();
        }

        /// <summary>
        /// ジャンプ
        /// </summary>
        private void Jump()
        {
            var velocity = playerRigidbody.velocity;
            velocity.y = JumpPower;
            playerRigidbody.velocity = velocity;
        }
    }
}