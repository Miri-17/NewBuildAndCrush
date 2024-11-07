using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultUIController : MonoBehaviour {
    private const float START_TEXT_FADE_DURATION = 1.0f;

    #region Score-related Serialized Fields
    [Header("どちらも勝ち, 負けの順")]
    [SerializeField] private List<Sprite> _nameBgs = new List<Sprite>();
    [SerializeField] private List<Sprite> _frameSprites = new List<Sprite>();
    [Header("すべてBuilder, Crusherの順")]
    [SerializeField] private List<Image> _nameBgImages = new List<Image>();
    [SerializeField] private List<Image> _frameImages = new List<Image>();
    [SerializeField] private List<TextMeshProUGUI> _crusherScores = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> _builderScores = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> _crusherKillCounts = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> _wagonCrushCounts = new List<TextMeshProUGUI>();
    [SerializeField] private List<Slider> _scoreSliders = new List<Slider>();
    #endregion

    #region Serialized Fields
    [SerializeField] private List<Image> _fadeOutImages = new List<Image>();
    #endregion

    public bool IsFadeOut { get; private set; } = false;

    private void Start() {
        SetBackgrounds();
        SetScoreUI(GameDirector.Instance.BuilderScore, GameDirector.Instance.CrusherScore);
        SetScoreSliders(GameDirector.Instance.BuilderScore, GameDirector.Instance.CrusherScore);

        StartFadeOUtAnimation();
    }

    private void SetBackgrounds() {
        bool isBuilderWin = GameDirector.Instance.IsBuilderWin;
        _nameBgImages[0].sprite = _nameBgs[isBuilderWin ? 0 : 1];
        _nameBgImages[1].sprite = _nameBgs[isBuilderWin ? 1 : 0];
        _frameImages[0].sprite = _frameSprites[isBuilderWin ? 0 : 1];
        _frameImages[1].sprite = _frameSprites[isBuilderWin ? 1 : 0];
    }

    private void SetScoreUI(int builderScore, int crusherScore) {
        for (int i = 0; i < 2; i++) {
            _builderScores[i].text = builderScore.ToString();
            _crusherScores[i].text = crusherScore.ToString();
            _crusherKillCounts[i].text = GameDirector.Instance.CrusherKillCounts.ToString();
            _wagonCrushCounts[i].text = GameDirector.Instance.WagonCrushCounts.ToString();
        }
    }

    private void SetScoreSliders(int builderScore, int crusherScore) {
        float sumScore = builderScore + crusherScore;
        if (sumScore > 0) {
            _scoreSliders[0].value = builderScore / sumScore;
            _scoreSliders[1].value = builderScore / sumScore;
        } else {
            _scoreSliders[0].value = 0.5f;
            _scoreSliders[1].value = 0.5f;
        }
    }

    private void StartFadeOUtAnimation() {
        var sequence = DOTween.Sequence();
        foreach (var image in _fadeOutImages) {
            sequence.Join(
                image.DOFade(0, 1.0f)
                    .SetEase(Ease.Linear)
            );
        }
        sequence.OnComplete(() => IsFadeOut = true);
    }
}
