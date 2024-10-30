using UnityEngine;
using UnityEngine.UI;

public class BuilderController : MonoBehaviour {
    private Vector3 _wagonStartPosition = new Vector3(5734, 11, 0);

    #region Serialized Fields
    [SerializeField] private BuildersDB _buildersDB = null;
    [SerializeField] private SetBaseBlock _setBaseBlock = null;


    [Header("ビルダー用カメラ"), SerializeField]
    private Camera builderCamera;
    // [Header("wagonの大元"), SerializeField]
    // private GameObject[] wagonOriginal;

    // [Header("ワゴン爆発エフェクト"), SerializeField]
    // private GameObject explosionEffect;
    // [SerializeField] private Clicker clicker;
    // [SerializeField]
    // private Image fillWeighingImage;
    #endregion


    #region Public Fields
    public BattleBuilderDB BattleBuilder { get; private set; } = null;


    // ObstacleButtonManagerでも使用する変数.
    [HideInInspector] public bool[] isSetWagon = { false, false, false, false, false, false, };
    [HideInInspector] public bool[] isRunningWagon = { false, false, false, false, false, false, };
    // SetBaseblock~~のスクリプトでも使用する変数.
    [HideInInspector] public GameObject _wagon;
    // 走っているwagonのWagonController.
    // BGM, CrusherControllerでも使用する変数.
    [HideInInspector] public WagonController wagonControllerRun = null;
    // WagonControllerでも使用する変数.
    [HideInInspector] public bool goButtonFirstClick = false;
    // 生成されたPrefabのWagonController.
    // ObstacleButtonManagerでも使用する変数.
    [HideInInspector] public WagonController wagonController = null;
    #endregion

    private void Awake() {
        BattleBuilder = _buildersDB.GetBattleBuilder(GameDirector.Instance.BuilderIndex);
        SetWagon(_wagonStartPosition);
    }

    private void Start() {
        // BattleBuilder = _buildersDB.GetBattleBuilder(GameDirector.Instance.BuilderIndex);
        // SetWagon(_wagonStartPosition);
        GameDirector.Instance.BuilderPosition = 5734.0f;
    }

    private void Update() {
        if (_wagon.transform.position.x < 5060.0f)
            SetWagon(new Vector3(5734, 11, 0));
        
        if (wagonControllerRun != null)
            GameDirector.Instance.BuilderPosition = wagonControllerRun.transform.position.x;
    }

    // ワゴンを配置する座標を受け取りワゴンを配置する.
    private void SetWagon(Vector3 position) {
        _wagon = Instantiate(BattleBuilder.WagonPrefab, position, Quaternion.identity, GameObject.Find("Builder").transform);
        _setBaseBlock.SetSpawnPoint();

        wagonController = _wagon.GetComponent<WagonController>();
        for (int i = 0; i < 6; i++) {
            isSetWagon[i] = true;
        }
    }

    /// <summary>
    /// ワゴンを走らせる.
    /// </summary>
    public void RunWagon() {
        wagonControllerRun = wagonController;
        wagonControllerRun.XSpeed = -wagonControllerRun.Speed;
        for (int i = 0; i < 6; i++) {
            isRunningWagon[i] = true;
        }
    }
}
