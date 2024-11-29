using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class Pig : MonoBehaviour {
    #region Private Fields
    private Tween _tween = null;
    private Animator _animator = null;
    private AudioSource _audioSource = null;
    private bool _isRunning = false;
    private float _tweenDuration = 0;
    #endregion

    #region Serialized Fields
    [SerializeField] private int _defense = 4;
    [SerializeField] private float _duration = 0.8f;
    [SerializeField] private BoxCollider2D _boxCollider2D = null;
    [SerializeField] private CapsuleCollider2D _capsuleCollider2D = null;
    [SerializeField] private float _endPosition = -272.0f;
    [SerializeField] private float _speed = 200.0f;
    #endregion

    private void Start() {
        _animator = this.GetComponent<Animator>();
        _audioSource = this.GetComponent<AudioSource>();
        _capsuleCollider2D.enabled = false;
        _tweenDuration = (this.transform.localPosition.x - _endPosition) / _speed;
        InitializeTween();
    }

    public void TakeDamage(int damage) {
        if (_audioSource != null)
            _audioSource.PlayOneShot(_audioSource.clip);

        _defense -= damage;
        if (_defense <= 0) {
            Crush(_duration).Forget();
            return;
        }

        if (!_isRunning) {
            _isRunning = true;
            _boxCollider2D.enabled = false;
            _capsuleCollider2D.enabled = true;
            this.gameObject.tag = "Obstacle";
            _animator.Play("Pig_Run");
            _tween.Play();
        }
    }

    private async UniTaskVoid Crush(float duration) {
        Destroy(_boxCollider2D);
        Destroy(_capsuleCollider2D);
        _tween.Pause();
        _animator.Play("Pig_Defeat");

        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        
        Destroy(this.gameObject);
    }

    private void InitializeTween() {
        _tween = this.transform.DOLocalMoveX(_endPosition, _tweenDuration)
            .SetEase(Ease.Linear)
            .SetLink(this.gameObject)
            .OnComplete(() => Destroy(this.gameObject))
            .Pause();
    }
}
