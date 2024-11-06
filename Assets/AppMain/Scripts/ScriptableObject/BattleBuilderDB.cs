using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleBuilderDB", menuName = "ScriptableObjects/BattleBuilder Database")]
public class BattleBuilderDB : ScriptableObject {
    [SerializeField] private List<ObstacleDB> _obstacleDBs = new List<ObstacleDB>();

    [SerializeField] private Sprite _bg = null;
    [SerializeField] private Sprite _ground = null;
    [SerializeField] private GameObject _builderPrefab = null;
    [SerializeField] private GameObject _wagonPrefab = null;
    [SerializeField] private Sprite _frameSprite = null;
    [SerializeField] private List<Sprite> _faceSprites = new List<Sprite>();

    public Sprite Bg { get => _bg; set => _bg = value; }
    public Sprite Ground { get => _ground; set => _ground = value; }
    public GameObject BuilderPrefab { get => _builderPrefab; set => _builderPrefab = value; }
    public GameObject WagonPrefab { get => _wagonPrefab; set => _wagonPrefab = value; }
    public Sprite FrameSprite { get => _frameSprite; set => _frameSprite = value; }
    public List<Sprite> FaceSprites { get => _faceSprites; set => _faceSprites = value; }

    /// <summary>
    /// Battleで使う障害物の情報を返す
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ObstacleDB GetObstacle(int index) {
        return _obstacleDBs[index];
    }
}
