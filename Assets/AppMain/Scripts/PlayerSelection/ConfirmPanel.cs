using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ConfirmPanel : MonoBehaviour {
    private bool _isAwaking = false;
    // private RectTransform _canvasGroupRectTransform = null;
    private Color _bgColor = Color.white;

    [SerializeField] private CrusherSelectionController _crusherSelectionController = null;
    [SerializeField] private Image _bg = null;
    // [SerializeField] private CanvasGroup _canvasGroup = null;

    private void Awake() {
        Debug.Log("Awake関数が呼ばれた");
        _isAwaking = true;
        // _canvasGroupRectTransform = _canvasGroup.GetComponent<RectTransform>();
    }

    private void OnEnable() {
        Debug.Log("Enable関数が呼ばれた");
        if (_isAwaking) {
            _isAwaking = false;
            return;
        }
        
        _bgColor = _bg.color;
        _bgColor.a = 0;
        _bg.color = _bgColor;
        _bg.DOFade(0.78f, 0.5f)
            .SetEase(Ease.Linear)
            .SetLink(_bg.gameObject);
            // .SetLink(_bg.gameObject)
            // .OnComplete(() => _crusherSelectionController.IsSettingConfirmPanel = false);
        
        // _canvasGroupRectTransform.anchoredPosition = new Vector2(0, -100.0f);
        // _canvasGroupRectTransform.DOAnchorPosY(0, 0.5f)
            // .SetEase(Ease.Linear)
            // .SetLink(_canvasGroupRectTransform.gameObject);

        // _canvasGroup.alpha = 0;
        // _canvasGroup.DOFade(1.0f, 0.5f)
            // .SetEase(Ease.Linear)
            // .SetLink(_canvasGroup.gameObject);
    }
}
