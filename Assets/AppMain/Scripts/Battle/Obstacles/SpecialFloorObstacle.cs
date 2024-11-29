using Cysharp.Threading.Tasks;
using UnityEngine;

public class SpecialFloorObstacle : MonoBehaviour {
    #region Private Fields
    private SpriteRenderer _spriteRenderer = null;
    private AudioSource _audioSource = null;
    private ParticleSystem _particleSystem = null;
    private int i = 1;
    #endregion

    #region Serialized Fields
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private bool _isUsingParticleSystem = false;
    [SerializeField] private int _defense = 4;
    [SerializeField] private float _duration = 0;
    [SerializeField] private BoxCollider2D _boxCollider2D;
    [SerializeField] private GameObject _childGameObject = null;
    // Hatakeのみ.
    [SerializeField] private Sprite[] _obstacleSprites = new Sprite[0];
    #endregion

    private void Start() {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _audioSource = this.GetComponent<AudioSource>();
        _particleSystem = this.GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("GirlAttack"))
            TakeDamage(2);
        else if (collision.CompareTag("QueenOfHeartsAttack"))
            TakeDamage(3);
        else if (collision.CompareTag("TenjinAttack"))
            TakeDamage(1);
        else if (collision.CompareTag("WitchAttack"))
            TakeDamage(4);
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

        if (i < _obstacleSprites.Length) {
            _spriteRenderer.sprite = _obstacleSprites[i];
            i++;
        }
    }

    private async UniTaskVoid Crush(float duration) {
        if (_isUsingParticleSystem)
            _particleSystem.Play();

        Destroy(_boxCollider2D);
        Destroy(_childGameObject);
        Destroy(_spriteRenderer);
        
        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        
        Destroy(this.gameObject);
    }
}
