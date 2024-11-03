using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeInComicsPanel : MonoBehaviour {
    private Image _image = null;
    private Color _imageColor = Color.white;

    private void Awake() {
        _image = this.GetComponent<Image>();
    }

    private void OnEnable() {
        _imageColor = _image.color;
        _imageColor.a = 0;
        _image.color = _imageColor;
        _image.DOFade(1.0f, 0.8f)
            .SetEase(Ease.Linear)
            .SetLink(_image.gameObject);
    }
}
