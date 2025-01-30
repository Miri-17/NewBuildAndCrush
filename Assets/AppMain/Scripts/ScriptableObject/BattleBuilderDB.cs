using UnityEngine;

[CreateAssetMenu(fileName = "BattleBuilderDB", menuName = "ScriptableObjects/BattleBuilder Database")]
public class BattleBuilderDB : ScriptableObject {
    #region Serialized Fields
    [SerializeField] private ObstacleDB[] _obstacleDBs = new ObstacleDB[0];
    [SerializeField] private Sprite _bg = null;
    [SerializeField] private Sprite _ground = null;
    [SerializeField] private GameObject _builderPrefab = null;
    [SerializeField] private GameObject _wagonPrefab = null;
    [SerializeField] private Sprite _frameSprite = null;
    [SerializeField] private Sprite[] _faceSprites = new Sprite[0];
    #endregion

    #region Public Properties
    public Sprite Bg { get => _bg; set => _bg = value; }
    public Sprite Ground { get => _ground; set => _ground = value; }
    public GameObject BuilderPrefab { get => _builderPrefab; set => _builderPrefab = value; }
    public GameObject WagonPrefab { get => _wagonPrefab; set => _wagonPrefab = value; }
    public Sprite FrameSprite { get => _frameSprite; set => _frameSprite = value; }
    public Sprite[] FaceSprites { get => _faceSprites; set => _faceSprites = value; }
    #endregion

    /// <summary>
    /// Battleで使う障害物の情報を返す
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ObstacleDB GetObstacle(int index) {
        return _obstacleDBs[index];
    }
}
