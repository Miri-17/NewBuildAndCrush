using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BattleTimeSettings : MonoBehaviour {
    private int _limitTimeIndex = 0;
    private List<RectTransform> _arrows = new List<RectTransform>();

    #region
    [SerializeField] private Image _bg = null;
    [SerializeField] private Sprite[] _bgSprites = new Sprite[0];
    [SerializeField] private TextMeshProUGUI _valueText = null;
    [SerializeField] private float[] _limitTimes = new float[8] {
        180f, 240f, 300f, 360f, 420f, 480f, 540f, 600f,
    };
    [SerializeField] private Image[] _horizontalArrowImages = new Image[0];
    [SerializeField] private float[] _horizontalArrowPositions = new float[0];
    [SerializeField] private Image[] _verticalArrowImages = new Image[0];
    [SerializeField] private float[] _verticalArrowPositions = new float[0];
    #endregion

    public bool IsSelected { get; private set; } = false;

    private void Start() {
        // LINQ で _limitTimes のうち GameDirector の limitTime と等しい index を探す.
        _limitTimeIndex = Array.IndexOf(_limitTimes, GameDirector.Instance.LimitTime);
        // 見つからなければ 2 (5分) に設定し、GameDirector の limitTime もその時間にする.
        if (_limitTimeIndex == -1) {
            _limitTimeIndex = 2;
            GameDirector.Instance.LimitTime = _limitTimes[_limitTimeIndex];
        }
        
        _valueText.text = (_limitTimes[_limitTimeIndex] / 60).ToString() + "分";

        int i = 0;
        foreach (var arrowImage in _horizontalArrowImages) {
            RectTransform arrow = arrowImage.GetComponent<RectTransform>();
            _arrows.Add(arrow);

            arrow.DOAnchorPosX(_horizontalArrowPositions[i], 1.0f)
                .SetEase(Ease.OutCubic)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(arrow.gameObject);
            i++;
        }
        i = 0;
        foreach (var arrowImage in _verticalArrowImages) {
            RectTransform arrow = arrowImage.GetComponent<RectTransform>();
            _arrows.Add(arrow);

            arrow.DOAnchorPosY(_verticalArrowPositions[i], 1.0f)
                .SetEase(Ease.OutCubic)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(arrow.gameObject);
            i++;
        }
    }

    private void Update() {
        if (IsSelected && Input.GetButtonDown("Horizontal")) {
            var horizontalKey = Input.GetAxisRaw("Horizontal");
            if (horizontalKey > 0) {
                _limitTimeIndex++;
                if (_limitTimeIndex >= _limitTimes.Length)
                    _limitTimeIndex = 0;
            } else if (horizontalKey < 0) {
                _limitTimeIndex--;
                if (_limitTimeIndex < 0)
                    _limitTimeIndex = _limitTimes.Length - 1;
            }

            GameDirector.Instance.LimitTime = _limitTimes[_limitTimeIndex];
            _valueText.text = (_limitTimes[_limitTimeIndex] / 60).ToString() + "分";
        }
    }

    public void SetSelection(bool isSelected) {
        IsSelected = isSelected;

        if (isSelected)
            _bg.sprite = _bgSprites[1];
        else
            _bg.sprite = _bgSprites[0];
        
        foreach (var arrowImage in _horizontalArrowImages)
            arrowImage.enabled = IsSelected;
        foreach (var arrowImage in _verticalArrowImages)
            arrowImage.enabled = IsSelected;
    }
}
