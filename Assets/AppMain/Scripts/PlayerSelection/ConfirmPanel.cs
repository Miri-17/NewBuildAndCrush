using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ConfirmPanel : MonoBehaviour {
    private RectTransform _canvasGroupRectTransform = null;
    private Color _bgColor = Color.white;

    #region Serialized Fields
    [SerializeField] private Image _bg = null;
    [SerializeField] private Animator _ribbonAnimator = null;
    [SerializeField] private CanvasGroup _canvasGroup = null;
    #endregion

    private void Awake() {
        _canvasGroupRectTransform = _canvasGroup.GetComponent<RectTransform>();
    }

    private void OnEnable() {
        _ribbonAnimator.Play("Ribbon_Move");

        _bgColor = _bg.color;
        _bgColor.a = 0;
        _bg.color = _bgColor;
        _bg.DOFade(0.78f, 0.6f)
            .SetEase(Ease.Linear)
            .SetLink(_bg.gameObject);
        
        _canvasGroupRectTransform.anchoredPosition = new Vector2(0, -100.0f);
        _canvasGroupRectTransform.DOAnchorPosY(0, 0.6f)
            .SetEase(Ease.Linear)
            .SetLink(_canvasGroupRectTransform.gameObject);

        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1.0f, 0.6f)
            .SetEase(Ease.Linear)
            .SetLink(_canvasGroup.gameObject);
    }
}
