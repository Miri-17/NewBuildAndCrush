using UnityEngine;

public class GirlAttack : MonoBehaviour {
    #region Private Fields
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

    // Girl固有
    [SerializeField] private GameObject _bulletPrefab = null;
    // [SerializeField] private float attackRange = 100.0f;
    // [SerializeField] private LayerMask obstacleLayer;
    // [SerializeField] private int damage = 2;
    // [SerializeField] private GameObject obstaclesCrushEffect;
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
            _isHorizontal = true;
            _animator.SetTrigger("Attack_Upwards");
            // Attack(_attackPointUp);
            Instantiate(_bulletPrefab, _attackPointUp.transform);
        } else if (verticalKey < 0) {
            _isHorizontal = true;
            _animator.SetTrigger("Attack_Downwards");
            // Attack(_attackPointDown);
            Instantiate(_bulletPrefab, _attackPointDown.transform);
        } else {
            Debug.Log("attack");
            _isHorizontal = false;
            _animator.SetTrigger("Attack");
            // Attack(_attackPoint);
            Instantiate(_bulletPrefab, _attackPoint.transform);
        }
        _audioSource.PlayOneShot(_audioSource.clip);

        _nextAttackTime = Time.time + 1.0f / _attackRate;
    }

    // private void Attack(Transform point) {
    //     float lineAngle = 0;
    //     if (isHorizon)
    //         lineAngle = 90;
    //     else
    //         lineAngle = 0;

    //     Collider2D[] hitInfos = Physics2D.OverlapBoxAll(point.position, new Vector2(attackRange, 1), lineAngle);


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