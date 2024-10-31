using UnityEngine;
using DG.Tweening;

public class SoundListSelection : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private RectTransform _upArrow = null;
    [SerializeField] private RectTransform _downArrow = null;
    [SerializeField] private float _animationDuration = 0.8f;
    [SerializeField] private float _upArrowYPosition = 96.0f;
    [SerializeField] private float _downArrowYPosition = -96.0f;
    [SerializeField] private Ease _easeType = Ease.OutCubic;
    #endregion

    private void Start() {
        if (_upArrow == null || _downArrow == null) {
            Debug.LogError("Arrows are not assigned.");
            return;
        }

        AnimateArrow(_upArrow, _upArrowYPosition);
        AnimateArrow(_downArrow, _downArrowYPosition);
    }

    private void AnimateArrow(RectTransform arrow, float targetYPosition) {
        arrow.DOAnchorPosY(targetYPosition, _animationDuration)
            .SetEase(_easeType)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(arrow.gameObject);
    }
}
