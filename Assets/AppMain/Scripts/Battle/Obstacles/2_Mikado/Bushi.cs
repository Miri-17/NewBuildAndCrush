using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Bushi : MonoBehaviour {
    #region Private Fields
    private AudioSource _audioSource = null;
    private Animator _animator = null;
    private bool _isGuarded = true;
    #endregion

    #region Serialized Fields
    [SerializeField] private AudioClip _audioClip = null;
    [SerializeField] private int _defense = 4;
    [SerializeField] private float _duration = 0;
    [SerializeField] private List<GameObject> _childGameObjects = null;
    #endregion

    private void Start() {
        _audioSource = this.GetComponent<AudioSource>();
        _animator = this.GetComponent<Animator>();
    }

    private void Update() {

    }

    public void TakeDamage(int damage) {
        if (_isGuarded) {
            // _animator.
            return;
        }

        if (_audioSource != null)
            _audioSource.PlayOneShot(_audioClip);

        _defense -= damage;
        if (_defense <= 0) {
            Crush(_duration).Forget();
            return;
        }
    }

    private async UniTaskVoid Crush(float duration) {
        // if (_boxCollider2Ds.Count > 0)
        //     foreach (var collider2D in _boxCollider2Ds)
        //         Destroy(collider2D);
        // if (_circleCollider2Ds.Count > 0)
        //     foreach (var collider2D in _circleCollider2Ds)
        //         Destroy(collider2D);
        // if (_capsuleCollider2Ds.Count > 0)
        //     foreach (var collider2D in _capsuleCollider2Ds)
        //         Destroy(collider2D);
        // if (_childGameObjects.Count > 0)
        foreach (var childGameObject in _childGameObjects)
            Destroy(childGameObject);
        // if (_enableDefeatAnimation)
        //     _animator.Play
        // (this.gameObject.name + "_Defeat");
        // else
        //     Destroy(_spriteRenderer);
        
        await UniTask.Delay((int)(duration * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
        
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Crusher")) return;

        _isGuarded = false;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Crusher"))
            _isGuarded = true;
    }
}
