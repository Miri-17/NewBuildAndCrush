using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleBuilderUIController : MonoBehaviour {
    #region Private Fields
    private List<Image> _obstacleImages = new List<Image>();
    private List<float> _generationTime = new List<float>();
    private List<float> _obstacleWeight = new List<float>();
    private Sprite[] _faceSprites = new Sprite[0];
    private Image _goButtonImage = null;
    // 今回のビルダーの障害物情報を格納しておく変数
    private int _selectedIndex = 0;
    private float _maxWeight = 0;
    private float _currentWeight = 0;
    private bool[] _isGeneration = new bool[6] { false, false, false, false, false, false, };
    private bool _isPlaceable = true;
    private bool _isPreparation = true;
    #endregion

    private enum FACE_EXPRESSION {
        FINE,
        PALE,
        DEATHLY_PALE,
    }
    FACE_EXPRESSION _faceExpression = FACE_EXPRESSION.FINE;

    #region Serialized Fields
    [SerializeField] private DirectionController _directionController = null;
    [SerializeField] private BuildersDB _buildersDB = null;
    [SerializeField] private Button[] _obstacleButtons = new Button[0];
    [SerializeField] private Slider[] _partsGenerationBars = new Slider[0];
    [SerializeField] private Image _frame = null;
    [SerializeField] private Image _facesImage = null;
    [SerializeField] private BuilderController _builderController = null;
    [SerializeField] private Slider _weighingBar = null;
    [SerializeField] private Button _goButton = null;
    [SerializeField] private Sprite _goButtonSprite = null;
    [SerializeField] private Sprite _goButtonDisabledSprite = null;
    [SerializeField] private GameObject _warningPanel = null;
    [SerializeField] private Image _warningPanelImage = null;
    [SerializeField] private Sprite _warningPanelForGoButtonSprite = null;
    // 現在は, 部下たちが辛くないのは重量50未満の時, 少し辛くなるのは重量50以上80未満の時, 辛いのは重量80以上の時.
    [SerializeField] private float[] _switchingWeights = new float[2] { 50.0f, 80.0f };
    #endregion

    #region Public Properties
    public List<GameObject> ObstaclePrefabs { get; private set; } = new List<GameObject>();
    public bool IsButtonDown { get; private set; } = false;
    public GameObject CurrentPrefabs { get; private set; } = null;
    #endregion

    private void Start() {
        for (int i = 0; i < _obstacleButtons.Length; i++) {
            _obstacleImages.Add(null);
            _generationTime.Add(0.0f);
            _obstacleWeight.Add(0.0f);
            ObstaclePrefabs.Add(null);
        }

        var battleBuilder = _buildersDB.GetBattleBuilder(GameDirector.Instance.BuilderIndex);
        _frame.sprite = battleBuilder.FrameSprite;
        _faceSprites = battleBuilder.FaceSprites;
        for (int i = 0; i < _obstacleButtons.Length; i++) {
            _obstacleImages[i] = _obstacleButtons[i].GetComponent<Image>();
            _obstacleImages[i].sprite = battleBuilder.GetObstacle(i).ObstacleImage;
            _generationTime[i] = battleBuilder.GetObstacle(i).GenerationTime;
            _obstacleWeight[i] = battleBuilder.GetObstacle(i).ObstacleWeight;
            ObstaclePrefabs[i] = battleBuilder.GetObstacle(i).ObstaclePrefab;

            var index = i;
            _obstacleButtons[i].onClick.AddListener(() => OnObstacleButtonClicked(index));
        }

        _facesImage.sprite = _faceSprites[0];
        _weighingBar.value = _currentWeight;
        _maxWeight = _weighingBar.maxValue;
        _goButtonImage = _goButton.GetComponent<Image>();
        _goButton.onClick.AddListener(() => OnGoButtonClicked());
        _warningPanel.SetActive(false);
    }

    private void Update() {
        // 部下たちが辛くない時.
        if (_weighingBar.value < _switchingWeights[0]) {
            if (_faceExpression != FACE_EXPRESSION.FINE) {
                _faceExpression = FACE_EXPRESSION.FINE;
                _facesImage.sprite = _faceSprites[0];
            }
        // 部下たちが少し辛くなる時.
        } else if (_weighingBar.value < _switchingWeights[1]) {
            if (_faceExpression != FACE_EXPRESSION.PALE) {
                _faceExpression = FACE_EXPRESSION.PALE;
                _facesImage.sprite = _faceSprites[1];
            }
        // 部下たちが辛い時.
        } else {
            if (_faceExpression != FACE_EXPRESSION.DEATHLY_PALE) {
                _faceExpression = FACE_EXPRESSION.DEATHLY_PALE;
                _facesImage.sprite = _faceSprites[2];
            }
        }
    }

    // 障害物ボタンを押した時の全ての障害物ボタンの制御を行う.
    private void OnObstacleButtonClicked(int index) {
        if (!_isPlaceable)
            return;
        
        // ボタンを押せない状態にする.
        foreach (var obstacleButton in _obstacleButtons) {
            obstacleButton.interactable = false;
        }
        SetGoButtonInteractive(false);

         // ワゴンのグリッドをアクティブにする.
        _builderController.Wagon.transform.Find("Grid").transform.gameObject.SetActive(true);

        CurrentPrefabs = ObstaclePrefabs[index];
        // クリッカーを使える状態にする.
        IsButtonDown = true;

        _selectedIndex = index;
    }

    // Goボタンを押した時のビルダー画面UIの制御を行う.
    public void OnGoButtonClicked() {
        if (!_isPreparation) {
            SetGoButtonInteractive(false);

            // 次のワゴンが設置されるまで障害物を置けないようにする.
            _isPlaceable = false;

            // ワゴンを走らせる.
            _builderController.RunWagon(_currentWeight);

            // 重量を初期化する.
            _currentWeight = 0;
            _weighingBar.value = _currentWeight;

            _warningPanel.SetActive(false);
        } else {
            _isPreparation = false;
            _goButtonImage.sprite = _goButtonSprite;
            var _goButtonSpriteState = _goButton.spriteState;
            _goButtonSpriteState.disabledSprite = _goButtonDisabledSprite;
            _goButton.spriteState = _goButtonSpriteState;
            _warningPanelImage.sprite = _warningPanelForGoButtonSprite;
            _directionController.BuilderReady();
        }
    }

    // ワゴンに載せた障害物が再度置けるか判断するためのバーの動きの制御を行う.
    private void SetPartsGenerationBar(int index) {
        _isGeneration[index] = true;
        _partsGenerationBars[index].value = 0;

        _partsGenerationBars[index].DOValue(1.0f, _generationTime[index])
            .SetEase(Ease.Linear)
            .SetLink(_partsGenerationBars[index].gameObject)
            .OnComplete(() => { _obstacleButtons[index].interactable = true; _isGeneration[index] = false; });
    }

    // ワゴンに載せた障害物によって重量を加算し, 重量バーの動きの制御を行う.
    private void SetWeighingBar(float weight) {
        _currentWeight += weight;
        if (_currentWeight > _maxWeight) {
            _isPlaceable = false;
            _currentWeight = _maxWeight;
            _warningPanel.SetActive(true);
        }

        _weighingBar.DOValue(_currentWeight, 0.5f)
            .SetEase(Ease.OutQuad)
            .SetLink(_weighingBar.gameObject);
    }

    /// <summary>
    /// 物を置いた直後のUI更新を行う.
    /// </summary>
    public void SetButtonUp() {
        // クリッカーを使えない状態にする.
        IsButtonDown = false;

        // ワゴンのグリッドを非アクティブにする.
        _builderController.Wagon.transform.Find("Grid").transform.gameObject.SetActive(false);
        
        // ボタンを押せる状態にする.
        for (int i = 0; i < _obstacleButtons.Length; i++) {
            if (i == _selectedIndex) {
                SetPartsGenerationBar(_selectedIndex);
                continue;
            }
            if (_isGeneration[i])
                continue;
            _obstacleButtons[i].interactable = true;
        }

        SetWeighingBar(_obstacleWeight[_selectedIndex]);

        if (_builderController.WagonControllerRun != null) return;
        // ワゴンが走っている時はこの処理を走らせない.
        SetGoButtonInteractive(true);
    }

    /// <summary>
    /// Go Buttonのinteractiveを変更する.
    /// </summary>
    /// <param name="interactable"></param>
    public void SetGoButtonInteractive(bool interactable) {
        _goButton.interactable = interactable;
    }

    /// <summary>
    /// ワゴンが再設置されたら呼び出される.
    /// </summary>
    public void SetObstaclePlaceable() {
        _isPlaceable = true;
    }
}
