using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeInStoryComicsPanel : MonoBehaviour {
    #region Private Fields
    private int _counter = 0;
    private Image _image = null;
    private Color _imageColor = Color.white;
    #endregion

    [SerializeField] private Image _backComicsPanel = null;

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

    // コミックパネルのアルファ値を0にリセットする.
    private void ResetImageAlpha() {
        _imageColor = _image.color;
        _imageColor.a = 0;
        _image.color = _imageColor;
    }

    // すでにフェードインしたコミックパネルを, そのパネルの裏にも配置する.
    // これにより前のコマから次のコマだけをフェードインさせたような表現を可能にする.
    // 2ページ目に移る場合は裏に配置しない.
    private void SetBackComicsPanel() {
        _backComicsPanel.sprite = _image.sprite;

        if (!GameDirector.Instance.IsOpening) {
            if (CheckBuilderEndingConditions())
                return;
        } else {
            SetOpeningBackPanelVisibility();
        }
    }

    // アリスのエンディングは2ページ目があるので, それも判断する.
    private bool CheckBuilderEndingConditions() {
        Debug.Log("counter: " + _counter);
        if ((_counter == 11 && GameDirector.Instance.BuilderIndex == 1 && !GameDirector.Instance.IsBuilderWin) ||
            (_counter == 8 && GameDirector.Instance.IsBuilderWin)) {
            _backComicsPanel.enabled = false;
            return true;
        }
        _backComicsPanel.enabled = true;
        return false;
    }

    // オープニングの2ページ目かどうかを判断する.
    private void SetOpeningBackPanelVisibility() {
        bool shouldEnable = _counter != GetMaxCounterForBuilder(GameDirector.Instance.BuilderIndex);
        _backComicsPanel.enabled = shouldEnable;
    }

    // ビルダーのインデックスを受け取って, オープニングの2ページ目が何コマ目からか返す.
    private int GetMaxCounterForBuilder(int builderIndex) {
        return builderIndex switch {
            0 => 12,
            1 => 13,
            2 => 8,
            _ => 7
        };
    }
}
