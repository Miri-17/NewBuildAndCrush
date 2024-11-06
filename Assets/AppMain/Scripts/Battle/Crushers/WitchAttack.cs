using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WitchAttack : MonoBehaviour {
    #region Private Fields
    private Animator _animator = null;
    private AudioSource _audioSource = null;
    private float _nextAttackTime = 0;
    #endregion

    #region Serialized Fields
    [SerializeField] private Transform _attackPoint = null;
    [SerializeField] private Transform _attackPointUp = null;
    [SerializeField] private Transform _attackPointDown = null;
    [SerializeField, Header("1秒に何回攻撃できるか")] private float _attackRate = 0.8f;
    [SerializeField] private int _damage = 2;
    [SerializeField] private float _attackRange = 10.0f;
    [SerializeField] private GameObject _obstacleCrushEffect;
    [SerializeField] private LayerMask _obstacleLayer;

    // Witch固有
    [SerializeField] private GameObject _cupcakePrefab;
    [SerializeField] private float _duration = 0.05f;
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
            Shoot(_attackPointUp).Forget();
        } else if (verticalKey < 0) {
            _animator.SetTrigger("Attack_Downwards");
            Attack(_attackPointDown);
            Shoot(_attackPointDown).Forget();
        } else {
            _animator.SetTrigger("Attack");
            Attack(_attackPoint);
            Shoot(_attackPoint).Forget();
        }
        _audioSource.PlayOneShot(_audioSource.clip);

        _nextAttackTime = Time.time + 1.0f / _attackRate;
    }

    private async UniTaskVoid Shoot(Transform point) {
        for (int i = 0; i < 3; i++) {
            Instantiate(_cupcakePrefab, point.transform);
            await UniTask.Delay(TimeSpan.FromSeconds(_duration));
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
