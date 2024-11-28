using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Bushi : MonoBehaviour {
    #region Private Fields
    private AudioSource _audioSource = null;
    private Animator _animator = null;
    private bool _isDefended = true;
    private float _notionTime = 0;
    #endregion

    #region Serialized Fields
    [SerializeField] private GameObject _parentObject = null;
    [SerializeField] private BoxCollider2D _boxCollider2D = null;
    [SerializeField] private CapsuleCollider2D _capsuleCollider2D = null;
    [SerializeField, Header("0...Defend, 1...Crush")] private AudioClip[] _audioClip = new AudioClip[2];
    [SerializeField] private int _defense = 4;
    [SerializeField] private float _duration = 0;
    [SerializeField] private SpriteRenderer _notionSpriteRenderer = null;
    [SerializeField] private float _maxNotionTime = 1.0f;
    [SerializeField] private BoxCollider2D _swordCollider = null;
    [SerializeField] private GameObject[] _childGameObjects = null;
    #endregion

    private void Start() {
        _audioSource = this.GetComponent<AudioSource>();
        _animator = this.GetComponent<Animator>();
        _notionSpriteRenderer.enabled = false;
        _swordCollider.enabled = false;
    }

    private void Update() {
        if (!_isDefended && _notionSpriteRenderer.enabled && Time.time > _notionTime)
            _notionSpriteRenderer.enabled = false;
    }

    public void TakeDamage(int damage) {
        if (_isDefended) {
            Debug.Log("ガードアニメーション！");
            _audioSource.PlayOneShot(_audioClip[0]);
            _animator.SetTrigger("Defend");
            return;
        }

        if (_audioSource != null)
            _audioSource.PlayOneShot(_audioClip[1]);

        _defense -= damage;
        if (_defense <= 0) {
            Crush(_duration).Forget();
            return;
        }
    }

    private async UniTaskVoid Crush(float duration) {
        Destroy(_boxCollider2D);
        Destroy(_capsuleCollider2D);
        foreach (var childGameObject in _childGameObjects)
            Destroy(childGameObject);
        _animator.Play("Bushi_Defeat");
        
        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        
        Destroy(_parentObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Crusher")) {
            _notionSpriteRenderer.enabled = true;
            _notionTime = Time.time + _maxNotionTime;
            Debug.Log("Notion time: " + _notionTime);
            _isDefended = true;
            _animator.SetBool("Attack", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Crusher")) {
            _isDefended = true;
            _notionSpriteRenderer.enabled = false;
            _animator.SetBool("Attack", false);
        }
    }

    /// <summary>
    /// コライダーを有効にする.
    /// </summary>
    public void EnableCollider() {
        if (_swordCollider == null) return;
        _swordCollider.enabled = true;
    }

    /// <summary>
    /// コライダーを無効にし, 隙をつくる.
    /// </summary>
    public void DisableCollider() {
        if (_swordCollider == null) return;
        _swordCollider.enabled = false;
        _isDefended = false;
    }

    /// <summary>
    /// 隙をなくす.
    /// </summary>
    public void DefenceAgain() {
        _isDefended = true;
    }
}
