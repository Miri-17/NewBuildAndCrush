using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreditsUIController : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private CreditsController _creditsController = null;
    [SerializeField] private RectTransform _scrollingObjects = null;
    [SerializeField] private float _endAnchorPosY = 7230.0f;
    [SerializeField] private float _time = 60.0f;
    [SerializeField] private List<Image> _fadeInImages = new List<Image>();
    #endregion

    public static float TransitionDuration { get; private set; } = 1.0f;

    private void Start() {
        _scrollingObjects.DOAnchorPosY(_endAnchorPosY, _time)
            .SetEase(Ease.Linear)
            .OnComplete(CreditsEnded)
            .SetLink(_scrollingObjects.gameObject);
    }

    public void FadeInImage() {
        var sequence = DOTween.Sequence();
        sequence.Append(
            _fadeInImages[0].DOFade(1.0f, TransitionDuration)
                .SetEase(Ease.Linear)
                .SetLink(_fadeInImages[0].gameObject)
        );
        sequence.Join(
            _fadeInImages[1].DOFade(1.0f, TransitionDuration)
                .SetEase(Ease.Linear)
                .SetLink(_fadeInImages[1].gameObject)
        );
    }

    private void CreditsEnded() {
        _creditsController.ChangeScene();
    }
}
