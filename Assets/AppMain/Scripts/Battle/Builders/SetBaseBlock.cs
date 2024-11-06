using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SetBaseBlock : MonoBehaviour {
    private GameObject _baseBlockPrefab = null;
    private SpawnPoint[,] _spawnPoints = new SpawnPoint[18, 9];
    private Dictionary<int, List<Vector2Int>> _patterns = new Dictionary<int, List<Vector2Int>>();

    #region Serialized Fields
    [SerializeField] private BuilderController _builderController = null;
    [SerializeField] private GameObject _spawnPointPrefab = null;
    [SerializeField] private float _startPosX = -272.0f;
    [SerializeField] private float _startPosY = 128.0f;
    [SerializeField] private float _space = 32.0f;
    [SerializeField] private List<GameObject> _baseBlockPrefabs = new List<GameObject>();
    [SerializeField] private List<TextAsset> _csvFiles = new List<TextAsset>();
    #endregion

    private void Start() {
        _baseBlockPrefab = _baseBlockPrefabs[GameDirector.Instance.BuilderIndex];
        LoadPatternsFromCSV();
        SetSpawnPoint();
    }

    private void LoadPatternsFromCSV() {
        // CSVの各行を読み込み、パターンごとに占有座標を保存
        var lines = _csvFiles[GameDirector.Instance.BuilderIndex].text.Split('\n');
        foreach (var line in lines) {
            if (string.IsNullOrWhiteSpace(line))
                continue;
            var values = line.Split(',');
            int patternId = int.Parse(values[0]);
            int row = int.Parse(values[1]);
            int col = int.Parse(values[2]);

            if (!_patterns.ContainsKey(patternId))
                _patterns[patternId] = new List<Vector2Int>();
            _patterns[patternId].Add(new Vector2Int(col, row));
        }
    }

    public void SetSpawnPoint() {
        _spawnPoints = new SpawnPoint[18, 9];

        // spawnPointsの初期化
        for (int j = 0; j < 9; j++) {
            for (int i = 0; i < 18; i++) {
                var spawnPoint = Instantiate(_spawnPointPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                spawnPoint.transform.parent = _builderController.Wagon.transform.Find("Grid").transform;
                spawnPoint.transform.localPosition = new Vector3(_startPosX + _space * i, _startPosY - _space * j, 0);
                _spawnPoints[i, j] = spawnPoint.GetComponent<SpawnPoint>();
                _spawnPoints[i, j].X = i;
                _spawnPoints[i, j].Y = j;
            }
        }

        // ランダムでパターンを選択し、配置
        int randomPattern = Random.Range(0, 4);
        ApplyPattern(randomPattern);
    }

    private void ApplyPattern(int patternId) {
        if (!_patterns.ContainsKey(patternId))
            return;

        foreach (var pos in _patterns[patternId]) {
            _spawnPoints[pos.x, pos.y].SetOccupied(true);
        }

        // isOccupiedがtrueの箇所にベースブロックを配置
        for (int j = 0; j < 9; j++) {
            for (int i = 0; i < 18; i++) {
                if (_spawnPoints[i, j].IsOccupied) {
                    var baseBlock = Instantiate(_baseBlockPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    baseBlock.transform.parent = _builderController.Wagon.transform;
                    baseBlock.transform.localPosition = _spawnPoints[i, j].transform.localPosition;
                }
            }
        }
    }
}
