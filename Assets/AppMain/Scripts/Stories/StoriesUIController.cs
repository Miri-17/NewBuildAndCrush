using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StoriesUIController : MonoBehaviour {
    private string _lowerRightSentence = "";
    private Color _textColor = Color.white;

    [SerializeField] private Image[] _fadeInOutImages = new Image[0];
    [SerializeField] private TextMeshProUGUI _lowerRightText = null;

    private void Start() {
        TransitionUI(false, 0.2f);

        // オープニングかエンディングかで, 最後に右下に表示するテキストの内容を変更する.
        if (GameDirector.Instance.IsOpening)
            _lowerRightSentence = "Yでバトルへ";
        else
            _lowerRightSentence = "おしまい";
    }

    /// <summary>
    /// ストーリーの最後に, 右下に表示するテキストを変更する.
    /// </summary>
    public void ChangeText() {
        _textColor = _lowerRightText.color;
        _textColor.a = 0;
        _lowerRightText.color = _textColor;
        _lowerRightText.text = _lowerRightSentence;
        _lowerRightText.DOFade(1.0f, 0.4f)
                .SetEase(Ease.Linear)
                .SetLink(_lowerRightText.gameObject);
    }

    /// <summary>
    /// ストーリーの最初にフェードアウト, ストーリーの最後にフェードインさせる.
    /// </summary>
    /// <param name="duration"></param>
    public void TransitionUI(bool isFadeIn, float duration) {
        var endValue = 0f;
        if (isFadeIn)
            endValue = 1.0f;
        
        foreach (var image in _fadeInOutImages) {
            image.DOFade(endValue, duration)
                .SetEase(Ease.Linear)
                .SetLink(image.gameObject);
        }
    }
}
