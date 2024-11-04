using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithY : MonoBehaviour {
    private List<RectTransform> _arrows = new List<RectTransform>();

    [SerializeField] private Image _frameImage = null;
    [SerializeField] private Image _glowImage = null;
    [SerializeField] private List<Image> _arrowImages = new List<Image>();
    [SerializeField] private List<float> _arrowXPositions = new List<float>();
    [SerializeField] private Image _imageY = null;
    [SerializeField] private Image _glowImageY = null;
    
    public bool IsSelected { get; private set; } = false;

    private void Start() {
        int i = 0;
        foreach (var arrowImage in _arrowImages) {
            RectTransform arrow = arrowImage.GetComponent<RectTransform>();
            _arrows.Add(arrow);

            arrow.DOAnchorPosX(_arrowXPositions[i], 1.0f)
                .SetEase(Ease.OutCubic)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(arrow.gameObject);
            i++;
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
        
        if (_glowImageY) {
            _imageY.enabled = IsSelected;
            _glowImageY.enabled = IsSelected;
        }
    }
}
