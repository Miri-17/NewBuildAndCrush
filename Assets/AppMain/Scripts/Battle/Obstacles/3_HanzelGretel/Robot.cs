using UnityEngine;
using DG.Tweening;

public class Robot : MonoBehaviour {
    private void Start() {
        var eulerAngles = this.transform.eulerAngles;
        eulerAngles.z = -10;
        this.transform.eulerAngles = eulerAngles;

        var sequence = DOTween.Sequence();
        sequence.Append(this.transform.DOMoveY(100, 2.0f).SetRelative(true).SetEase(Ease.InOutCubic))
            .Append(this.transform.DOLocalRotate(new Vector3(0, 0, 10), 0.5f).SetEase(Ease.InOutBack))
            .Append(this.transform.DOMoveY(-100, 2.0f).SetRelative(true).SetEase(Ease.InOutCubic))
            .Append(this.transform.DOLocalRotate(new Vector3(0, 0, -10), 0.5f).SetEase(Ease.InOutBack));
        sequence.SetLoops(-1, LoopType.Restart).SetLink(this.gameObject);
    }
}
