using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AttackEffect : MonoBehaviour {
    private CancellationTokenSource _cancellationTokenSource = null;
    
    [SerializeField] private float _existenceTime = 0.1f;
    [SerializeField] private float _speed = 200.0f;
    [SerializeField] private int _damage = 2;
    [SerializeField] private GameObject _obstacleCrushEffect;

    private void Update() {
        this.transform.Translate(_speed * Time.deltaTime, 0, 0);
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

    private void OnCollisionEnter2D(Collision2D collision) {
        var destroyableObstacle = collision.transform.GetComponent<DestroyableObstacle>();

        if (destroyableObstacle != null) {
            destroyableObstacle.TakeDamage(_damage);
            Instantiate(_obstacleCrushEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        if (!collision.collider.CompareTag("Crusher") || !collision.collider.CompareTag("WitchAttack"))
            Destroy(this.gameObject);
    }
}
