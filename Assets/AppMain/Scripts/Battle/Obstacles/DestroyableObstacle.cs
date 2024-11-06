using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class DestroyableObstacle : MonoBehaviour {
    #region Private Fields
    private SpriteRenderer _spriteRenderer = null;
    private AudioSource _audioSource = null;
    private ParticleSystem _particleSystem = null;
    private Animator _animator = null;
    private int i = 1;
    #endregion

    #region Serialized Fields
    [SerializeField] private List<Sprite> _obstacleSprites;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private bool _isUsingParticleSystem = false;
    [SerializeField] private int _defense = 4;
    [SerializeField] private List<BoxCollider2D> _boxCollider2Ds;
    [SerializeField] private List<CircleCollider2D> _circleCollider2Ds;
    [SerializeField] private List<CapsuleCollider2D> _capsuleCollider2Ds;
    [SerializeField] private float _duration = 0;
    [SerializeField] private List<GameObject> _childGameObjects = null;
    [SerializeField] private bool _enableDefeatAnimation = false;
    #endregion

    /// <summary>
    /// Roseで使用.
    /// </summary>
    public bool IsDamaged { get; private set; } = false;

    private void Start() {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _audioSource = this.GetComponent<AudioSource>();
        _particleSystem = this.GetComponent<ParticleSystem>();

        if (!_enableDefeatAnimation) return;
        _animator = this.GetComponent<Animator>();
    }

    public void TakeDamage(int damage) {
        if (_audioSource != null) {
            Debug.Log("オブジェクト破壊音がなった");
            _audioSource.PlayOneShot(_audioClip);
        }

        _defense -= damage;
        if (_defense <= 0) {
            Crush(_duration).Forget();
            return;
        }

        IsDamaged = true;
        if (i < _obstacleSprites.Count) {
            _spriteRenderer.sprite = _obstacleSprites[i];
            i++;
        }
    }

    private async UniTaskVoid Crush(float duration) {
        if (_isUsingParticleSystem)
            _particleSystem.Play();

        // this.tag = "ObstacleGround";
        if (_boxCollider2Ds.Count > 0)
            foreach (var collider2D in _boxCollider2Ds)
                Destroy(collider2D);
        if (_circleCollider2Ds.Count > 0)
            foreach (var collider2D in _circleCollider2Ds)
                Destroy(collider2D);
        if (_capsuleCollider2Ds.Count > 0)
            foreach (var collider2D in _capsuleCollider2Ds)
                Destroy(collider2D);
        if (_childGameObjects.Count > 0)
            foreach (var childGameObject in _childGameObjects)
                Destroy(childGameObject);
        if (_enableDefeatAnimation)
            _animator.Play(this.gameObject.name + "_Defeat");
        else
            Destroy(_spriteRenderer);
        
        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        
        Destroy(this.gameObject);
    }
}
