using System.Collections.Generic;
using UnityEngine;

public class CrusherController : MonoBehaviour {
    #region Private Fields
    private BuilderController _builderController = null;
    private Animator _animator = null;
    private Rigidbody2D _rb2D = null;
    private float _xSpeed = 0.0f;
    private bool _isFacingRight = true;
    private bool _isWalking = false;
    private bool _isRunning = false;
    private bool _isJumping = false;
    private bool _isStunning = false;
    private float _normalRunSpeed = 0;
    private float _normalWalkSpeed = 0;
    private float _normalJumpForce = 0;
    private float _addSpeedX = 0.0f;
    private List<float> _rightNum = new List<float>() { 19.0f, 19.0f, 20.0f, 18.5f, };
    private List<float> _endNum = new List<float>() { 19.2f, 19.2f, 20.2f, 19.5f, };
    private int _crusherIndex = 0;
    #endregion

    private enum MOVE_DIRECTION {
        STOP,
        RIGHT,
        LEFT,
    }
    MOVE_DIRECTION _moveDirection = MOVE_DIRECTION.STOP;

    #region Serialized Fields
    [SerializeField] private string _crusherName = "";
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _walkSpeed = 100.0f;
    [SerializeField] private float _runSpeed = 180.0f;
    [SerializeField] private float _jumpForce = 250.0f;
    #endregion

    private void Start() {
        Application.targetFrameRate = 60;

        _normalWalkSpeed = _walkSpeed;
        _normalRunSpeed = _runSpeed;
        _normalJumpForce = _jumpForce;

        _animator = this.GetComponent<Animator>();
        _rb2D = this.GetComponent<Rigidbody2D>();

        _crusherIndex = GameDirector.Instance.CrusherIndex;

        _builderController = GameObject.Find("BuilderController").GetComponent<BuilderController>();
    }

    private void Update() {
        GameDirector.Instance.CrusherPosition = this.transform.position.x;

        float horizontalKey = Input.GetAxisRaw("Horizontal");
        bool runKey = Input.GetButton("Select");
        if (horizontalKey == 0) {
            _moveDirection = MOVE_DIRECTION.STOP;
            _isWalking = false;
            _isRunning = false;
        } else if (horizontalKey > 0) {
            _moveDirection = MOVE_DIRECTION.RIGHT;

            if (!_isFacingRight)
                Flip();

            if (runKey) {
                _isWalking = false;
                _isRunning = true;
            } else {
                _isRunning = false;
                _isWalking = true;
            }
        } else if (horizontalKey < 0) {
            _moveDirection = MOVE_DIRECTION.LEFT;

            if (_isFacingRight)
                Flip();

            if (runKey) {
                _isWalking = false;
                _isRunning = true;
            } else {
                _isRunning = false;
                _isWalking = true;
            }
        }

        if (IsGrounded()) {
            Debug.Log("接地してるよ");
            if (Input.GetButtonDown("Jump")) {
                Debug.Log("ジャンプボタンを押したよ");
                _isJumping = true;
                Jump();
            } else {
                _isJumping = false;
            }
        }

        // FixedUpdate()に書くと他の処理との兼ね合いか、ワゴンを出た後にスピードが元に戻らないので注意.
        if (_builderController.wagonControllerRun != null) {
            // ワゴンに乗ったらクラッシャーのスピードをワゴンのスピードとも関連づける.
            if (_builderController.wagonControllerRun.CrusherEnterCheck.IsOn)
                _addSpeedX = _builderController.wagonControllerRun.GetWagonVelocity();

            //ワゴンから降りたらクラッシャーのスピードを通常に戻す.
            if (_builderController.wagonControllerRun.CrusherExitCheck.IsOn)
                _addSpeedX = 0.0f;
        }
        
        SetAnimation();
    }

    private void FixedUpdate() {
        switch (_moveDirection) {
            case MOVE_DIRECTION.STOP:
                _xSpeed = 0.0f;
                break;
            case MOVE_DIRECTION.RIGHT:
                if (_isRunning)
                    _xSpeed = _runSpeed;
                else
                    _xSpeed = _walkSpeed;
                break;
            case MOVE_DIRECTION.LEFT:
                if (_isRunning)
                    _xSpeed = -_runSpeed;
                else
                    _xSpeed = -_walkSpeed;
                break;
            default:
                break;
        }

        _rb2D.velocity = new Vector2(_xSpeed + _addSpeedX, _rb2D.velocity.y);
    }

    private void Flip() {
        _isFacingRight = !_isFacingRight;

        transform.Rotate(0, 180.0f, 0);
    }

    private bool IsGrounded() {
        Vector3 startRightVec = transform.position - transform.up * _rightNum[_crusherIndex] + transform.right * 5.2f;
        Vector3 startLeftVec = transform.position - transform.up * _rightNum[_crusherIndex] - transform.right * 5.2f;
        Vector3 endVec = transform.position - transform.up * _endNum[_crusherIndex];
        Debug.DrawLine(startRightVec, endVec);
        Debug.DrawLine(startLeftVec, endVec);

        return Physics2D.Linecast(startRightVec, endVec, _groundLayer) ||
               Physics2D.Linecast(startLeftVec, endVec, _groundLayer);
    }

    private void Jump() {
        Debug.Log("ジャンプ！");
        _rb2D.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
    }

    private void SetAnimation() {
        // 優先度の低い順に記述する.
        _animator.SetBool("Walk", _isWalking);
        _animator.SetBool("Run", _isRunning);
        _animator.SetBool("Jump", _isJumping);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("Obstacle")) {
            GameDirector.Instance.CrusherKillCounts++;

            _isStunning = true;
            _animator.SetBool("Stun",true);
            _isWalking = false;
            _isRunning = false;
            _isJumping = false;
            _animator.Play(_crusherName + "_Stun");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("SpecialFloor")) {
            _walkSpeed *= 0.5f;
            _runSpeed *= 0.3f;
            _jumpForce *= 0.7f;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("SpecialFloor")) {
            _walkSpeed = _normalWalkSpeed;
            _runSpeed = _normalRunSpeed;
            _jumpForce = _normalJumpForce;
        }
    }

    private bool IsStunAnimationEnded() {
        if (_isStunning && _animator != null) {
            AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName(_crusherName + "_Stun"))
                if (currentState.normalizedTime >= 1)
                    return true;
        }

        return false;
    }

    /// <summary>
    /// スタンアニメーションが終了し、コンティニュー待ちか判定する
    /// </summary>
    /// <returns></returns>
    public bool IsContinueWaiting() {
        return IsStunAnimationEnded();
    }

    /// <summary>
    /// クラッシャーのアニメーション状態を戻す
    /// </summary>
    public void ContinueCrusher() {
        _isStunning = false;
        _animator.SetBool("Stun",false);    // TODO addした. デバッグ時確認
        _animator.Play(_crusherName + "_Idle");
        _isWalking = false;
        _isRunning = false;
        _isJumping = false;
    }
}
