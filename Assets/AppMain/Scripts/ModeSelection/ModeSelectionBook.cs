using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectionBook : MonoBehaviour {
    private List<RectTransform> _arrows = new List<RectTransform>();

    [SerializeField] private Image _selectedImage = null;
    [SerializeField] private Image _textImage = null;
    [SerializeField] private List<Image> _arrowImages = new List<Image>();
    // TODO 後で名前を_arrowXPositionsに変更
    [SerializeField] private List<float> _arrowPosXs = new List<float>();
    
    public bool IsSelected { get; private set; } = false;

    private void Start() {
        int i = 0;
        foreach (var arrowImage in _arrowImages) {
            RectTransform arrow = arrowImage.GetComponent<RectTransform>();
            _arrows.Add(arrow);

            arrow.DOAnchorPosX(_arrowPosXs[i], 1.0f)
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
