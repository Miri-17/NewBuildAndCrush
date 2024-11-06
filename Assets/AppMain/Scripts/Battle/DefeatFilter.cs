using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DefeatFilter : MonoBehaviour {
    private Image _image = null;

    private void Start() {
        _image = this.GetComponent<Image>();
        _image.DOFade(0.78f, 1.0f)
            .SetLink(this.gameObject)
            .OnComplete(FadeOutImage);
    }

    private void FadeOutImage() {
        _image.DOFade(0, 1.0f)
            .SetDelay(0.5f)
            .SetLink(this.gameObject)
            .OnComplete(() => Destroy(this.gameObject));
    }
}
