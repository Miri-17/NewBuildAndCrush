using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherController : MonoBehaviour {
//     #region
//     [SerializeField] private LayerMask groundLayer;
//     [SerializeField] private float _walkSpeed = 100.0f;
//     [SerializeField] private float _runSpeed = 180.0f;
//     [SerializeField] private float _jumpForce = 250.0f;
//     #endregion


//     #region
//     private Animator _animator = null;
//     private Rigidbody2D _rb2D = null;
//     private float _xSpeed = 0.0f;
//     private bool _isFacingRight = true;
//     private bool _isWalking = false;
//     private bool _isRunning = false;
//     private bool _isJumping = false;
//     private bool _isStunning = false;
//     private string[] crusherNames = { "Girl", "QueenOfHearts", "Tenjin", "Witch" };
//     private string crusherName;
//     private float addSpeedX = 0.0f;
//     private BuilderController builderController;
//     #endregion

//     // TODO 
//     /// <summary>
//     /// run初期値（ガムで使います）
//     /// </summary>
//     private float _runValue;
//     /// <summary>
//     /// walk初期値（ガムで使います）
//     /// </summary>
//     private float _walkValue;
//     /// <summary>
//     /// jump初期値（ガムで使います）
//     /// </summary>
//     private float _jumpValue;

//     private enum MOVE_DIRECTION {
//         STOP,
//         RIGHT,
//         LEFT,
//     }
//     MOVE_DIRECTION moveDirection = MOVE_DIRECTION.STOP;

//     private void Start() {
//         Application.targetFrameRate = 60;

//         _runValue = _runSpeed;
//         _walkValue = _walkSpeed;
//         _jumpValue = _jumpForce;

//         _animator = GetComponent<Animator>();
//         _rb2D = GetComponent<Rigidbody2D>();

//         crusherName = crusherNames[GameDirector.Instance.CrusherIndex];

//         builderController = GameObject.Find("BuilderController").GetComponent<BuilderController>();
//     }

//     private void Update()
//     {
//         if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
//         {
//             _isStunning = true;
//             _animator.Play(crusherName + "@stun");
//         }

//         GameDirector.Instance.crusherPosition = this.transform.position.x;

//         float horizontalKey = Input.GetAxisRaw("Horizontal");
//         bool runKey = Input.GetButton("select");
//         if (horizontalKey == 0)
//         {
//             moveDirection = MOVE_DIRECTION.STOP;
//             _isWalking = false;
//             _isRunning = false;
//         }
//         else if (horizontalKey > 0)
//         {
//             moveDirection = MOVE_DIRECTION.RIGHT;

//             if (!_isFacingRight)
//             {
//                 Flip();
//             }

//             if (runKey)
//             {
//                 _isWalking = false;
//                 _isRunning = true;
//             }
//             else
//             {
//                 _isRunning = false;
//                 _isWalking = true;
//             }
//         }
//         else if (horizontalKey < 0)
//         {
//             moveDirection = MOVE_DIRECTION.LEFT;

//             if (_isFacingRight)
//             {
//                 Flip();
//             }

//             if (runKey)
//             {
//                 _isWalking = false;
//                 _isRunning = true;
//             }
//             else
//             {
//                 _isRunning = false;
//                 _isWalking = true;
//             }
//         }

//         if (IsGrounded())
//         {
//             if (Input.GetButtonDown("Jump"))
//             {
//                 Jump();
//                 _isJumping = true;
//             }
//             else
//             {
//                 _isJumping = false;
//             }
//         }

//         // FixedUpdate()に書くと他の処理との兼ね合いか、ワゴンを出た後にスピードが元に戻らないので注意
//         if (builderController.wagonControllerRun != null)
//         {
//             // ワゴンに乗ったらクラッシャーのスピードをワゴンのスピードとも関連づける
//             if (builderController.wagonControllerRun.crusherEnterCheck.isOn)
//             {
//                 addSpeedX = builderController.wagonControllerRun.GetWagonVelocity();
//             }

//             //ワゴンから降りたらクラッシャーのスピードを通常に戻す.
//             if (builderController.wagonControllerRun.crusherExitCheck.isOn)
//             {
//                 addSpeedX = 0.0f;
//             }
//         }
        
//         SetAnimation();
//     }

//     private void FixedUpdate()
//     {
//         switch (moveDirection)
//         {
//             case MOVE_DIRECTION.STOP:
//                 _xSpeed = 0.0f;
//                 break;
//             case MOVE_DIRECTION.RIGHT:
//                 if (_isRunning)
//                 {
//                     _xSpeed = _runSpeed;
//                 }
//                 else
//                 {
//                     _xSpeed = _walkSpeed;
//                 }

//                 break;
//             case MOVE_DIRECTION.LEFT:
//                 if (_isRunning)
//                 {
//                     _xSpeed = -_runSpeed;
//                 }
//                 else
//                 {
//                     _xSpeed = -_walkSpeed;
//                 }

//                 break;
//         }
        
//         _rb2D.velocity = new Vector2(_xSpeed + addSpeedX, _rb2D.velocity.y);
//     }

//     private bool IsGrounded()
//     {
//         // Vector3 startRightVec = transform.position - transform.up * 19.0f + transform.right * 5.2f;  // Girl
//         // Vector3 startRightVec = transform.position - transform.up * 20.0f + transform.right * 5.2f; // Tenjin
//         Vector3 startRightVec = transform.position - transform.up * 18.5f + transform.right * 5.2f; // Witch
//         // Vector3 startLeftVec = transform.position - transform.up * 19.0f - transform.right * 5.2f;   // Girl
//         // Vector3 startLeftVec = transform.position - transform.up * 20.0f - transform.right * 5.2f;  // Tenjin
//         Vector3 startLeftVec = transform.position - transform.up * 18.5f - transform.right * 5.2f; // Witch
//         // Vector3 endVec = transform.position - transform.up * 19.2f;  // Girl
//         // Vector3 endVec = transform.position - transform.up * 20.2f; // Tenjin
//         Vector3 endVec = transform.position - transform.up * 19.5f; // Witch
//         Debug.DrawLine(startRightVec, endVec);
//         Debug.DrawLine(startLeftVec, endVec);
//         return Physics2D.Linecast(startRightVec, endVec, groundLayer) ||
//                Physics2D.Linecast(startLeftVec, endVec, groundLayer);
//     }

//     private void Jump()
//     {
//         _rb2D.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
//     }

//     private void Flip()
//     {
//         _isFacingRight = !_isFacingRight;

//         transform.Rotate(0f, 180f, 0f);
//     }

//     private void SetAnimation()
//     {
//         _animator.SetBool("walk", _isWalking);
//         _animator.SetBool("run", _isRunning);
//         _animator.SetBool("jump", _isJumping);
//     }

//     private async void OnCollisionEnter2D(Collision2D other)
//     {
//         if (other.collider.CompareTag("Obstacle"))
//         {
//             GameDirector.Instance.crusherKillCounts++;
//             _animator.Play(crusherName + "@stun");
//             _animator.SetBool("stun",true);
//             _isStunning = true;
//             _isRunning = false;
//             _isWalking = false;
//             _isJumping = false;
//             await UniTask.Delay(TimeSpan.FromSeconds(1f));
//             _animator.SetBool("stun",false);
//             _isRunning = true;
//             _isWalking = true;
//             _isJumping = true;
//         }
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (collision.CompareTag("Gum"))
//         {
//             _walkSpeed *= 0.5f;
//             _runSpeed *= 0.3f;
//             _jumpForce *= 0.7f;
//         }
//     }

//     private void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Gum"))
//         {
//             _walkSpeed = _walkValue;
//             _runSpeed = _runValue;
//             _jumpForce = _jumpValue;
//         }
//     }

//     [HideInInspector]
//     public bool IsContinueWaiting()
//     {
//         return IsStunAnimationEnded();
//     }

//     private bool IsStunAnimationEnded()
//     {
//         if (_isStunning && _animator != null)
//         {
//             AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);
//             if (currentState.IsName(crusherName + "@stun"))
//             {
//                 if (currentState.normalizedTime >= 1)
//                 {
//                     return true;
//                 }
//             }
//         }

//         return false;
//     }

//     public void ContinueCrusher()
//     {
//         _isStunning = false;
//         _animator.Play(crusherName + "@idle");
//         _isJumping = false;
//         _isWalking = false;
//         _isRunning = false;
//     }
}
