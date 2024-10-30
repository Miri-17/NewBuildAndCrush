using UnityEngine;

public class TenjinAttack : MonoBehaviour {
    #region Private Fields
    private Animator _animator = null;
    private AudioSource _audioSource = null;
    private float _nextAttackTime = 0;
    #endregion

    #region Serialized Fields
    [SerializeField] private Transform _attackPoint = null;
    [SerializeField] private Transform _attackPointUp = null;
    [SerializeField] private Transform _attackPointDown = null;
    [SerializeField, Header("1秒に何回攻撃できるか")] private float _attackRate = 6.0f;
    [SerializeField] private int _damage = 2;
    [SerializeField] private float _attackRange = 10.0f;
    [SerializeField] private GameObject _obstacleCrushEffect;
    [SerializeField] private LayerMask _obstacleLayer;

    // Tenjin固有
    [SerializeField] private GameObject _attackEffectPrefab;
    #endregion


    private void Start() {
        _animator = this.GetComponent<Animator>();
        _audioSource = this.GetComponent<AudioSource>();
    }

    private void Update() {
        if (Time.time < _nextAttackTime || !Input.GetButton("Fire1"))
            return;

        var verticalKey = Input.GetAxisRaw("Vertical");
        if (verticalKey > 0) {
            _animator.SetTrigger("Attack_Upwards");
            Attack(_attackPointUp);
            Instantiate(_attackEffectPrefab, _attackPointUp.position, _attackPointUp.rotation);
        } else if (verticalKey < 0) {
            _animator.SetTrigger("Attack_Downwards");
            Attack(_attackPointDown);
            Instantiate(_attackEffectPrefab, _attackPointDown.position, _attackPointDown.rotation);
        } else {
            _animator.SetTrigger("Attack");
            Attack(_attackPoint);
            Instantiate(_attackEffectPrefab, _attackPoint.position, _attackPoint.rotation);
        }
        _audioSource.PlayOneShot(_audioSource.clip);

        _nextAttackTime = Time.time + 1.0f / _attackRate;
    }

    private void Attack(Transform point) {
        Collider2D[] hitInfos = Physics2D.OverlapCircleAll(point.position, _attackRange, _obstacleLayer);        
        foreach (Collider2D hitInfo in hitInfos) {
            var destroyableObstacle = hitInfo.transform.GetComponent<DestroyableObstacle>();
            // BuilderDestroy builderDestroy = hitInfo.transform.GetComponent<BuilderDestroy>();

            if (destroyableObstacle != null) {
                destroyableObstacle.TakeDamage(_damage);
                Instantiate(_obstacleCrushEffect, hitInfo.transform.position, Quaternion.identity);
            }

            // if (builderDestroy != null) {
            //     builderDestroy.TakeDamage(damage);
            //     Instantiate(obstaclesCrushEffect, hitInfo.transform.position, Quaternion.identity);
            // }
        }
    }
}