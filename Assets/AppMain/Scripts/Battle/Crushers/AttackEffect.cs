using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class AttackEffect : MonoBehaviour {
    private CancellationTokenSource _cancellationTokenSource = null;

    #region Serialized Fields
    [SerializeField, Header("飛距離")] private float _distance = 60.0f;
    [SerializeField] private float _existenceTime = 0.1f;
    [SerializeField] private int _damage = 2;
    [SerializeField] private GameObject _obstacleCrushEffect;
    #endregion

    private void Start() {
        this.transform.DOLocalMove(new Vector3(_distance, 0, 0), _existenceTime)
            .SetEase(Ease.Linear)
            .SetLink(this.gameObject)
            .OnComplete(() => Destroy(this.gameObject));
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
        var destroyableBuilder = collision.transform.GetComponent<DestroyableBuilder>();
        var chef = collision.transform.GetComponent<Chef>();
        var zakoWolf = collision.transform.GetComponent<ZakoWolf>();
        var pig = collision.transform.GetComponent<Pig>();
        var frog = collision.transform.GetComponent<Frog>();
        var bushi = collision.transform.GetComponent<Bushi>();
        var creamPuff = collision.transform.GetComponent<CreamPuff>();

        if (destroyableObstacle != null) {
            destroyableObstacle.TakeDamage(_damage);
            Instantiate(_obstacleCrushEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (destroyableBuilder != null) {
            destroyableBuilder.TakeDamage(1);
            Instantiate(_obstacleCrushEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (chef != null) {
            chef.TakeDamage(_damage);
            Instantiate(_obstacleCrushEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (zakoWolf != null) {
            zakoWolf.TakeDamage(_damage);
            Instantiate(_obstacleCrushEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (pig != null) {
            pig.TakeDamage(_damage);
            Instantiate(_obstacleCrushEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (frog != null) {
            frog.TakeDamage(_damage);
            Instantiate(_obstacleCrushEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (bushi != null) {
            bushi.TakeDamage(_damage);
            Instantiate(_obstacleCrushEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (creamPuff != null) {
            creamPuff.TakeDamage();
            Instantiate(_obstacleCrushEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
