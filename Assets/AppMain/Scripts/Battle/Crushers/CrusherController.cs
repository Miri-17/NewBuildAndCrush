using UnityEngine;

public class CrusherController : MonoBehaviour {
    private const float RIGHT_NUM = 18.5f;
    private const float END_NUM = 19.5f;

    #region Private Fields
    private DirectionController _directionController = null;
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
    private float[] _rightNum = new float[4] { 19.0f, 17.0f, 20.0f, 18.9f, };
    private float[] _endNum = new float[4] { 20.2f, 18.2f, 21.2f, 20.1f, };
    private int _crusherIndex = 0;
    private AudioSource _audioSourceSE = null;
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
    [SerializeField] private AudioClip _stunAudioClip = null;
    #endregion

    private void Start() {
        Application.targetFrameRate = 60;

        _normalWalkSpeed = _walkSpeed;
        _normalRunSpeed = _runSpeed;
        _normalJumpForce = _jumpForce;

        _animator = this.GetComponent<Animator>();
        _rb2D = this.GetComponent<Rigidbody2D>();

        _crusherIndex = GameDirector.Instance.CrusherIndex;

        _directionController = GameObject.FindWithTag("Direction").GetComponent<DirectionController>();
        _builderController = GameObject.Find("BuilderController").GetComponent<BuilderController>();

        _audioSourceSE = CrusherSE.Instance.GetComponent<AudioSource>();
    }

    private void Update() {
        if (_directionController.IsDirection) {
            _rb2D.velocity = new Vector2(0, 0);
            // _addSpeedX = 0;
            return;
        }
        
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
            if (Input.GetButtonDown("Jump")) {
                _isJumping = true;
                _animator.SetBool("Jump", _isJumping);  // ジャンプアニメーションがされないことのないようにする.
                Jump();
            } else {
                _isJumping = false;
            }
        }

        // FixedUpdate()に書くと他の処理との兼ね合いか、ワゴンを出た後にスピードが元に戻らないので注意.
        if (_builderController.WagonControllerRun != null) {
            // ワゴンに乗ったらクラッシャーのスピードをワゴンのスピードとも関連づける.
            if (_builderController.WagonControllerRun.CrusherEnterCheck.IsOn) {
                // Debug.Log("add speed x: " + _addSpeedX);
                _addSpeedX = _builderController.WagonControllerRun.GetWagonVelocity();
            }

            //ワゴンから降りたらクラッシャーのスピードを通常に戻す.
            if (_builderController.WagonControllerRun.CrusherExitCheck.IsOn) {
                // Debug.Log("降りた。add speed x: " + _addSpeedX);
                _addSpeedX = 0;
            }
        }
        
        SetAnimation();
    }

    private void FixedUpdate() {
        if (_directionController.IsDirection) {
            _xSpeed = 0;
            _addSpeedX = 0;
            return;
        }

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
        _rb2D.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
    }

    private void SetAnimation() {
        // 優先度の低い順に記述する.
        _animator.SetBool("Walk", _isWalking);
        _animator.SetBool("Run", _isRunning);
        _animator.SetBool("Jump", _isJumping);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!_isStunning && collision.collider.CompareTag("Obstacle")) {
            _isStunning = true;
            GameDirector.Instance.CrusherKillCounts++;
            _audioSourceSE.PlayOneShot(_stunAudioClip);
            _animator.SetBool("Stun",true);
            _isWalking = false;
            _isRunning = false;
            _isJumping = false;
            _animator.Play(_crusherName + "_Stun");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("SpecialFloor")) {
            Debug.Log("Enter");
            _walkSpeed *= 0.5f;
            _runSpeed *= 0.3f;
            _jumpForce *= 0.7f;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("SpecialFloor")) {
            Debug.Log("Exit");
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
