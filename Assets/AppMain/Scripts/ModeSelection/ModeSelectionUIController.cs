using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectionUIController : MonoBehaviour {
    [SerializeField] private RectTransform _decoration = null;
    [SerializeField] private Image _title = null;

    public bool IsAnimationEnded { get; private set; } = false;

    private void Start() {
        _title.DOFade(1.0f, 0.5f)
            .SetEase(Ease.Linear)
            .SetLink(_title.gameObject);
        
        _decoration.DOAnchorPosY(101.0f, 0.5f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() => IsAnimationEnded = true);
    }
}
