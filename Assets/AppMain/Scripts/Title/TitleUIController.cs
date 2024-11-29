using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TitleUIController : MonoBehaviour {
    private const float START_TEXT_FADE_DURATION = 1.0f;
    private const float TITLE_SCALE = 10.0f;

    private Image _titleImage;

    #region Serialized Fields
    [SerializeField] private TextMeshProUGUI _startText = null;
    [SerializeField] private TextMeshProUGUI _copyright = null;
    [SerializeField] private RectTransform _title = null;
    [SerializeField] private Image _filter = null;
    #endregion

    public static float TransitionDuration { get; private set; } = 0.5f;

    private void Start() {
        if (_title == null || _startText == null || _filter == null) {
            Debug.LogError("UI elements are not assigned.");
            return;
        }

        _titleImage = _title.GetComponent<Image>();
        if (_titleImage == null) {
            Debug.LogError("Image component missing on title RectTransform.");
            return;
        }

        _startText.DOFade(0, START_TEXT_FADE_DURATION)
            .SetEase(Ease.InQuart)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(_startText.gameObject);
    }

    /// <summary>
    /// タイトルからシーン遷移する際のUI挙動の定義.
    /// </summary>
    public void TransitionUI() {
        _startText.enabled = false;
        _copyright.enabled = false;

        var sequence = DOTween.Sequence();
        sequence.Append(
            _title.DOScale(TITLE_SCALE, TransitionDuration)
                .SetEase(Ease.Linear)
                .SetLink(_title.gameObject)
        );
        sequence.Join(
            _titleImage.DOFade(0, TransitionDuration)
                .SetEase(Ease.Linear)
                .SetLink(_titleImage.gameObject)
        );
        sequence.Join(
            _filter.DOFade(0, TransitionDuration)
                .SetEase(Ease.Linear)
                .SetLink(_filter.gameObject)
        );
    }
}
