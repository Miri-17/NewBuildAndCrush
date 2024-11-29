using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ModeSelectionBook : MonoBehaviour {
    private List<RectTransform> _arrows = new List<RectTransform>();

    [SerializeField] private Image _selectedImage = null;
    [SerializeField] private Image _textImage = null;
    [SerializeField] private Image[] _arrowImages = new Image[0];
    [SerializeField] private float[] _arrowXPositions = new float[0];
    
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

        _selectedImage.DOFade(1.0f, 2.0f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(_selectedImage.gameObject);
        _textImage.DOFade(1.0f, 2.0f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(_textImage.gameObject);
    }

    public void SetSelection(bool isSelected) {
        IsSelected = isSelected;
        _selectedImage.enabled = IsSelected;
        _textImage.enabled = IsSelected;
        foreach (var arrowImage in _arrowImages)
            arrowImage.enabled = IsSelected;
    }
}
