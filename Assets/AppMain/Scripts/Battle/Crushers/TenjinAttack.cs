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

    // Tenjin固有
    [SerializeField] private GameObject _attackEffectPrefab;
    // [SerializeField] private float attackRange = 10.0f;
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
    }

    // private void Attack(Transform point)
    // {
    //     Collider2D[] hitInfos = Physics2D.OverlapCircleAll(point.position, attackRange);

        
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