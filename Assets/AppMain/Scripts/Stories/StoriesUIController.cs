using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StoriesUIController : MonoBehaviour {
    private string _lowerRightSentence = "";
    private Color _textColor = Color.white;

    [SerializeField] private List<Image> _fadeInOutImages = new List<Image>();
    [SerializeField] private TextMeshProUGUI _lowerRightText = null;

    private void Start() {
        foreach (var image in _fadeInOutImages) {
            image.DOFade(0, 0.2f)
                .SetEase(Ease.Linear)
                .SetLink(image.gameObject);
        }

        if (GameDirector.Instance.IsOpening)
            _lowerRightSentence = "Yでバトルへ";
        else
            _lowerRightSentence = "おしまい";
    }

    public void ChangeText() {
        _textColor = _lowerRightText.color;
        _textColor.a = 0;
        _lowerRightText.color = _textColor;
        _lowerRightText.text = _lowerRightSentence;
        _lowerRightText.DOFade(1.0f, 0.4f)
                .SetEase(Ease.Linear)
                .SetLink(_lowerRightText.gameObject);
    }

    public void TransitionUI(float duration) {
        foreach (var image in _fadeInOutImages) {
            image.DOFade(1.0f, duration)
                .SetEase(Ease.Linear)
                .SetLink(image.gameObject);
        }
    }
}
