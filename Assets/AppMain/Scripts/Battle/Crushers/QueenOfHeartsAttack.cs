using UnityEngine;

public class QueenOfHeartsAttack : MonoBehaviour {
    #region Private Fields
    private DirectionController _directionController = null;
    private Animator _animator = null;
    private AudioSource _audioSource = null;
    private float _nextAttackTime = 0;

    // QueenOfHearts固有
    private float _chargeCounter = 0;
    private bool _isCharging = false;
    private int _normalDamage = 0;
    private float _normalAttackRange = 0;
    private GameObject _chargingEffect = null;
    private GameObject _heartChargedEffect = null;
    #endregion

    #region Serialized Fields
    [SerializeField] private Transform _attackPoint = null;
    [SerializeField] private Transform _attackPointUp = null;
    [SerializeField] private Transform _attackPointDown = null;
    [SerializeField, Header("1秒に何回攻撃できるか")] private float _attackRate = 3.0f;
    [SerializeField] private int _damage = 2;
    [SerializeField] private float _attackRange = 15.0f;
    [SerializeField] private GameObject _obstacleCrushEffect = null;
    [SerializeField] private LayerMask _obstacleLayer;

    // QueenOfHearts固有
    [SerializeField] private GameObject[] _attackEffectPrefabs = null;
    [SerializeField] private float _chargeTime = 1.0f;
    [SerializeField] private GameObject _chargingEffectPrefab = null;
    [SerializeField] private GameObject _heartChargedEffectPrefab = null;
    [SerializeField] private AudioClip[] _audioClips = null;
    [SerializeField] private Transform _chargedAttackPointDown = null;
    #endregion


    private void Start() {
        _directionController = GameObject.FindWithTag("Direction").GetComponent<DirectionController>();
        _animator = this.GetComponent<Animator>();
        _audioSource = this.GetComponent<AudioSource>();
        
        _normalDamage = _damage;
        _normalAttackRange = _attackRange;
    }

    private void Update() {
        if (_directionController.IsDirection || Time.time < _nextAttackTime)
            return;

        if (Input.GetButtonUp("Fire1")) {
            var verticalKey = Input.GetAxisRaw("Vertical");

            if (verticalKey > 0) {
                Attack(_attackPointUp);
                if (_isCharging) {
                    _animator.SetTrigger("ChargedAttack_Upwards");
                    Instantiate(_attackEffectPrefabs[1], _attackPointUp.transform);
                } else {
                    _animator.SetTrigger("Attack_Upwards");
                    Instantiate(_attackEffectPrefabs[0], _attackPointUp.transform);
                }
            } else if (verticalKey < 0) {
                Attack(_attackPointDown);
                if (_isCharging) {
                    _animator.SetTrigger("ChargedAttack_Downwards");
                    Instantiate(_attackEffectPrefabs[1], _chargedAttackPointDown);
                } else {
                    _animator.SetTrigger("Attack_Downwards");
                    Instantiate(_attackEffectPrefabs[0], _attackPointDown.transform);
                }
            } else {
                Attack(_attackPoint);
                if (_isCharging) {
                    _animator.SetTrigger("ChargedAttack");
                    Instantiate(_attackEffectPrefabs[1], _attackPoint.transform);
                } else {
                    _animator.SetTrigger("Attack");
                    Instantiate(_attackEffectPrefabs[0], _attackPoint.transform);
                }
            }
            if (_isCharging)
                _audioSource.PlayOneShot(_audioClips[1]);
            else
                _audioSource.PlayOneShot(_audioClips[0]);

            _nextAttackTime = Time.time + 1.0f / _attackRate;
            
            // 値のリセット.
            if (_isCharging)
                Destroy(_heartChargedEffect);
            _isCharging = false;
            Destroy(_chargingEffect);
            _chargeCounter = 0;
            _damage = _normalDamage;
            _attackRange = _normalAttackRange;
        }

        if (Input.GetButton("Fire1")) {
            _chargeCounter += Time.deltaTime;
            _animator.SetBool("Attack_Charge", true);

            if (_isCharging)
                return;
            
            if (_chargeCounter >= _chargeTime) {
                _isCharging = true;

                Destroy(_chargingEffect);
                if (_heartChargedEffect == null) {
                    _heartChargedEffect = Instantiate(_heartChargedEffectPrefab, Vector3.zero, Quaternion.identity);
                    _heartChargedEffect.transform.SetParent(this.transform, false);
                    _heartChargedEffect.transform.localPosition = Vector3.zero;
                }

                _damage = _normalDamage + 2;
                _attackRange = _normalAttackRange * 4.0f;
            } else {
                if (_chargingEffect == null) {
                    _chargingEffect = Instantiate(_chargingEffectPrefab, Vector3.zero, Quaternion.identity);
                    _chargingEffect.transform.SetParent(this.transform, false);
                    _chargingEffect.transform.localPosition = Vector3.zero;
                }
            }
        } else {
            _animator.SetBool("Attack_Charge", false);
        }
    }

    private void Attack(Transform point) {
        Collider2D[] hitInfos = Physics2D.OverlapCircleAll(point.position, _attackRange, _obstacleLayer);
        foreach (Collider2D hitInfo in hitInfos) {
            var destroyableObstacle = hitInfo.transform.GetComponent<DestroyableObstacle>();
            var destroyableBuilder = hitInfo.transform.GetComponent<DestroyableBuilder>();
            var chef = hitInfo.transform.GetComponent<Chef>();
            var zakoWolf = hitInfo.transform.GetComponent<ZakoWolf>();
            var pig = hitInfo.transform.GetComponent<Pig>();
            var frog = hitInfo.transform.GetComponent<Frog>();
            var bushi = hitInfo.transform.GetComponent<Bushi>();
            var creamPuff = hitInfo.transform.GetComponent<CreamPuff>();

            if (destroyableObstacle != null) {
                destroyableObstacle.TakeDamage(_damage);
                Instantiate(_obstacleCrushEffect, hitInfo.transform.position, Quaternion.identity);
            }
            if (destroyableBuilder != null) {
                destroyableBuilder.TakeDamage(1);
                Instantiate(_obstacleCrushEffect, hitInfo.transform.position, Quaternion.identity);
            }
            if (chef != null) {
                chef.TakeDamage(_damage);
                Instantiate(_obstacleCrushEffect, hitInfo.transform.position, Quaternion.identity);
            }
            if (zakoWolf != null) {
                zakoWolf.TakeDamage(_damage);
                Instantiate(_obstacleCrushEffect, hitInfo.transform.position, Quaternion.identity);
            }
            if (pig != null) {
                pig.TakeDamage(_damage);
                Instantiate(_obstacleCrushEffect, hitInfo.transform.position, Quaternion.identity);
            }
            if (frog != null) {
                frog.TakeDamage(_damage);
                Instantiate(_obstacleCrushEffect, hitInfo.transform.position, Quaternion.identity);
            }
            if (bushi != null) {
                bushi.TakeDamage(_damage);
                Instantiate(_obstacleCrushEffect, hitInfo.transform.position, Quaternion.identity);
            }
            if (creamPuff != null) {
                creamPuff.TakeDamage();
                Instantiate(_obstacleCrushEffect, hitInfo.transform.position, Quaternion.identity);
            }
        }
    }
}