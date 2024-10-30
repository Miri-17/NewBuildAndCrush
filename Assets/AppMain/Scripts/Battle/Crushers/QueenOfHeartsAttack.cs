using UnityEngine;

public class QueenOfHeartsAttack : MonoBehaviour {
    #region Private Fields
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

    // QueenOfHearts固有
    [SerializeField] private GameObject _attackEffectPrefab;
    [SerializeField] private float _chargeTime = 1.0f;
    [SerializeField] private GameObject _chargingEffectPrefab = null;
    [SerializeField] private GameObject _heartChargedEffectPrefab = null;
    [SerializeField] private int _damage = 2;
    [SerializeField] private float _attackRange = 15.0f;

    // [SerializeField] private LayerMask obstacleLayer;
    // [SerializeField] private GameObject obstaclesCrushEffect;
    // [SerializeField] private GameObject chargeEffect;
    // [SerializeField] private GameObject chargeEffectGorld;
    #endregion


    private void Start() {
        _animator = this.GetComponent<Animator>();
        _audioSource = this.GetComponent<AudioSource>();
        
        _normalDamage = _damage;
        _normalAttackRange = _attackRange;
    }

    private void Update() {
        if (Time.time < _nextAttackTime)
            return;

        if (Input.GetButtonUp("Fire1")) {
            var verticalKey = Input.GetAxisRaw("Vertical");

            if (verticalKey > 0) {
                _animator.SetTrigger("Attack_Upwards");
                // Attack(_attackPointUp);
                Instantiate(_attackEffectPrefab, _attackPointUp.position, _attackPointUp.rotation);
            } else if (verticalKey < 0) {
                _animator.SetTrigger("Attack_Downwards");
                // Attack(_attackPointDown);
                Instantiate(_attackEffectPrefab, _attackPointDown.position, _attackPointDown.rotation);
            } else {
                _animator.SetTrigger("Attack");
                // Attack(_attackPoint);
                Instantiate(_attackEffectPrefab, _attackPoint.position, _attackPoint.rotation);
            }
            _audioSource.PlayOneShot(_audioSource.clip);

            _nextAttackTime = Time.time + 1.0f / _attackRate;
            
            // 値のリセット.
            if (_isCharging)
                // TODO heartエフェクトを消す処理
                Destroy(_heartChargedEffect);
            _isCharging = false;
            // TODO エフェクトをDestroyする関数を呼ぶ
            Destroy(_chargingEffect);
            // chargeEffect.SetActive(false);
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

                // TODO エフェクトをDestroyする関数を呼ぶ
                Destroy(_chargingEffect);
                // chargeEffect.SetActive(false);
                // var heartChargedEffect = Instantiate(_heartChargedEffectPrefab, Vector3.zero, Quaternion.identity);
                if (_heartChargedEffect == null) {
                    _heartChargedEffect = Instantiate(_heartChargedEffectPrefab, Vector3.zero, Quaternion.identity);
                    _heartChargedEffect.transform.SetParent(this.transform, false);
                    _heartChargedEffect.transform.localPosition = Vector3.zero;
                }

                _damage = _normalDamage + 2;
                _attackRange = _normalAttackRange * 4.0f;
            } else {
                // var chargingEffect = Instantiate(_chargingEffectPrefab, Vector3.zero, Quaternion.identity);
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

    // private void Attack(Transform point)
    // {
    //     Collider2D[] hitInfos = Physics2D.OverlapCircleAll(point.position, attackRange, obstacleLayer);

    //     foreach (Collider2D hitInfo in hitInfos)
    //     {
    //         //WoodBox woodbox = hitInfo.transform.GetComponent<WoodBox>();
    //         Brick brick = hitInfo.transform.GetComponent<Brick>();
    //         Spike spike = hitInfo.transform.GetComponent<Spike>();
    //         Rose rose = hitInfo.transform.GetComponent<Rose>();
    //         Canon canon = hitInfo.transform.GetComponent<Canon>();
    //         Wolf wolf = hitInfo.transform.GetComponent<Wolf>();
    //         //daichi changed
    //         Halberd halberd = hitInfo.transform.GetComponent<Halberd>();
    //         Pig pig = hitInfo.transform.GetComponent<Pig>();
    //         PartsDestroy partsDestroy = hitInfo.transform.GetComponent<PartsDestroy>();
    //         Bushi bushi = hitInfo.transform.GetComponent<Bushi>();
    //         BuilderDestroy builderDestroy = hitInfo.transform.GetComponent<BuilderDestroy>();


    //         /*if (woodbox != null)
    //         {
    //             woodbox.TakeDamage(damage);
    //         }*/
    //         if (brick != null)
    //         {
    //             brick.TakeDamage(damage);
    //             Instantiate(obstaclesCrushEffect, hitInfo.transform.position, Quaternion.identity);
    //         }

    //         if (spike != null)
    //         {
    //             spike.TakeDamage(damage);
    //             Instantiate(obstaclesCrushEffect, hitInfo.transform.position, Quaternion.identity);
    //         }

    //         if (rose != null)
    //         {
    //             rose.TakeDamage(damage);
    //             Instantiate(obstaclesCrushEffect, hitInfo.transform.position, Quaternion.identity);
    //         }

    //         if (canon != null)
    //         {
    //             canon.TakeDamage(damage);
    //             Instantiate(obstaclesCrushEffect, hitInfo.transform.position, Quaternion.identity);
    //         }

    //         if (wolf != null)
    //         {
    //             wolf.TakeDamage(damage);
    //             Instantiate(obstaclesCrushEffect, hitInfo.transform.position, Quaternion.identity);
    //         }

    //         //daichi changed

    //         if (halberd != null)
    //         {
    //             halberd.TakeDamage(damage);
    //             Instantiate(obstaclesCrushEffect, hitInfo.transform.position, Quaternion.identity);
    //         }

    //         if (pig != null)
    //         {
    //             pig.TakeDamage(damage);
    //             Instantiate(obstaclesCrushEffect, hitInfo.transform.position, Quaternion.identity);
    //         }

    //         if (partsDestroy != null)
    //         {
    //             partsDestroy.TakeDamage(damage);
    //             Instantiate(obstaclesCrushEffect, hitInfo.transform.position, Quaternion.identity);
    //         }

    //         if (bushi != null)
    //         {
    //             bushi.TakeDamage(damage);
    //             Instantiate(obstaclesCrushEffect, hitInfo.transform.position, Quaternion.identity);
    //         }

    //         if (builderDestroy != null)
    //         {
    //             builderDestroy.TakeDamage(damage);
    //             Instantiate(obstaclesCrushEffect, hitInfo.transform.position, Quaternion.identity);
    //         }
    //     }
    // }
}