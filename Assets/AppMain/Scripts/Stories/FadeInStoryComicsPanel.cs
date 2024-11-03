using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class FadeInStoryComicsPanel : MonoBehaviour {
    private int _counter = 0;
    private Image _image = null;
    private Color _imageColor = Color.white;

    [SerializeField] private Image _backComicsPanel = null;
    [SerializeField] private List<int> _pageSwitchingNumbers = new List<int>() {
        0, 
    };

    private void Awake() {
        _image = this.GetComponent<Image>();
        _backComicsPanel.enabled = false;
    }

    private void OnEnable() {
        _counter++;

        ResetImageAlpha();
        _image.DOFade(1.0f, 0.8f)
            .SetEase(Ease.Linear)
            .SetLink(_image.gameObject)
            .OnComplete(() => SetBackComicsPanel());
    }

    private void ResetImageAlpha() {
        _imageColor = _image.color;
        _imageColor.a = 0;
        _image.color = _imageColor;
    }

    private void SetBackComicsPanel() {
        _backComicsPanel.sprite = _image.sprite;

        if (!GameDirector.Instance.IsOpening) {
            if (CheckBuilderEndingConditions())
                return;
        } else {
            SetOpeningBackPanelVisibility();
        }
    }

    private bool CheckBuilderEndingConditions() {
        if ((_counter == 11 && GameDirector.Instance.BuilderIndex == 1 && !GameDirector.Instance.IsBuilderWin) ||
            (_counter == 8 && GameDirector.Instance.IsBuilderWin)) {
            _backComicsPanel.enabled = false;
            return true;
        }
        _backComicsPanel.enabled = true;
        return false;
    }

    private void SetOpeningBackPanelVisibility() {
        bool shouldEnable = _counter != GetMaxCounterForBuilder(GameDirector.Instance.BuilderIndex);
        _backComicsPanel.enabled = shouldEnable;
    }

    private int GetMaxCounterForBuilder(int builderIndex) {
        return builderIndex switch {
            0 => 12,
            1 => 13,
            2 => 8,
            _ => 7
        };
    }
    
    // private void SetBackComicsPanel() {
    //     _backComicsPanel.sprite = _image.sprite;

    //     if (!GameDirector.Instance.IsOpening) {
    //         if (_counter == 11 && GameDirector.Instance.BuilderIndex == 1 && !GameDirector.Instance.IsBuilderWin) {
    //             _backComicsPanel.enabled = false;
    //             return;
    //         } else if (_counter == 8 && GameDirector.Instance.IsBuilderWin) {
    //             _backComicsPanel.enabled = false;
    //             return;
    //         } else {
    //             _backComicsPanel.enabled = true;
    //             return;
    //         }
    //     }

    //     switch (GameDirector.Instance.BuilderIndex) {
    //         case 0:
    //             if (_counter == 12)
    //                 _backComicsPanel.enabled = false;
    //             else
    //                 _backComicsPanel.enabled = true;
    //             break;
    //         case 1:
    //             if (_counter == 13)
    //                 _backComicsPanel.enabled = false;
    //             else
    //                 _backComicsPanel.enabled = true;
    //             break;
    //         case 2:
    //             if (_counter == 8)
    //                 _backComicsPanel.enabled = false;
    //             else
    //                 _backComicsPanel.enabled = true;
    //             break;
    //         default:
    //             if (_counter == 7)
    //                 _backComicsPanel.enabled = false;
    //             else
    //                 _backComicsPanel.enabled = true;
    //             break;
    //     }
    // }
}
