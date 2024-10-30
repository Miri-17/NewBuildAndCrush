using UnityEngine;

// TODO SetBaseBlock~~クラスをまとめたい
public class SetBaseBlockWolf : MonoBehaviour {
    private SpawnPoint[,] _spawnPoints = new SpawnPoint[18, 9];

    #region Serialized Fields
    [SerializeField] private BuilderController _builderController;
    [SerializeField] private GameObject _spawnPointPrefab;
    [SerializeField] private float _startPosX = -272.0f;
    [SerializeField] private float _startPosY = 128.0f;
    [SerializeField] private float _space = 32.0f;

    // キャラ固有
    [SerializeField] private GameObject _baseBlockPrefab;
    #endregion

    // ゲームスタートと同時にspawnPointのインスタンスを作成し、spawnPointsXYに代入する
    public void SetSpawnPoint() {
        _spawnPoints = new SpawnPoint[18, 9];

        for (int j = 0; j < 9; j++) {
            for (int i = 0; i < 18; i++) {
                GameObject spawnPoint = Instantiate(_spawnPointPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                spawnPoint.transform.parent = _builderController._wagon.transform.Find("Grid").transform;
                spawnPoint.transform.localPosition = new Vector3(_startPosX + _space * i, _startPosY - _space * j, 0);
                _spawnPoints[i, j] = spawnPoint.GetComponent<SpawnPoint>();
                _spawnPoints[i, j].x = i;
                _spawnPoints[i, j].y = j;
            }
        }

        int randomNumber = Random.Range(0, 4);
        switch(randomNumber) {
            case 0:
                SetBaseBlock0();
                break;
            case 1:
                SetBaseBlock1();
                break;
            case 2:
                SetBaseBlock2();
                break;
            default:
                SetBaseBlock3();
                break;
        }
    }

    private void SetBaseBlock0() {
        //   0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17
        // 0 ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■
        // 1 ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■
        // 2 ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■
        // 3 ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■
        // 4 ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■
        // 5 □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □
        // 6 □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □
        // 7 □  □  ■  ■  □  □  ■  ■  □  □  ■  ■  □  □  ■  ■  □  □
        // 8 □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □

        // ベースブロックが配置されるマスのspawnPointのisOccupiedをtrueにしておく
        for (int j = 0; j <= 4; j++) {
            for (int i = 0; i < 18; i++) {
                _spawnPoints[i, j].isOccupied = true;
            }
        }
        for (int i = 2; i <= 15; i++) {
            if (i % 3 == 2 || i % 3 == 0) {
                _spawnPoints[i, 7].isOccupied = true;
            }
        }

        // isOccupiedがtrueのところにベースブロックを配置する
        for (int j = 0; j < 9; j++) {
            for (int i = 0; i < 18; i++) {
                if (_spawnPoints[i, j].isOccupied == true) {
                    GameObject _baseBlock = Instantiate(_baseBlockPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    _baseBlock.transform.parent = _builderController._wagon.transform;
                    _baseBlock.transform.localPosition = _spawnPoints[i, j].transform.localPosition;
                }
            }
        }
    }

    private void SetBaseBlock1() {
        //   0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17
        // 0 □  □  □  □  □  □  □  □  □  ■  ■  ■  ■  ■  ■  ■  ■  ■
        // 1 □  □  □  □  □  □  □  □  □  ■  ■  ■  ■  ■  ■  ■  ■  ■
        // 2 □  □  □  □  ■  □  □  □  □  □  ■  ■  ■  ■  ■  ■  ■  ■
        // 3 □  □  □  ■  ■  ■  □  □  □  □  □  ■  ■  ■  ■  ■  ■  ■
        // 4 □  □  ■  ■  ■  ■  ■  □  □  □  □  □  ■  ■  ■  ■  ■  ■
        // 5 □  □  □  ■  ■  ■  ■  ■  □  □  □  □  □  ■  ■  ■  □  □
        // 6 ■  □  □  □  ■  ■  ■  ■  ■  □  □  □  □  □  ■  □  □  □
        // 7 □  □  □  □  □  ■  ■  ■  ■  ■  □  □  □  □  □  □  □  □
        // 8 □  □  ■  ■  ■  ■  ■  ■  ■  ■  ■  □  □  □  □  □  □  □

        // ベースブロックが配置されるマスのspawnPointのisOccupiedをtrueにしておく
        for (int j = 0; j < 9; j++) {
            for (int i = 0; i < 18; i++) {
                if (j <= 1 && i >= 9 && i < 18) _spawnPoints[i, j].isOccupied = true;
                if (j == 2 && i >= 10 && i < 18) _spawnPoints[i, j].isOccupied = true;
                if (j == 3 && ((i >= 3 && i <= 5) || (i >= 11 && i < 18))) _spawnPoints[i, j].isOccupied = true;
                if (j == 4 && ((i >= 2 && i <= 6) || (i >= 12 && i < 18))) _spawnPoints[i, j].isOccupied = true;
                if (j == 5 && ((i >= 3 && i <= 7) || (i >= 13 && i <= 15))) _spawnPoints[i, j].isOccupied = true;
                if (j == 6 && i >= 4 && i <= 8) _spawnPoints[i, j].isOccupied = true;
                if (j == 7 && i >= 5 && i <= 9) _spawnPoints[i, j].isOccupied = true;
                if (j == 8 && i >= 2 && i <= 10) _spawnPoints[i, j].isOccupied = true;
            }
        }
        _spawnPoints[4, 2].isOccupied = true;
        _spawnPoints[0, 6].isOccupied = true;
        _spawnPoints[14, 6].isOccupied = true;

        // isOccupiedがtrueのところにベースブロックを配置する
        for (int j = 0; j < 9; j++) {
            for (int i = 0; i < 18; i++) {
                if (_spawnPoints[i, j].isOccupied == true) {
                    GameObject _baseBlock = Instantiate(_baseBlockPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    _baseBlock.transform.parent = _builderController._wagon.transform;
                    _baseBlock.transform.localPosition = _spawnPoints[i, j].transform.localPosition;
                }
            }
        }
    }

    private void SetBaseBlock2() {
        //   0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17
        // 0 □  □  □  □  □  □  ■  ■  ■  □  □  □  □  □  □  ■  ■  ■
        // 1 □  □  □  □  □  □  ■  ■  ■  □  □  □  □  □  □  ■  ■  ■
        // 2 □  □  □  □  □  □  ■  ■  ■  □  □  □  □  □  □  ■  ■  ■
        // 3 □  □  □  □  □  □  ■  ■  ■  □  □  □  □  □  □  ■  ■  ■
        // 4 □  □  □  □  □  □  ■  ■  ■  □  □  □  □  □  □  ■  ■  ■
        // 5 □  □  □  □  □  □  ■  ■  ■  □  □  □  □  □  □  ■  ■  ■
        // 6 ■  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □
        // 7 □  □  ■  ■  □  □  □  □  □  □  □  ■  ■  □  □  □  □  □
        // 8 □  □  ■  ■  ■  □  □  □  □  □  □  ■  ■  ■  □  □  □  □

        // ベースブロックが配置されるマスのspawnPointのisOccupiedをtrueにしておく
        for (int j = 0; j <= 5; j++) {
            for (int i = 6; i < 18; i++) {
                if (i >= 6 && i <= 8) _spawnPoints[i, j].isOccupied = true;
                if (i >= 15 && i < 18) _spawnPoints[i, j].isOccupied = true;
            }
        }
        _spawnPoints[0, 6].isOccupied = true;
        _spawnPoints[2, 7].isOccupied = true;
        _spawnPoints[3, 7].isOccupied = true;
        _spawnPoints[11, 7].isOccupied = true;
        _spawnPoints[12, 7].isOccupied = true;
        _spawnPoints[2, 8].isOccupied = true;
        _spawnPoints[3, 8].isOccupied = true;
        _spawnPoints[4, 8].isOccupied = true;
        _spawnPoints[11, 8].isOccupied = true;
        _spawnPoints[12, 8].isOccupied = true;
        _spawnPoints[13, 8].isOccupied = true;

        // isOccupiedがtrueのところにベースブロックを配置する
        for (int j = 0; j < 9; j++) {
            for (int i = 0; i < 18; i++) {
                if (_spawnPoints[i, j].isOccupied == true) {
                    GameObject _baseBlock = Instantiate(_baseBlockPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    _baseBlock.transform.parent = _builderController._wagon.transform;
                    _baseBlock.transform.localPosition = _spawnPoints[i, j].transform.localPosition;
                }
            }
        }
    }

    private void SetBaseBlock3() {
        //   0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17
        // 0 ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■
        // 1 ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■
        // 2 ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■
        // 3 ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■
        // 4 ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■
        // 5 □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □
        // 6 □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □  □
        // 7 □  □  ■  ■  □  □  ■  ■  □  □  ■  ■  □  □  ■  ■  □  □
        // 8 □  □  ■  ■  □  □  ■  ■  □  □  ■  ■  □  □  ■  ■  □  □

        // ベースブロックが配置されるマスのspawnPointのisOccupiedをtrueにしておく
        for (int j = 0; j <= 4; j++) {
            for (int i = 0; i < 18; i++) {
                _spawnPoints[i, j].isOccupied = true;
            }
        }
        for (int j = 7; j < 9; j++) {
            for (int i = 2; i <= 15; i++) {
                if (i % 3 == 2 || i % 3 == 0)
                    _spawnPoints[i, j].isOccupied = true;
            }
        }

        // isOccupiedがtrueのところにベースブロックを配置する
        for (int j = 0; j < 9; j++) {
            for (int i = 0; i < 18; i++) {
                if (_spawnPoints[i, j].isOccupied == true) {
                    GameObject _baseBlock = Instantiate(_baseBlockPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    _baseBlock.transform.parent = _builderController._wagon.transform;
                    _baseBlock.transform.localPosition = _spawnPoints[i, j].transform.localPosition;
                }
            }
        }
    }
}
