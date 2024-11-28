using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUIController : MonoBehaviour {
    #region Private Fields
    private float _endPosition = 5270.0f;
    // private float _endPosition = 5734.0f;
    private int _builderUpdateScore = 0;
    private int _crusherUpdateScore = 0;
    private bool _isChangingScene = false;
    private float _currentTime = 0;
    #endregion

    #region Serialized Fields
    [SerializeField] private DirectionController _directionController = null;
    [Header("ビルダー側, クラッシャー側の順")]
    [SerializeField] private List<TextMeshProUGUI> _builderScores = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> _crusherScores = new List<TextMeshProUGUI>();
    [SerializeField] private List<RectTransform> _builderIcons = new List<RectTransform>();
    [SerializeField] private List<RectTransform> _crusherIcons = new List<RectTransform>();
    [SerializeField] private List<TextMeshProUGUI> _timeText = new List<TextMeshProUGUI>();
    #endregion

    #region Public Properties
    public bool IsTimeUp { get; private set; } = false;
    public int CrusherCurrentScore { get; private set; } = 0;
    public int BuilderCurrentScore { get; private set; } = 0;
    #endregion

    private void Start() {
        // 制限時間の初期化.
        _currentTime = GameDirector.Instance.LimitTime;
        _timeText[0].text = _currentTime.ToString("f");
        _timeText[1].text = _currentTime.ToString("f");

        GameDirector.Instance.CrusherPosition = 0;
        // GameDirector.Instance.BuilderPosition = 0;
        
        _builderScores[0].text = BuilderCurrentScore.ToString();
        _builderScores[1].text = BuilderCurrentScore.ToString();
        _crusherScores[0].text = CrusherCurrentScore.ToString();
        _crusherScores[1].text = CrusherCurrentScore.ToString();

        _builderIcons[0].anchoredPosition = new Vector2(467.0f, -2.0f);
        _builderIcons[1].anchoredPosition = new Vector2(467.0f, -2.0f);
        _crusherIcons[0].anchoredPosition = new Vector2(-460.0f, -2.0f);
        _crusherIcons[1].anchoredPosition = new Vector2(-460.0f, -2.0f);
    }

    private void Update() {
        if (_directionController.IsDirection || _isChangingScene || IsTimeUp) return;

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
        // クラッシャーのアイコン位置 + ビルダーの現在位置 * (ビルダーとクラッシャーのアイコン位置の合計) / 最初のビルダーの位置
        var builderIconPosition = -460f + GameDirector.Instance.BuilderPosition * 927f / 5734f;
        _builderIcons[0].anchoredPosition = new Vector2(builderIconPosition, -2.0f);
        _builderIcons[1].anchoredPosition = new Vector2(builderIconPosition, -2.0f);

        var crusherIconPosition = -460f + GameDirector.Instance.CrusherPosition * 927f / 5734f;
        if (crusherIconPosition >= -460) {
            _crusherIcons[0].anchoredPosition = new Vector2(crusherIconPosition, -2.0f);
            _crusherIcons[1].anchoredPosition = new Vector2(crusherIconPosition, -2.0f);
        }
    }

    private void ScoreUpdate() {
        _builderUpdateScore = (int)((_endPosition - GameDirector.Instance.BuilderPosition) * 999 / _endPosition);
        if (_builderUpdateScore >= 0 && _builderUpdateScore < 1000 && _builderUpdateScore > BuilderCurrentScore) {
            BuilderCurrentScore = _builderUpdateScore;
            _builderScores[0].text = BuilderCurrentScore.ToString();
            _builderScores[1].text = BuilderCurrentScore.ToString();
        }

        _crusherUpdateScore = (int)(GameDirector.Instance.CrusherPosition * 999 / _endPosition);
        if (_crusherUpdateScore < 1000 && _crusherUpdateScore > CrusherCurrentScore) {
            CrusherCurrentScore = _crusherUpdateScore;
            _crusherScores[0].text = CrusherCurrentScore.ToString();
            _crusherScores[1].text = CrusherCurrentScore.ToString();
        }
    }

    /// <summary>
    /// タイマーテキストをFinish!に変更し, 今後処理を受け付けない.
    /// </summary>
    public void SetFinishText() {
        _isChangingScene = true;
        _timeText[0].text = "Finish!";
        _timeText[1].text = "Finish!";
    }
}
