using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreamPuffFilter : MonoBehaviour {
    private Image _image = null;

    private void Start() {
        _image = this.GetComponent<Image>();
        _image.DOFade(0, 10.0f)
            .SetLink(this.gameObject)
            .OnComplete(() => Destroy(this.gameObject));
    }
}
