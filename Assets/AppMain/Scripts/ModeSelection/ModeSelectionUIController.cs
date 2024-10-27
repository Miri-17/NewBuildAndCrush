using DG.Tweening;
using UnityEngine;

public class ModeSelectionUIController : MonoBehaviour {
    [SerializeField] private RectTransform _decoration = null;

    private void Start() {
        _decoration.DOAnchorPosY(101.0f, 0.5f)
            .SetEase(Ease.OutBounce);
            // .SetEase(Ease.OutBack);
    }
}
