using UnityEngine;
using DG.Tweening;

public class FrogNote : MonoBehaviour {
    private Tweener _tweener = null;

    [SerializeField] private int _index = 0;

    private void Start() {
        switch (_index) {
            case 0:
                this.transform.DOLocalMove(new Vector3(-235.0f, 0, 0), 3.0f)
                    .SetEase(Ease.Linear)
                    .SetLink(this.gameObject)
                    .OnComplete(() => Destroy(this.gameObject));
                break;
            case 1:
                this.transform.DOLocalMove(new Vector3(-235.0f, 135.0f, 0), 3.0f)
                    .SetEase(Ease.Linear)
                    .SetLink(this.gameObject)
                    .OnComplete(() => Destroy(this.gameObject));
                break;
            case 2:
                this.transform.DOLocalMove(new Vector3(-235.0f, 407.0f, 0), 3.0f)
                    .SetEase(Ease.Linear)
                    .SetLink(this.gameObject)
                    .OnComplete(() => Destroy(this.gameObject));
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.collider.CompareTag("Obstacle"))
            Destroy(this.gameObject);
    }

    private void OnDestroy() {
        _tweener?.Kill();
    }
}
