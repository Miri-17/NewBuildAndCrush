using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ModeSelectionButton : MonoBehaviour {
    private List<RectTransform> _arrows = new List<RectTransform>();

    #region Serialized Fields
    [SerializeField] private Image _frameImage = null;
    [SerializeField] private Image _glowImage = null;
    [SerializeField] private Image[] _arrowImages = new Image[1];
    [SerializeField] private float[] _arrowPositions = new float[1];
    [SerializeField] private bool _isHorizontalButton = true;
    [Header("以下はないこともある")]
    [SerializeField] private Image _imageY = null;
    [SerializeField] private Image _glowImageY = null;
    #endregion
    
    public bool IsSelected { get; private set; } = false;

    private void Start() {
        int i = 0;
        if (_isHorizontalButton) {
            foreach (var arrowImage in _arrowImages) {
                RectTransform arrow = arrowImage.GetComponent<RectTransform>();
                _arrows.Add(arrow);

                arrow.DOAnchorPosX(_arrowPositions[i], 1.0f)
                    .SetEase(Ease.OutCubic)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetLink(arrow.gameObject);
                i++;
            }
        } else {
            foreach (var arrowImage in _arrowImages) {
                RectTransform arrow = arrowImage.GetComponent<RectTransform>();
                _arrows.Add(arrow);

                arrow.DOAnchorPosY(_arrowPositions[i], 1.0f)
                    .SetEase(Ease.OutCubic)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetLink(arrow.gameObject);
                i++;
            }
        }

        _glowImage.DOFade(1.0f, 2.0f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(_glowImage.gameObject);
        
        if (_glowImageY != null) {
            _glowImageY.DOFade(1.0f, 2.0f)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(_glowImageY.gameObject);
        }
    }

    public void SetSelection(bool isSelected) {
        IsSelected = isSelected;
        _frameImage.enabled = IsSelected;
        _glowImage.enabled = IsSelected;
        foreach (var arrowImage in _arrowImages)
            arrowImage.enabled = IsSelected;
        
        if (_glowImageY != null) {
            _imageY.enabled = IsSelected;
            _glowImageY.enabled = IsSelected;
        }
    }
}
