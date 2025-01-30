using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class Cupcake : MonoBehaviour {
    private CancellationTokenSource _cancellationTokenSource = null;

    #region Serialized Fields
    [SerializeField, Header("飛距離")] private float _distance = 60.0f;
    [SerializeField] private float _existenceTime = 0.1f;
    [SerializeField] private int _damage = 2;
    [SerializeField] private GameObject _obstacleCrushEffect;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private float _attackRange = 3.0f;
    #endregion

    private void Start() {
        this.transform.DOLocalMove(new Vector3(_distance, 0, 0), _existenceTime)
            .SetEase(Ease.Linear)
            .SetLink(this.gameObject)
            .OnComplete(() => Destroy(this.gameObject));
    }
    
    private void OnEnable() {
        _cancellationTokenSource = new CancellationTokenSource();
        DestroyCupcakeAsync(_cancellationTokenSource.Token).Forget();
    }

    private void OnDisable() {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }

    // 存在時間を超えたカップケーキを消す.
    private async UniTaskVoid DestroyCupcakeAsync(CancellationToken token) {
        try {
            await UniTask.Delay(TimeSpan.FromSeconds(_existenceTime), cancellationToken: token);
            if (!token.IsCancellationRequested) {
                Destroy(this.gameObject);
            }
        } catch (OperationCanceledException) {
            // 処理のキャンセルが行われた.
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        Debug.Log("collision name: " + collision.gameObject.name);
        Collider2D[] hitInfos = Physics2D.OverlapCircleAll(this.transform.position, _attackRange, _obstacleLayer);
        foreach (Collider2D hitInfo in hitInfos) {
            var destroyableObstacle = hitInfo.transform.GetComponent<DestroyableObstacle>();
            var destroyableBuilder = hitInfo.transform.GetComponent<DestroyableBuilder>();
            var chef = hitInfo.transform.GetComponent<Chef>();
            var zakoWolf = hitInfo.transform.GetComponent<ZakoWolf>();
            var pig = hitInfo.transform.GetComponent<Pig>();
            var frog = hitInfo.transform.GetComponent<Frog>();
            var bushi = hitInfo.transform.GetComponent<Bushi>();
            var creamPuff = hitInfo.transform.GetComponent<CreamPuff>();

            if (destroyableObstacle != null) {
                Debug.Log("obstacle attack");
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
                Debug.Log("Bushi attack");
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
}
