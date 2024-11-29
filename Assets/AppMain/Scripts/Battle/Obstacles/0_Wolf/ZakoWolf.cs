using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class ZakoWolf : MonoBehaviour {
    #region Private Fields
    private Tween _tween = null;
    private SpriteRenderer _spriteRenderer = null;
    private AudioSource _audioSource = null;
    private Animator _animator = null;
    private bool _isRunning = false;
    private float _tweenDuration = 0;
    #endregion

    #region Serialized Fields
    [SerializeField] private GameObject _parentObject = null;
    [SerializeField] private int _defense = 8;
    [SerializeField] private float _duration = 2.0f;
    [SerializeField] private BoxCollider2D[] _boxCollider2Ds = new BoxCollider2D[0];
    [SerializeField] private CircleCollider2D _circleCollider2D = null;
    [SerializeField] private float _endPosition = -272.0f;
    [SerializeField] private float _speed = 200.0f;
    #endregion

    private void Start() {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _audioSource = this.GetComponent<AudioSource>();
        _animator = this.GetComponent<Animator>();
        _endPosition -= _parentObject.transform.localPosition.x;
        _tweenDuration = (this.transform.localPosition.x - _endPosition) / _speed;
        InitializeTween();
    }

    public void TakeDamage(int damage) {
        if (_audioSource != null)
            _audioSource.PlayOneShot(_audioSource.clip);

        _defense -= damage;
        Debug.Log("WolfのDefense: " + _defense);
        if (_defense <= 0) {
            Crush(_duration).Forget();
            return;
        }
    }

    private async UniTaskVoid Crush(float duration) {
        foreach (var collider2D in _boxCollider2Ds)
            Destroy(collider2D);
        Destroy(_circleCollider2D);
        Destroy(_spriteRenderer);
        
        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        
        Destroy(_parentObject);
    }

    private void InitializeTween() {
        _tween = this.transform.DOLocalMoveX(_endPosition, _tweenDuration)
            .SetEase(Ease.Linear)
            .SetLink(this.gameObject)
            .OnComplete(() => Destroy(_parentObject))
            .Pause();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!_isRunning && other.CompareTag("Crusher")) {
            _isRunning = true;
            _animator.Play("ZakoWolf_Run");
            _tween.Play();
        }
    }
}
