using UnityEngine;
using Cysharp.Threading.Tasks;

public class CreamPuff : MonoBehaviour {
    #region Private Fields
    private CircleCollider2D _circleCollider2D = null;
    private SpriteRenderer _spriteRenderer = null;
    private AudioSource _audioSource = null;
    private bool _isDamaged = false;
    #endregion

    #region Serialized Fields
    [SerializeField] private Sprite _sprite = null;
    [SerializeField] private AudioClip _audioClip = null;
    [SerializeField] private float _duration = 2;
    [SerializeField] private GameObject _creamPuffFilterPrefab = null;
    #endregion

    private void Start() {
        _circleCollider2D = this.GetComponent<CircleCollider2D>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _audioSource = this.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!_isDamaged && other.collider.CompareTag("Crusher"))
            Crush(_duration).Forget();
    }

    /// <summary>
    /// ダメージを受ける処理.
    /// </summary>
    public void TakeDamage() {
        if (_isDamaged) return;

        Crush(_duration).Forget();
    }

    private async UniTaskVoid Crush(float duration) {
        if (_audioSource != null) {
            Debug.Log("オブジェクト破壊音がなった");
            _audioSource.PlayOneShot(_audioClip);
        }

        Destroy(_circleCollider2D);
        _spriteRenderer.sprite = _sprite;

        await UniTask.Delay((int)(0.2f * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        
        var prefab = Instantiate(_creamPuffFilterPrefab, GameObject.Find("DirectionPanel").transform);
        prefab.transform.localPosition = Vector3.zero;
        Destroy(_spriteRenderer);
        
        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        
        Destroy(this.gameObject);
    }
}
