using UnityEngine;
using DG.Tweening;

public class Arrow : Dish {
    private Tweener _tweener = null;

    [SerializeField] private Vector3 _rotation = new Vector3(0, 0, 10);

    protected override void Start() {
        base.Start();

        _tweener = this.transform.DOLocalRotate(_rotation, 0.5f)
            .SetLoops(-1, LoopType.Incremental)
            .SetLink(this.gameObject);
    }

    private void OnDestroy() {
        _tweener?.Kill();
    }
}
