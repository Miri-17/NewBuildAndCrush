using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private CancellationTokenSource _cancellationTokenSource = null;

    [SerializeField] private float _existenceTime = 0.02f;
    
    private void OnEnable() {
        _cancellationTokenSource = new CancellationTokenSource();
        DestroyBulletAsync(_cancellationTokenSource.Token).Forget();
    }

    private void OnDisable() {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }

    // 存在時間を超えた弾丸を消す.
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
