using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleBuilderUIController : MonoBehaviour {
    private float _currentWeight = 0;
    // 今回のビルダーの障害物情報を格納しておく変数
    private List<Image> _obstacleImages = new List<Image>();
    private List<GameObject> _obstaclePrefabs = new List<GameObject>();
    private List<float> _generationTime = new List<float>();

    private enum FACE_EXPRESSION {
        FINE,
        PALE,
        DEATHLY_PALE,
    }
    FACE_EXPRESSION _faceExpression = FACE_EXPRESSION.FINE;

    [SerializeField] private BuilderController _builderController = null;
    [SerializeField] private List<Slider> _partsGenerationBars = new List<Slider>();
    [SerializeField] private Slider _weighingBar = null;
    [SerializeField] private List<Image> _faceImages = new List<Image>();
    [SerializeField] private List<Sprite> _faceSprites = new List<Sprite>();
    [SerializeField] private Button _goButton = null;
    [SerializeField] private List<Button> _obstacleButtons = new List<Button>();

    private void Start() {
        for (int i = 0; i < _obstacleButtons.Count; i++) {
            _obstacleImages.Add(null);
            _obstaclePrefabs.Add(null);
            _generationTime.Add(0.0f);
        }

        _weighingBar.value = _currentWeight;
        _faceImages[0].sprite = _faceSprites[0];
        _faceImages[1].sprite = _faceSprites[0];
        _goButton.onClick.AddListener(() => OnGoButtonClicked());

        for (int i = 0; i < _obstacleButtons.Count; i++) {
            _obstaclePrefabs[i] = _builderController.BattleBuilder.GetObstacle(i).ObstaclePrefab;
            _obstacleImages[i] = _obstacleButtons[i].GetComponent<Image>();
            _obstacleImages[i].sprite = _builderController.BattleBuilder.GetObstacle(i).ObstacleImage;
            _generationTime[i] = _builderController.BattleBuilder.GetObstacle(i).GenerationTime;

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
        _goButton.interactable = false;
        _currentWeight = 0;
        _weighingBar.value = _currentWeight;

        _builderController.RunWagon();
    }

    // TODO ObstacleButtonManagerより
    private void OnObstacleButtonClicked(int index) {
        _obstacleButtons[index].interactable = false;

        // TODO 
        _builderController._wagon.transform.Find("Grid").transform.gameObject.SetActive(true);
        // _builderController.wagonController.speed -= weight[GameDirector.Instance.BuilderIndex];
        // speed を90以下にはしない処理.
        if (_builderController.wagonController.Speed <= 90) {
            _builderController.wagonController.Speed = 90;
        }
    }

    private void SetPartsGenerationBar(int index) {
        _partsGenerationBars[index].value = 0;

        _partsGenerationBars[index].DOValue(1.0f, _generationTime[index])
            .SetLink(_partsGenerationBars[index].gameObject)
            .OnComplete(() => _obstacleButtons[index].interactable = true);
    }

    private void SetWeighingBar() {
        // TODO ものの重さを入れる
        var x = 0;
        _weighingBar.DOValue(_currentWeight + x, 0.5f);
    }
}
