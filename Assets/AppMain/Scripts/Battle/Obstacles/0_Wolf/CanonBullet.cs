using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class CanonBullet : MonoBehaviour {
    #region Private Fields
    private CancellationTokenSource _cancellationTokenSource = null;
    private CircleCollider2D _circleCollider2D = null;
    // 衝突結果を5個まで格納する.
    private Collider2D[] _results = new Collider2D[5];
    #endregion

    [SerializeField] private float _finalPosition = 100.0f;
    [SerializeField] private float _existenceTime = 0.5f;

    private void Start() {
        _circleCollider2D = this.GetComponent<CircleCollider2D>();
        
        this.transform.DOMoveY(_finalPosition, _existenceTime)
            .SetEase(Ease.Linear)
            .SetRelative(true)
            .SetLink(this.gameObject);
    }

    private void Update() {
        if (!IsHitToGround()) return;
        
        Destroy(this.gameObject);
    }

    private bool IsHitToGround() {
        int hitCount = _circleCollider2D.OverlapCollider(new ContactFilter2D(), _results);
        if (hitCount > 0) {
            for (int i = 0; i < hitCount; i++) {
                // 衝突したオブジェクトのLayerがGround.
                if (_results[i].gameObject.layer == 6) return true;
            }
        }
        return false;
    }

    private void OnEnable() {
        _cancellationTokenSource = new CancellationTokenSource();
        DestroyBulletAsync(_cancellationTokenSource.Token).Forget();
    }

    private void OnDisable() {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }

    private async UniTaskVoid DestroyBulletAsync(CancellationToken token) {
        try {
            await UniTask.Delay(TimeSpan.FromSeconds(_existenceTime), cancellationToken: token);
            if (!token.IsCancellationRequested) {
                Destroy(this.gameObject);
            }
        } catch (OperationCanceledException) {
            // 処理のキャンセルが行われた.
        }
    }
}
