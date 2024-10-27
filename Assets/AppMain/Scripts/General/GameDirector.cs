using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour {
    public static GameDirector Instance { get; private set; }

    #region
    [SerializeField] private int _crusherIndex = 0;
    [SerializeField] private int _builderIndex = 0;
    // [SerializeField] private int _crusherScore = 0;
    // [SerializeField] private int _builderScore = 0;
    // [SerializeField] private int _wagonCrushCounts = 0;
    // [SerializeField] private int _crusherKillCounts = 0;
    // [SerializeField] private float _timeLimit = 600.0f;
    // [SerializeField] private float _crusherPosition = 0;
    // [SerializeField] private float _builderPosition = 0;
    // [SerializeField] private float _currentFill = 0;
    // [SerializeField] private bool _crusherWin = true;
    // [SerializeField] private bool _builderWin = true;

    
    // ビルダー4体の順. 勝ったら0が1になる.
    [SerializeField] private List<int> _girlAchievements = new List<int>() { 0, 0, 0, 0, };
    [SerializeField] private List<int> _queenOfHeartsAchievements = new List<int>() { 0, 0, 0, 0, };
    [SerializeField] private List<int> _tenjinAchievements = new List<int>() { 0, 0, 0, 0, };
    [SerializeField] private List<int> _witchAchievements = new List<int>() { 0, 0, 0, 0, };
    // クラッシャー4体の順. 勝ったら0が1になる.
    [SerializeField] private List<int> _wolfAchievements = new List<int>() { 0, 0, 0, 0, };
    [SerializeField] private List<int> _queenAliceAchievements =new List<int>() { 0, 0, 0, 0, };
    [SerializeField] private List<int> _mikadoAchievements = new List<int>() { 0, 0, 0, 0, };
    [SerializeField] private List<int> _hanzelGretelAchievements = new List<int>() { 0, 0, 0, 0, };
    #endregion

    #region
    public int CrusherIndex { get => _crusherIndex; set => _crusherIndex = value; }
    public int BuilderIndex { get => _builderIndex; set => _builderIndex = value; }
    // public int CrusherScore { get => _crusherScore; set => _crusherScore = value; }
    // public int BuilderScore { get => _builderScore; set => _builderScore = value; }
    // public int WagonCrushCounts { get => _wagonCrushCounts; set => _wagonCrushCounts = value; }
    // public int CrusherKillCounts { get => _crusherKillCounts; set => _crusherKillCounts = value; }
    // public float TimeLimit { get => _timeLimit; set => _timeLimit = value; }
    // public float CrusherPosition { get => _crusherPosition; set => _crusherPosition = value; }
    // public float BuilderPosition { get => _builderPosition; set => _builderPosition = value; }
    // public float CurrentFill { get => _currentFill; set => _currentFill = value; }
    // public bool CrusherWin { get => _crusherWin; set => _crusherWin = value; }
    // public bool BuilderWin { get => _builderWin; set => _builderWin = value; }

    public List<int> GirlAchievements { get => _girlAchievements; set => _girlAchievements = value; }
    public List<int> QueenOfHeartsAchievements { get => _queenOfHeartsAchievements; set => _queenOfHeartsAchievements = value; }
    public List<int> TenjinAchievements { get => _tenjinAchievements; set => _tenjinAchievements = value; }
    public List<int> WitchAchievements { get => _witchAchievements; set => _witchAchievements = value; }
    public List<int> WolfAchievements { get => _wolfAchievements; set => _wolfAchievements = value; }
    public List<int> QueenAliceAchievements { get => _queenAliceAchievements; set => _queenAliceAchievements = value; }
    public List<int> MikadoAchievements { get => _mikadoAchievements; set => _mikadoAchievements = value; }
    public List<int> HanzelGretelAchievements { get => _hanzelGretelAchievements; set => _hanzelGretelAchievements = value; }
    #endregion

    private void Awake() {
        // すでにロードされていたら、自分自身を破棄して終了する.
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
