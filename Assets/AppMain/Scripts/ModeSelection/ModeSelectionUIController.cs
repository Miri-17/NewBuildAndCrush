using DG.Tweening;
using UnityEngine;

public class ModeSelectionUIController : MonoBehaviour {
    [SerializeField] private RectTransform _decoration = null;

    public bool IsAnimationEnded { get; private set; } = false;

    private void Start() {
        _decoration.DOAnchorPosY(101.0f, 0.5f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() => IsAnimationEnded = true);
    }
}
