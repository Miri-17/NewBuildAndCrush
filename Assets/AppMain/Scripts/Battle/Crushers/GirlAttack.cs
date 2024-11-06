using UnityEngine;

public class GirlAttack : MonoBehaviour {
    #region Private Fields
    private DirectionController _directionController = null;
    private Animator _animator = null;
    private AudioSource _audioSource = null;
    private float _nextAttackTime = 0;
    private bool _isHorizontal = false;
    #endregion

    #region Serialized Fields
    [SerializeField] private Transform _attackPoint = null;
    [SerializeField] private Transform _attackPointUp = null;
    [SerializeField] private Transform _attackPointDown = null;
    [SerializeField, Header("1秒に何回攻撃できるか")] private float _attackRate = 2.5f;
    [SerializeField] private int _damage = 2;
    [SerializeField] private float _attackRange = 100.0f;
    [SerializeField] private GameObject _obstacleCrushEffect = null;
    [SerializeField] private LayerMask _obstacleLayer;

    // Girl固有
    [SerializeField] private GameObject _bulletPrefab = null;
    #endregion

    private void Start() {
        _directionController = GameObject.FindWithTag("Direction").GetComponent<DirectionController>();
        _animator = this.GetComponent<Animator>();
        _audioSource = this.GetComponent<AudioSource>();
    }

    private void Update() {
        if (_directionController.IsDirection || Time.time < _nextAttackTime || !Input.GetButton("Fire1"))
            return;

        var verticalKey = Input.GetAxisRaw("Vertical");
        if (verticalKey > 0) {
            _isHorizontal = true;
            _animator.SetTrigger("Attack_Upwards");
            Attack(_attackPointUp);
            Instantiate(_bulletPrefab, _attackPointUp.transform);
        } else if (verticalKey < 0) {
            _isHorizontal = true;
            _animator.SetTrigger("Attack_Downwards");
            Attack(_attackPointDown);
            Instantiate(_bulletPrefab, _attackPointDown.transform);
        } else {
            Debug.Log("attack");
            _isHorizontal = false;
            _animator.SetTrigger("Attack");
            Attack(_attackPoint);
            Instantiate(_bulletPrefab, _attackPoint.transform);
        }
        _audioSource.PlayOneShot(_audioSource.clip);

        _nextAttackTime = Time.time + 1.0f / _attackRate;
    }

    private void Attack(Transform point) {
        float lineAngle = 0;
        if (_isHorizontal)
            lineAngle = 90.0f;

        Collider2D[] hitInfos = Physics2D.OverlapBoxAll(point.position, new Vector2(_attackRange, 1), lineAngle, _obstacleLayer);
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