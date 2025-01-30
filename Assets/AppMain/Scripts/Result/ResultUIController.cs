using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ResultUIController : MonoBehaviour {
    private const float START_TEXT_FADE_DURATION = 1.0f;

    #region Serialized Fields
    // スコア関係.
    [Header("どちらも勝ち, 負けの順")]
    [SerializeField] private Sprite[] _nameBgs = new Sprite[0];
    [SerializeField] private Sprite[] _frameSprites = new Sprite[0];
    [Header("すべてBuilder, Crusherの順")]
    [SerializeField] private Image[] _nameBgImages = new Image[0];
    [SerializeField] private Image[] _frameImages = new Image[0];
    [SerializeField] private TextMeshProUGUI[] _crusherScores = new TextMeshProUGUI[0];
    [SerializeField] private TextMeshProUGUI[] _builderScores = new TextMeshProUGUI[0];
    [SerializeField] private TextMeshProUGUI[] _crusherKillCounts = new TextMeshProUGUI[0];
    [SerializeField] private TextMeshProUGUI[] _wagonCrushCounts = new TextMeshProUGUI[0];
    [SerializeField] private Slider[] _scoreSliders = new Slider[0];
    // スコア関係以外.
    [SerializeField] private Image[] _fadeOutImages = new Image[0];
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
            _scoreSliders[0].value = crusherScore / sumScore;
            _scoreSliders[1].value = crusherScore / sumScore;
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
