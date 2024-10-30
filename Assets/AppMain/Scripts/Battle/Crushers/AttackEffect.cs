using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AttackEffect : MonoBehaviour {
    private CancellationTokenSource _cancellationTokenSource = null;
    private Rigidbody2D _rb2D = null;
    
    [SerializeField] private float _existenceTime = 0.1f;
    [SerializeField] private float _speed = 200.0f;

    private void Start() {
        _rb2D = this.GetComponent<Rigidbody2D>();
        _rb2D.velocity = transform.right * _speed;
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
