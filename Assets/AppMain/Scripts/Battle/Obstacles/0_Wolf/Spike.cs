using UnityEngine;
using DG.Tweening;

public class Spike : MonoBehaviour {
    #region Private Fields
    private bool _isFalling = false;
    private Tween _tween = null;
    private BoxCollider2D _boxCollider2D = null;
    // 衝突結果を5個まで格納する.
    private Collider2D[] _results = new Collider2D[5];
    private bool _isTweenPlaying = false;
    #endregion

    [SerializeField] private float _finalPosition = -300.0f;
    [SerializeField] private float _duration = 1.0f;

    private void Start() {
        _boxCollider2D = this.GetComponent<BoxCollider2D>();

        _tween = this.transform.DOMoveY(_finalPosition, _duration)
            .SetEase(Ease.Linear)
            .SetRelative(true)
            .SetLink(this.gameObject)
            .Pause();
    }

    private void Update() {
        if (_tween == null || _boxCollider2D == null) return;

        // 落ちる状態であって、地面に接地していたら.
        if (_isTweenPlaying && _isFalling && IsHitToGround()) {
            Debug.Log("落ちるの止める");
            _isTweenPlaying = false;
            _tween.Pause();
        // 落ちる状態であって、地面に接地していなければ.
        } else if (!_isTweenPlaying && _isFalling && !IsHitToGround()) {
            Debug.Log("落ちるの再開");
            _isTweenPlaying = true;
            _tween.Play();
        }
    }

    private bool IsHitToGround() {
        int hitCount = _boxCollider2D.OverlapCollider(new ContactFilter2D(), _results);
        if (hitCount > 0) {
            for (int i = 0; i < hitCount; i++) {
                // 衝突したオブジェクトのLayerがGround or ObstacleGround.
                if (_results[i].gameObject.layer == 6 || _results[i].gameObject.layer == 8) return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!_isFalling && other.CompareTag("Crusher")) {
            _isFalling = true;
            Debug.Log("落ちる状態に遷移");
        }
    }
}
