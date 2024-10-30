using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class DestroyableObstacle : MonoBehaviour {
    #region Private Fields
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;
    private ParticleSystem _particleSystem;
    private int i = 1;
    #endregion

    #region Serialized Fields
    [SerializeField] private List<Sprite> _obstacleSprites;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private bool _isUsingParticleSystem = false;
    [SerializeField] private int _defense = 4;
    [SerializeField] private List<CapsuleCollider2D> _capsuleCollider2Ds;
    [SerializeField] private List<BoxCollider2D> _boxCollider2Ds;
    [SerializeField] private float _duration = 0;
    #endregion

    private void Start() {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _audioSource = this.GetComponent<AudioSource>();
        _particleSystem = this.GetComponent<ParticleSystem>();
    }

    public void TakeDamage(int damage) {
        if (_audioSource != null)
            _audioSource.PlayOneShot(_audioClip);

        _defense -= damage;
        if (_defense <= 0) {
            Crush(_duration).Forget();
            return;
        }

        if (i < _obstacleSprites.Count) {
            _spriteRenderer.sprite = _obstacleSprites[i];
            i++;
        }
    }

    private async UniTaskVoid Crush(float duration) {
        if (_isUsingParticleSystem)
            _particleSystem.Play();

        this.tag = "ObstacleGround";
        if (_boxCollider2Ds.Count > 0)
            foreach (var collider2D in _boxCollider2Ds)
                Destroy(collider2D);
        if (_capsuleCollider2Ds.Count > 0)
            foreach (var collider2D in _capsuleCollider2Ds)
                Destroy(collider2D);
        
        Destroy(_spriteRenderer);
        
        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        
        Destroy(this.gameObject);
    }
}
