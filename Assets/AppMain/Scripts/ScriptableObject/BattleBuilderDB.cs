using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleBuilderDB", menuName = "ScriptableObjects/BattleBuilder Database")]
public class BattleBuilderDB : ScriptableObject {
    [SerializeField] private List<ObstacleDB> _obstacleDBs = new List<ObstacleDB>();

    [SerializeField] private Sprite _bg = null;
    [SerializeField] private Sprite _ground = null;
    [SerializeField] private GameObject _builderPrefab = null;
    [SerializeField] private GameObject _wagonPrefab = null;
    // [SerializeField] private List<Sprite> _obstacleImages = new List<Sprite>();
    // [SerializeField] private List<GameObject> _obstaclePrefabs = new List<GameObject>();
    // [SerializeField] private List<float> _obstacleWeight = new List<float>();

    public Sprite Bg { get => _bg; set => _bg = value; }
    public Sprite Ground { get => _ground; set => _ground = value; }
    public GameObject BuilderPrefab { get => _builderPrefab; set => _builderPrefab = value; }
    public GameObject WagonPrefab { get => _wagonPrefab; set => _wagonPrefab = value; }
    // public List<Sprite> ObstacleImages { get => _obstacleImages; set => _obstacleImages = value; }
    // public List<GameObject> ObstaclePrefabs { get => _obstaclePrefabs; set => _obstaclePrefabs = value; }
    // public List<float> ObstacleWeight { get => _obstacleWeight; set => _obstacleWeight = value; }

    /// <summary>
    /// Battleで使う障害物の情報を返す
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ObstacleDB GetObstacle(int index) {
        return _obstacleDBs[index];
    }
}
