using UnityEngine;
using DG.Tweening;

public class HalberdBlade : MonoBehaviour {
    private void Start() {
        this.transform.DOLocalRotate(new Vector3(0, 0, 360.0f), 1.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.InBack)
            .SetLoops(-1, LoopType.Restart)
            .SetLink(this.gameObject);
    }
}
