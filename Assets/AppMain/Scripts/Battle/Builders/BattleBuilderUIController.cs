using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleBuilderUIController : MonoBehaviour {
    private float _currentWeight = 0;
    // 今回のビルダーの障害物情報を格納しておく変数
    private List<Image> _obstacleImages = new List<Image>();
    private List<float> _generationTime = new List<float>();
    private List<float> _obstacleWeight = new List<float>();
    private int _selectedIndex = 0;
    private List<bool> _isGeneration = new List<bool>() { false, false, false, false, false, false, };
    private float _maxWeight = 0;
    private bool _isPlaceable = true;

    private enum FACE_EXPRESSION {
        FINE,
        PALE,
        DEATHLY_PALE,
    }
    FACE_EXPRESSION _faceExpression = FACE_EXPRESSION.FINE;

    #region Serialized Fields
    [SerializeField] private BuilderController _builderController = null;
    [SerializeField] private List<Slider> _partsGenerationBars = new List<Slider>();
    [SerializeField] private Slider _weighingBar = null;
    [SerializeField] private List<Image> _faceImages = new List<Image>();
    [SerializeField] private List<Sprite> _faceSprites = new List<Sprite>();
    [SerializeField] private Button _goButton = null;
    [SerializeField] private List<Button> _obstacleButtons = new List<Button>();
    [SerializeField] private GameObject _warningPanel = null;
    #endregion

    public bool IsButtonDown { get; private set; } = false;
    public List<GameObject> ObstaclePrefabs { get; private set; } = new List<GameObject>();
    public GameObject CurrentPrefabs { get; private set; } = null;

    private void Start() {
        for (int i = 0; i < _obstacleButtons.Count; i++) {
            _obstacleImages.Add(null);
            ObstaclePrefabs.Add(null);
            _generationTime.Add(0.0f);
            _obstacleWeight.Add(0.0f);
        }

        _weighingBar.value = _currentWeight;
        _maxWeight = _weighingBar.maxValue;
        _faceImages[0].sprite = _faceSprites[0];
        _faceImages[1].sprite = _faceSprites[0];
        _goButton.onClick.AddListener(() => OnGoButtonClicked());

        for (int i = 0; i < _obstacleButtons.Count; i++) {
            ObstaclePrefabs[i] = _builderController.BattleBuilder.GetObstacle(i).ObstaclePrefab;
            _obstacleImages[i] = _obstacleButtons[i].GetComponent<Image>();
            _obstacleImages[i].sprite = _builderController.BattleBuilder.GetObstacle(i).ObstacleImage;
            _generationTime[i] = _builderController.BattleBuilder.GetObstacle(i).GenerationTime;
            _obstacleWeight[i] = _builderController.BattleBuilder.GetObstacle(i).ObstacleWeight;

            var index = i;
            _obstacleButtons[i].onClick.AddListener(() => OnObstacleButtonClicked(index));
        }
    }

    private void Update() {
        if (_weighingBar.value < 50) {
            if (_faceExpression != FACE_EXPRESSION.FINE) {
                _faceExpression = FACE_EXPRESSION.FINE;
                _faceImages[0].sprite = _faceSprites[0];
                _faceImages[1].sprite = _faceSprites[0];
            }
        } else if (_weighingBar.value < 80) {
            if (_faceExpression != FACE_EXPRESSION.PALE) {
                _faceExpression = FACE_EXPRESSION.PALE;
                _faceImages[0].sprite = _faceSprites[1];
                _faceImages[1].sprite = _faceSprites[1];
            }
        } else {
            if (_faceExpression != FACE_EXPRESSION.DEATHLY_PALE) {
                _faceExpression = FACE_EXPRESSION.DEATHLY_PALE;
                _faceImages[0].sprite = _faceSprites[2];
                _faceImages[1].sprite = _faceSprites[2];
            }
        }
    }

    private void OnGoButtonClicked() {
        // _goButton.interactable = false;
        SetGoButtonInteractive(false);

        // 次のワゴンが設置されるまで障害物を置けないようにする.
        _isPlaceable = false;

        // 重量を初期化する.
        _currentWeight = 0;
        _weighingBar.value = _currentWeight;

        // ワゴンを走らせる.
        _builderController.RunWagon();
    }

    private void OnObstacleButtonClicked(int index) {
        if (!_isPlaceable)
            return;
        
        // ボタンを押せない状態にする.
        foreach (var obstacleButton in _obstacleButtons) {
            obstacleButton.interactable = false;
        }
        // _goButton.interactable = false;
        SetGoButtonInteractive(false);

         // ワゴンのグリッドをアクティブにする.
        _builderController._wagon.transform.Find("Grid").transform.gameObject.SetActive(true);

        CurrentPrefabs = ObstaclePrefabs[index];
        // クリッカーを使える状態にする.
        IsButtonDown = true;

        _selectedIndex = index;
        // _builderController.wagonController.speed -= weight[GameDirector.Instance.BuilderIndex];
        // speed を90以下にはしない処理.
        // if (_builderController.wagonController.Speed <= 90) {
        //     _builderController.wagonController.Speed = 90;
        // }
    }

    // private void GetChildren(GameObject obj) {
    //     SpawnPoint children = obj.GetComponentInChildren<SpawnPoint>();
    //     if (children.childCount == 0) {
    //         return;
    //     }
    //     foreach (SpawnPoint)
    // }

    private void SetPartsGenerationBar(int index) {
        _isGeneration[index] = true;
        _partsGenerationBars[index].value = 0;

        _partsGenerationBars[index].DOValue(1.0f, _generationTime[index])
            .SetLink(_partsGenerationBars[index].gameObject)
            .OnComplete(() => { _obstacleButtons[index].interactable = true; _isGeneration[index] = false; });
    }

    private void SetWeighingBar(float weight) {
        _currentWeight += weight;
        if (_currentWeight > _maxWeight) {
            _isPlaceable = false;
            _currentWeight = _maxWeight;
            _warningPanel.SetActive(true);
        }

        _weighingBar.DOValue(_currentWeight, 0.5f);
    }

    /// <summary>
    /// 物を置いた直後のUI更新を行う
    /// </summary>
    public void SetButtonUp() {
        // クリッカーを使えない状態にする.
        IsButtonDown = false;

        // ワゴンのグリッドを非アクティブにする.
        _builderController._wagon.transform.Find("Grid").transform.gameObject.SetActive(false);
        
        // ボタンを押せる状態にする.
        for (int i = 0; i < _obstacleButtons.Count; i++) {
            if (i == _selectedIndex) {
                SetPartsGenerationBar(_selectedIndex);
                continue;
            }
            if (_isGeneration[i])
                continue;
            _obstacleButtons[i].interactable = true;
        }
        // _goButton.interactable = true;
        SetGoButtonInteractive(true);

        SetWeighingBar(_obstacleWeight[_selectedIndex]);
    }

    /// <summary>
    /// Go Buttonのinteractiveを変更する。
    /// </summary>
    /// <param name="interactable"></param>
    public void SetGoButtonInteractive(bool interactable) {
        _goButton.interactable = interactable;
    }

    public void SetObstaclePlaceable() {
        _isPlaceable = true;
    }
}
