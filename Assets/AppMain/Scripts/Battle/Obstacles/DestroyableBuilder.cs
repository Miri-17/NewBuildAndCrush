using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class DestroyableBuilder : MonoBehaviour {
    #region Private Fields
    private SpriteRenderer _spriteRenderer = null;
    private AudioSource _audioSource = null;
    private ParticleSystem _particleSystem = null;
    // private int i = 1;

    private Animator _animator = null;
    private bool _isDamaged = false;
    #endregion

    #region Serialized Fields
    [SerializeField] private List<Sprite> _obstacleSprites;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private bool _isUsingParticleSystem = false;
    [SerializeField] private int _defense = 4;
    // TODO そもそも、子オブジェクトは先に消す処理を行った方が良いかもしれん（Fanとかの風的に）
    [SerializeField] private List<BoxCollider2D> _boxCollider2Ds;
    [SerializeField] private List<CircleCollider2D> _circleCollider2Ds; // ZakoWolf
    [SerializeField] private List<CapsuleCollider2D> _capsuleCollider2Ds;
    [SerializeField] private float _duration = 0;
    #endregion

    private void Start() {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _particleSystem = this.GetComponent<ParticleSystem>();
        _animator = this.GetComponent<Animator>();

        _audioSource = this.GetComponent<AudioSource>();
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

        if (!_isDamaged && _defense < 5) {
            _isDamaged = true;
            SetDamageAnimation();
        } 
    }

    private void SetDamageAnimation() {
        _animator.SetBool("Damage", _isDamaged);
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
        
        Destroy(_spriteRenderer);
        
        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        
        Destroy(this.gameObject);
    }
}
