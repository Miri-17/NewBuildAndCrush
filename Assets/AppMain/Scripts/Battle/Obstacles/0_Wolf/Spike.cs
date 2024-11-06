using UnityEngine;
using DG.Tweening;

public class Spike : MonoBehaviour {
    private bool _isFalling = false;
    private Tween _tween = null;
    private BoxCollider2D _boxCollider2D = null;
    // 衝突結果を5個まで格納する.
    private Collider2D[] _results = new Collider2D[5];

    [SerializeField] private float _finalPosition = -300.0f;
    [SerializeField] private float _duration = 1.0f;

    private void Start() {
        _boxCollider2D = this.GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if (!_isFalling || _tween == null || !IsHitToGround()) return;

        _isFalling = false;
        _tween.Kill();
    }

    private bool IsHitToGround() {
        int hitCount = _boxCollider2D.OverlapCollider(new ContactFilter2D(), _results);
        if (hitCount > 0) {
            for (int i = 0; i < hitCount; i++) {
                // 衝突したオブジェクトのLayerがGround.
                Debug.Log(_results[i].gameObject.layer);
                if (_results[i].gameObject.layer == 6) return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!_isFalling && other.CompareTag("Crusher")) {
            _isFalling = true;
            SpikeFall();
        }
    }

    private void SpikeFall() {
        _tween = this.transform.DOMoveY(_finalPosition, _duration)
            .SetEase(Ease.Linear)
            .SetRelative(true)
            .SetLink(this.gameObject);
    }
}
