using UnityEngine;
using Cysharp.Threading.Tasks;

public class Chef : MonoBehaviour {
    #region Private Fields
    private Animator _animator = null;
    private AudioSource _audioSource = null;
    private float _nextAttackTime = 0;
    private int _counter = 0;
    #endregion

    #region Serialized Fields
    [SerializeField] private GameObject _parentObject = null;
    [SerializeField] private AudioClip _audioClip = null;
    [SerializeField] private GameObject _prefab = null;
    [SerializeField] private Transform _attackPoint = null;
    [SerializeField, Header("1秒に何回攻撃できるか")] private float _attackRate = 1.0f;
    [SerializeField, Header("何回ごとに休むか")] private int _maxCount = 3;
    [SerializeField] private CapsuleCollider2D _capsuleCollider2D = null;
    [SerializeField] private GameObject _childGameObject = null;
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private int _defense = 4;
    #endregion

    private void Start() {
        _animator = this.GetComponent<Animator>();
        _audioSource = this.GetComponent<AudioSource>();
    }

    private void Update() {
        if (Time.time < _nextAttackTime || _attackPoint == null)
            return;

        if (_counter == 0) {
            _animator.SetBool("Shoot", true);
            _nextAttackTime = Time.time + 1.0f / _attackRate;
            _counter++;
        } else {
            Instantiate(_prefab, _attackPoint.transform);
            
            _counter++;
            if (_counter > _maxCount) {
                _counter = 0;
                _animator.SetBool("Shoot", false);
            }

            _nextAttackTime = Time.time + 1.0f / _attackRate;
        }
    }

    public void TakeDamage(int damage) {
        if (_audioSource != null)
            _audioSource.PlayOneShot(_audioClip);

        _defense -= damage;
        if (_defense <= 0) {
            Crush(_duration).Forget();
            return;
        }
    }

    private async UniTaskVoid Crush(float duration) {
        _animator.SetTrigger("Defeat");
        Destroy(_capsuleCollider2D);
        Destroy(_childGameObject);
        
        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        
        Destroy(_parentObject);
    }
}
