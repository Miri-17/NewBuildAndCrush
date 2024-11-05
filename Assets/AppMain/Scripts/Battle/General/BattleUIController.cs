using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUIController : MonoBehaviour {
   private const float END_POSITION = 5270.0f;

    #region Private Fields
    private int _builderUpdateScore = 0;
    private int _crusherUpdateScore = 0;
    #endregion

    #region Serialized Fields
    [Header("ビルダー側, クラッシャー側の順")]
    [SerializeField] private List<TextMeshProUGUI> _builderScores = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> _crusherScores = new List<TextMeshProUGUI>();
    [SerializeField] private List<RectTransform> _builderIcons = new List<RectTransform>();
    [SerializeField] private List<RectTransform> _crusherIcons = new List<RectTransform>();
    [SerializeField] private List<TextMeshProUGUI> _timeText = new List<TextMeshProUGUI>();
    [SerializeField] private float _currentTime = 600.0f;
    #endregion

    #region Public Properties
    public bool IsTimeUp { get; private set; } = false;
    public int CrusherCurrentScore { get; private set; } = 0;
    public int BuilderCurrentScore { get; private set; } = 0;
    #endregion

    private void Start() {
        _builderScores[0].text = BuilderCurrentScore.ToString();
        _builderScores[1].text = BuilderCurrentScore.ToString();
        _crusherScores[0].text = CrusherCurrentScore.ToString();
        _crusherScores[1].text = CrusherCurrentScore.ToString();
        
        _builderIcons[0].anchoredPosition = new Vector2(475.0f, -60.0f);
        // _builderIcons[0].anchoredPosition = new Vector2(467.0f, -2.0f);
        _builderIcons[1].anchoredPosition = new Vector2(-490.0f, 0);
        _crusherIcons[0].anchoredPosition = new Vector2(-490.0f, -60.0f);
        // _crusherIcons[0].anchoredPosition = new Vector2(-460.0f, -2.0f);
        _crusherIcons[1].anchoredPosition = new Vector2(-475.0f, 0);
    }

    private void Update() {
        if (IsTimeUp)
            return;

        TimerUpdate();
        IconPositionUpdate();
        ScoreUpdate();
    }

    private void TimerUpdate() {
        _currentTime -= Time.deltaTime;
        if (_currentTime <= 0) {
            _currentTime = 0;
            IsTimeUp = true;
            _timeText[0].text = "Time Up!";
            _timeText[0].fontSize = 60;
            _timeText[1].text = "Time Up!";
            _timeText[1].fontSize = 60;
            return;
        }

        _timeText[0].text = _currentTime.ToString("f");
        _timeText[1].text = _currentTime.ToString("f");
    }

    private void IconPositionUpdate() {
        // TODO 第一引数をなぜこれにしているのか要確認.
        _builderIcons[0].anchoredPosition = new Vector2(-560 + GameDirector.Instance.BuilderPosition * 958f / END_POSITION, -60.0f);
        // _builderIcons[0].anchoredPosition = new Vector2(-560 + GameDirector.Instance.BuilderPosition * 958f / END_POSITION, -2.0f);
        _builderIcons[1].anchoredPosition = new Vector2(-560 + GameDirector.Instance.BuilderPosition * 961.53f / END_POSITION, 0);
        _crusherIcons[0].anchoredPosition = new Vector2(-475 + GameDirector.Instance.CrusherPosition * 958f / END_POSITION, -60.0f);
        // _crusherIcons[0].anchoredPosition = new Vector2(-475 + GameDirector.Instance.CrusherPosition * 958f / END_POSITION, -2.0f);
        _crusherIcons[1].anchoredPosition = new Vector2(-475 + GameDirector.Instance.CrusherPosition  * 961.53f / END_POSITION, 0);
    }

    private void ScoreUpdate() {
        _builderUpdateScore = (int)((END_POSITION - GameDirector.Instance.BuilderPosition) * 999 / END_POSITION);
        if (_builderUpdateScore >= 0 && _builderUpdateScore < 1000 && _builderUpdateScore > BuilderCurrentScore) {
            BuilderCurrentScore = _builderUpdateScore;
            _builderScores[0].text = BuilderCurrentScore.ToString();
            _builderScores[1].text = BuilderCurrentScore.ToString();
        }

        _crusherUpdateScore = (int)(GameDirector.Instance.CrusherPosition * 999 / END_POSITION);
        if (_crusherUpdateScore < 1000 && _crusherUpdateScore > CrusherCurrentScore) {
            CrusherCurrentScore = _crusherUpdateScore;
            _crusherScores[0].text = CrusherCurrentScore.ToString();
            _crusherScores[1].text = CrusherCurrentScore.ToString();
        }
    }
}
