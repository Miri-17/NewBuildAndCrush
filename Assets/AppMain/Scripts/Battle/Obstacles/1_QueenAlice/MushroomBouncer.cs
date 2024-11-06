using UnityEngine;
using DG.Tweening;

public class MushroomBouncer : MonoBehaviour {
    private ParticleSystem _particleSystem = null;
    private bool _firstBounced = false;
    private bool _isBounced = false;

    #region Serialized Fields
    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private AudioClip _audioClip = null;
    [SerializeField] private float _bounceTime = 1.0f;
    [SerializeField] private Vector2 _bounceScale = new Vector2(2.0f, 2.0f);
    #endregion

    private void Start() {
        _spriteRenderer.enabled = false;
        _particleSystem = this.GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Crusher")) {
            if (!_firstBounced) {
                _firstBounced = true;
                _particleSystem.Play();
                _spriteRenderer.enabled = true;
            }

            if (!_isBounced) {
                _isBounced = true;
                _animator.SetTrigger("Appear");
                _audioSource.PlayOneShot(_audioClip);
                this.transform.DOScale(_bounceScale, _bounceTime)
                    .SetEase(Ease.OutElastic)
                    .SetLink(this.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (_isBounced && other.CompareTag("Crusher")) {
            _isBounced = false;
            this.transform.DOScale(new Vector2(1.0f, 1.0f), _bounceTime)
                .SetEase(Ease.OutElastic)
                .SetLink(this.gameObject);
        }
    }
}
