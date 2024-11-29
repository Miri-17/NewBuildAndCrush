using UnityEngine;

public class TenjinAttack : MonoBehaviour {
    #region Private Fields
    private DirectionController _directionController = null;
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
            _animator.SetTrigger("Attack_Upwards");
            Instantiate(_attackEffectPrefab, _attackPointUp.transform);
        } else if (verticalKey < 0) {
            _animator.SetTrigger("Attack_Downwards");
            Instantiate(_attackEffectPrefab, _attackPointDown.transform);
        } else {
            _animator.SetTrigger("Attack");
            Instantiate(_attackEffectPrefab, _attackPoint.transform);
        }
        _audioSource.PlayOneShot(_audioSource.clip);

        _nextAttackTime = Time.time + 1.0f / _attackRate;
    }
}