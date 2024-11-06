using UnityEngine;

public class BottleBeam : MonoBehaviour {
    private BoxCollider2D _boxCollider2D = null;
    private AudioSource _audioSource = null;

    private void Start() {
        _boxCollider2D = this.GetComponent<BoxCollider2D>();
        _audioSource = this.GetComponent<AudioSource>();
        _boxCollider2D.enabled = false;
    }

    /// <summary>
    /// コライダーを有効にする.
    /// </summary>
    public void EnableCollider() {
        if (_boxCollider2D == null) return;
        _audioSource.PlayOneShot(_audioSource.clip);
        _boxCollider2D.enabled = true;
    }

    /// <summary>
    /// コライダーを無効にする.
    /// </summary>
    public void DisableCollider() {
        if (_boxCollider2D == null) return;
        _boxCollider2D.enabled = false;
    }
}
