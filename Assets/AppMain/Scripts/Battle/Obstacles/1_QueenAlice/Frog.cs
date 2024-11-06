using UnityEngine;
using Cysharp.Threading.Tasks;

public class Frog : MonoBehaviour {
    #region Private Fields
    private Animator _animator = null;
    private AudioSource _audioSource = null;
    private float _nextAttackTime = 0;
    private int _counter = 0;

    private bool _isSinging = false;
    private float _notionTime = 0;
    #endregion

    #region Serialized Fields
    [SerializeField, Header("0...Sing0, 1...Sing1, 2...Crush")] private AudioClip[] _audioClips = new AudioClip[3];
    [SerializeField] private GameObject[] _prefabs = new GameObject[3];
    [SerializeField] private Transform _attackPoint = null;
    [SerializeField, Header("1秒に何回攻撃できるか")] private float _attackRate = 1.0f;
    [SerializeField, Header("何回ごとに休むか")] private int _maxCount = 3;
    [SerializeField] private CircleCollider2D _circleCollider2D = null;
    [SerializeField] private GameObject[] _childGameObjects = new GameObject[2];
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private int _defense = 4;

    [SerializeField] private SpriteRenderer _notionSpriteRenderer = null;
    [SerializeField] private float _maxNotionTime = 1.0f;
    #endregion

    private void Start() {
        _animator = this.GetComponent<Animator>();
        _audioSource = this.GetComponent<AudioSource>();
    }

    private void Update() {
        if (_isSinging && _notionSpriteRenderer != null && _notionSpriteRenderer.enabled && Time.time > _notionTime)
            _notionSpriteRenderer.enabled = false;
        
        if (!_isSinging || Time.time < _nextAttackTime || _attackPoint == null)
            return;

        if (_counter == 0) {
            _animator.SetBool("Sing", true);
            _nextAttackTime = Time.time + 1.0f / _attackRate;
            _counter++;
        } else {
            Instantiate(_prefabs[_counter - 1], _attackPoint.transform);
            _counter++;

            if (_counter > _maxCount) {
                _audioSource.PlayOneShot(_audioClips[1]);
                _counter = 0;
                _animator.SetBool("Sing", false);
            }

            _audioSource.PlayOneShot(_audioClips[0]);
            _nextAttackTime = Time.time + 1.0f / _attackRate;
        }
    }

    public void TakeDamage(int damage) {
        if (_audioSource != null)
            _audioSource.PlayOneShot(_audioClips[2]);

        _defense -= damage;
        if (_defense <= 0) {
            Crush(_duration).Forget();
            return;
        }
    }

    private async UniTaskVoid Crush(float duration) {
        Destroy(_circleCollider2D);
        foreach(var childGameObject in _childGameObjects)
            Destroy(childGameObject);
        _animator.Play("Frog_Defeat");
        
        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Crusher")) {
            _isSinging = true;
            _notionSpriteRenderer.enabled = true;
            _notionTime = Time.time + _maxNotionTime;
            Debug.Log("Notion time: " + _notionTime);
            _counter = 0;
            // _animator.Play("Frog_Sing");
            // _animator.SetBool("Sing", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Crusher")) {
            _isSinging = false;
            _notionSpriteRenderer.enabled = false;
            // _animator.Play("Frog_Idle");
            _animator.SetBool("Sing", false);
        }
    }
}
