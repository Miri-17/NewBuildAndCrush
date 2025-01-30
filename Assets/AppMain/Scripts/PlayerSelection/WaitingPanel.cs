using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// TODO 時間があればBattleWaitingPanel.csと統合したい.
// バトルのWaiting Panelはプレハブ化して必要な時だけ生成するようにしているが, こちらは表示を切り替える設計にしてしまった.
// 元からプレハブ化するべきだったと反省.
public class WaitingPanel : MonoBehaviour {
    private RectTransform _canvasGroupRectTransform = null;
    private Color _bgColor = Color.white;

    #region Serialized Fields
    [SerializeField] private Image _bg = null;
    [SerializeField] private float _initialBgAlpha = 0;
    [SerializeField] private CanvasGroup _canvasGroup = null;
    #endregion

    private void Awake() {
        _canvasGroupRectTransform = _canvasGroup.GetComponent<RectTransform>();
    }

    private void OnEnable() {
        _bgColor = _bg.color;
        _bgColor.a = _initialBgAlpha;
        _bg.color = _bgColor;
        _bg.DOFade(0.78f, 0.5f)
            .SetEase(Ease.Linear)
            .SetLink(_bg.gameObject);
        
        _canvasGroupRectTransform.anchoredPosition = new Vector2(0, -100.0f);
        _canvasGroupRectTransform.DOAnchorPosY(0, 0.5f)
            .SetEase(Ease.Linear)
            .SetLink(_canvasGroupRectTransform.gameObject);

        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1.0f, 0.5f)
            .SetEase(Ease.Linear)
            .SetLink(_canvasGroup.gameObject);
    }
}
